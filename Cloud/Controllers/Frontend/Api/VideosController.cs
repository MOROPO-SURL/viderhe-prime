using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities;
using MOROPO.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade;
using SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Cloud.Controllers.Frontend.Api
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Policy = Security.Authorization.Policies.ACCOUNT_MODERATION)]
    [Route("api/[area]/[controller]")]
    [Area("account")]
    [ApiController]
    [MOROPO.SAPRESSC.Core.Web.Security.Attributes.ChangeUserPasswordRequiredAction]
    [MOROPO.SAPRESSC.Core.Web.Security.Attributes.CatchChangeUserPasswordRequiredException]
    public class VideosController : Controller
    {
        private readonly ILogger<VideosController> logger;
        private readonly IVideoFileManagerFacade videoFileManager;
        private readonly IThumbnailFileManagerFacade thumbnailFileManager;
        private readonly Persistence.EF.ApplicationDbContext dbContext;
        private readonly MOROPO.SAPRESSC.Core.Persistence.Repositories.IEntityRepository<Video, System.Guid> videoRepository;
        private readonly MediaToolkit.Services.IMediaToolkitService mediaToolkitService;

        public VideosController(MediaToolkit.Services.IMediaToolkitService mediaToolkitService, MOROPO.SAPRESSC.Core.Persistence.Repositories.IEntityRepository<Video, System.Guid> videoRepository, IVideoFileManagerFacade videoFileManager, IThumbnailFileManagerFacade thumbnailFileManager, Persistence.EF.ApplicationDbContext dbContext, ILogger<VideosController> logger)
        {
            this.logger = logger;
            this.videoFileManager = videoFileManager;
            this.thumbnailFileManager = thumbnailFileManager;
            this.dbContext = dbContext;
            this.videoRepository = videoRepository;
            this.mediaToolkitService = mediaToolkitService;
        }

        [HttpGet]
        [HttpGet("page/{page}")]
        public ActionResult<MOROPO.SAPRESSC.Core.Common.Models.PaginationModel<VideoDisplayModel>> GetPage(int page = 1)
        {
            var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            var limit = 16;

            var account = dbContext.Accounts.Include(e => e.VideoBridges).ThenInclude(b => b.Video).ThenInclude(e => e.Thumbnail).First(e => e.Id == accountId);

            var maxCount = account.VideoBridges.Count;

            var rest = maxCount % limit;

            var maxPageCount = maxCount / limit;

            if (rest > 0) maxPageCount += 1;

            if (page < 0 || page > maxPageCount)
            {
                page = 1;
            }

            return Ok(new MOROPO.SAPRESSC.Core.Common.Models.PaginationModel<VideoDisplayModel>
            {
                List = account.VideoBridges.Select(bridge => bridge.Video).OrderByDescending(b => b.CreateAt).Skip((page - 1) * limit).Take(limit).Select(entity =>
                {
                    var model = new VideoDisplayModel();

                    model.Import(entity);

                    return model;
                }).ToList(),
                Limit = limit,
                Page = page,
                HasPrevious = page > 1,
                HasNext = page < maxPageCount,
                MaxPageCount = maxPageCount
            });
        }

        [HttpPut("{videoId}/segment/{segment}")]
        [HttpPut("{videoId}/segment/last")]
        public async System.Threading.Tasks.Task<ActionResult> PushAsync(System.Guid videoId, Microsoft.AspNetCore.Http.IFormFile file, [FromForm] string about, int segment = 0)
        {
            var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            var account = dbContext.Accounts.First(e => e.Id == accountId);

            if (file == null)
            {
                file = Request.Form.Files.First();
            }

            using (var memoryStream = new System.IO.MemoryStream())
            {
                file.CopyTo(memoryStream);

                if (segment > 0)
                {
                    videoFileManager.AddVideoFileContentSegment(videoId, segment, memoryStream.ToArray());
                }
                else
                {
                    var video = new Video
                    {
                        Id = videoId,
                        Thumbnail = new Thumbnail
                        {
                            About = string.Format("Thumbnail: {0}", about),
                            ContentType = "image/jpeg"
                        },
                        Extension = System.IO.Path.GetExtension(file.FileName),
                        About = about
                    };

                    videoFileManager.AddVideoFileContentSegmentAndClose(videoId, memoryStream.ToArray(), video.Extension);

                    try
                    {
                        var videoPath = videoFileManager.GetVideoAbsoluteFilePath(video);
                        var thumbnailPath = thumbnailFileManager.GetThumbnailAbsoluteFilePath(video.Thumbnail);

                        var getMetadataTask = new MediaToolkit.Tasks.FfTaskGetMetadata(videoFileManager.GetVideoAbsoluteFilePath(video));

                        var getMetadataResult = await mediaToolkitService.ExecuteAsync(getMetadataTask);

                        var videoMediaStream = getMetadataResult.Metadata.Streams.First(stream => stream.CodecType == "video");
                        var auduioMediaStream = getMetadataResult.Metadata.Streams.FirstOrDefault(stream => stream.CodecType == "audio");

                        var videoDuration = System.TimeSpan.FromSeconds(double.Parse(getMetadataResult.Metadata.Format.Duration.Replace(".", ",")));

                        var saveThumbnailTask = new MediaToolkit.Tasks.FfTaskSaveThumbnail(
                            videoPath,
                            thumbnailPath,
                            System.TimeSpan.FromSeconds(10)
                        );

                        await mediaToolkitService.ExecuteAsync(saveThumbnailTask);

                        try
                        {
                            var thumbnailMediaFileInfo = new System.IO.FileInfo(thumbnailPath);

                            video.Thumbnail.Length = thumbnailMediaFileInfo.Length;

                            video.Length = memoryStream.Length;
                            video.Duration = videoDuration;
                            video.Format = string.Format("{0} ({1})", videoMediaStream.CodecName, videoMediaStream.CodecLongName);
                            video.FrameSize = string.Format("{0}x{1}", videoMediaStream.Width, videoMediaStream.Height);
                            video.Fps = 0;
                            video.ColorModel = videoMediaStream.PixFmt;
                            video.AudioFormat = auduioMediaStream?.CodecLongName ?? "Unknown";
                            video.AudioChannelOutput = auduioMediaStream?.ChannelLayout ?? "Unknown";

                            dbContext.Videos.Add(video);

                            var accountVideoBridge = new Persistence.Entities.AccountVideoBridge
                            {
                                AccountId = account.Id,
                                VideoId = videoId
                            };

                            dbContext.AccountVideoBridges.Add(accountVideoBridge);

                            dbContext.SaveChanges(User.Identity.Name);
                        }
                        catch (System.Exception ex)
                        {
                            thumbnailFileManager.DeleteThumbnailFile(video.Thumbnail);

                            throw ex;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        videoFileManager.DeleteVideoFile(videoId, video.Extension);

                        throw ex;
                    }
                }
            }

            return Ok();
        }

        [HttpPut]
        public ActionResult Edit([FromBody] VideoInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

                    var account = dbContext.Accounts.Include(e => e.VideoBridges).ThenInclude(b => b.Video).First(e => e.Id == accountId);

                    var entity = account.VideoBridges.First(b => b.VideoId == inputModel.Id).Video;

                    inputModel.Mergue(entity);

                    videoRepository.Update(entity, User.Identity.Name);

                    return Ok();
                }
                catch (System.InvalidOperationException)
                {
                    return NoContent();
                }
                catch (System.Exception)
                {
                    return Conflict();
                }
            }

            return BadRequest();
        }

        [HttpDelete("{videoId}")]
        public ActionResult Delete(System.Guid videoId)
        {
            if (dbContext.PlacardVideoBridges.Any(item => item.VideoId == videoId))
            {
                return Conflict();
            }

            var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            var account = dbContext.Accounts.Include(e => e.VideoBridges).ThenInclude(b => b.Video).ThenInclude(e => e.Thumbnail).First(e => e.Id == accountId);

            var bridge = account.VideoBridges.First(b => b.VideoId == videoId);

            dbContext.AccountVideoBridges.Remove(bridge);

            dbContext.Videos.Remove(bridge.Video);

            dbContext.Thumbnails.Remove(bridge.Video.Thumbnail);

            dbContext.SaveChanges(User.Identity.Name);

            videoFileManager.DeleteVideoFile(bridge.VideoId, bridge.Video.Extension);

            thumbnailFileManager.DeleteThumbnailFile(bridge.Video.Thumbnail);

            return Ok();
        }
    }
}
