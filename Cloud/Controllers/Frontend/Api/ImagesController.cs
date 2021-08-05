using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSolucionesCuba.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade;
using SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models;
using SmartSolucionesCuba.Viderhe.Cloud.Extensions;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Cloud.Controllers.Frontend.Api
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Policy = Security.Authorization.Policies.ACCOUNT_ASSOCIATED)]
    [Route("api/[area]/[controller]")]
    [Area("business")]
    [ApiController]
    [SAPRESSC.Core.Web.Security.Filters.ChangeUserPasswordRequiredAction]
    [SAPRESSC.Core.Web.Security.Filters.CatchChangeUserPasswordRequiredException]
    public class ImagesController : Controller
    {
        private readonly IImageFileManagerFacade imageFileManager;
        private readonly IThumbnailFileManagerFacade thumbnailFileManager;
        private readonly Persistence.EF.ApplicationDbContext dbContext;
        private readonly Microsoft.Extensions.Options.IOptions<Options.MediaThumbnailResizeOptions> mediaThumbnailResizeOptions;
        private readonly SAPRESSC.Core.Persistence.Repositories.IEntityRepository<SAPRESSC.Middleware.Media.Persistence.Entities.Image, System.Guid> imageRepository;

        public ImagesController(SAPRESSC.Core.Persistence.Repositories.IEntityRepository<SAPRESSC.Middleware.Media.Persistence.Entities.Image, System.Guid> imageRepository, IImageFileManagerFacade imageFileManager, IThumbnailFileManagerFacade thumbnailFileManager, Persistence.EF.ApplicationDbContext dbContext, Microsoft.Extensions.Options.IOptions<Options.MediaThumbnailResizeOptions> mediaThumbnailResizeOptions)
        {
            this.imageFileManager = imageFileManager;
            this.thumbnailFileManager = thumbnailFileManager;
            this.dbContext = dbContext;
            this.mediaThumbnailResizeOptions = mediaThumbnailResizeOptions;
            this.imageRepository = imageRepository;
        }

        [HttpGet]
        [HttpGet("page/{page}")]
        public ActionResult GetPage(int page = 1)
        {
            var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            var limit = 15;

            var account = dbContext.Accounts.Include(e => e.ImageBridges).ThenInclude(b => b.Image).ThenInclude(e => e.Thumbnail).First(e => e.Id == accountId);

            var maxCount = account.ImageBridges.Count;

            var rest = maxCount % limit;

            var maxPageCount = maxCount / limit;

            if (rest > 0) maxPageCount += 1;

            if (page < 0 || page > maxPageCount)
            {
                page = 1;
            }

            return Ok(new SAPRESSC.Core.Common.Models.PaginationModel<ImageDisplayModel>
            {
                List = account.ImageBridges.Select(bridge => bridge.Image).OrderByDescending(e => e.CreateAt).Skip((page - 1) * limit).Take(limit).Select(entity =>
                {
                    var model = new ImageDisplayModel();

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

        [HttpPost]
        public ActionResult Add(Microsoft.AspNetCore.Http.IFormFile file, [FromForm] string about)
        {
            var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            var account = dbContext.Accounts.First(e => e.Id == accountId);

            if (file == null)
            {
                file = Request.Form.Files.First();
            }

            using (var imageSkData = SkiaSharp.SKData.Create(file.OpenReadStream(), file.Length))
            {
                using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
                {
                    var image = new SAPRESSC.Middleware.Media.Persistence.Entities.Image
                    {
                        ContentType = file.ContentType,
                        Length = file.Length,
                        Width = imageSkBitmap.Width,
                        Height = imageSkBitmap.Height,
                        About = about
                    };

                    imageFileManager.SaveImageFileContent(image.Id, imageSkData.ToArray(), image.ContentType);

                    SkiaSharp.SKImageInfo desiredImageInfo;

                    if (imageSkBitmap.Width > imageSkBitmap.Height)
                    {
                        desiredImageInfo = imageSkBitmap.Info.LandscapeScale(mediaThumbnailResizeOptions.Value.DesiredWidth);
                    }
                    else
                    {
                        desiredImageInfo = imageSkBitmap.Info.PortraitScale(mediaThumbnailResizeOptions.Value.DesiredHeigth);
                    }

                    using (var thumbnailSkBitmap = imageSkBitmap.Resize(desiredImageInfo, SkiaSharp.SKFilterQuality.High))
                    {
                        using (var thumbnailSkImage = SkiaSharp.SKImage.FromBitmap(thumbnailSkBitmap))
                        {
                            var thumbnailSkData = thumbnailSkImage.Encode(SkiaSharp.SKEncodedImageFormat.Png, 75);

                            image.Thumbnail = new SAPRESSC.Middleware.Media.Persistence.Entities.Thumbnail
                            {
                                ContentType = "image/png",
                                Length = thumbnailSkData.Size,
                                About = string.Format(string.Format("Thumbnail: {0}", about))
                            };

                            thumbnailFileManager.SaveThumbnailFileContent(image.Thumbnail.Id, thumbnailSkData.ToArray(), image.Thumbnail.ContentType);
                        }
                    }

                    dbContext.Images.Add(image);

                    dbContext.AccountImageBridges.Add(new Persistence.Entities.AccountImageBridge
                    {
                        AccountId = accountId,
                        ImageId = image.Id
                    });

                    dbContext.SaveChanges(User.Identity.Name);
                }
            }

            return Ok();
        }

        [HttpPut]
        public IActionResult Edit([FromBody] ImageInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

                    var account = dbContext.Accounts.Include(e => e.ImageBridges).ThenInclude(b => b.Image).First(e => e.Id == accountId);

                    var entity = account.ImageBridges.First(b => b.ImageId == inputModel.Id).Image;

                    inputModel.Mergue(entity);

                    imageRepository.Update(entity, User.Identity.Name);

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

        [HttpDelete("{key}")]
        public ActionResult Delete(System.Guid key)
        {
            var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            var account = dbContext.Accounts.Include(e => e.ImageBridges).ThenInclude(b => b.Image).ThenInclude(e => e.Thumbnail).First(e => e.Id == accountId);

            var bridge = account.ImageBridges.First(b => b.ImageId == key);

            dbContext.AccountImageBridges.Remove(bridge);

            dbContext.Images.Remove(bridge.Image);

            dbContext.Thumbnails.Remove(bridge.Image.Thumbnail);

            dbContext.SaveChanges(User.Identity.Name);

            imageFileManager.DeleteImageFile(bridge.Image);

            thumbnailFileManager.DeleteThumbnailFile(bridge.Image.Thumbnail);

            return Ok();
        }
    }
}
