using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade;
using SmartSolucionesCuba.Viderhe.Cloud.Extensions;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Cloud.Controllers.Frontend
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Policy = Security.Authorization.Policies.ACCOUNT_ASSOCIATED)]
    [Route("[controller]")]
    public class MediaController : SAPRESSC.Core.Web.Common.Controllers.BaseWebController
    {
        private readonly SAPRESSC.Core.Persistence.Repositories.IEntityRepository<Persistence.Entities.Account, System.Guid> accountRepository;
        private readonly SAPRESSC.Core.Persistence.Repositories.IEntityRepository<Persistence.Entities.Player, string> playerRepository;
        private readonly IImageFileManagerFacade imageFileManager;
        private readonly IThumbnailFileManagerFacade thumbnailFileManager;

        public MediaController(SAPRESSC.Core.Persistence.Repositories.IEntityRepository<Persistence.Entities.Player, string> playerRepository, SAPRESSC.Core.Persistence.Repositories.IEntityRepository<Persistence.Entities.Account, System.Guid> accountRepository, IImageFileManagerFacade imageFileManager, IThumbnailFileManagerFacade thumbnailFileManager, Microsoft.Extensions.Localization.IStringLocalizer<MediaController> localizer, ILogger<MediaController> logger)
            : base(localizer, logger)
        {
            this.accountRepository = accountRepository;
            this.playerRepository = playerRepository;
            this.imageFileManager = imageFileManager;
            this.thumbnailFileManager = thumbnailFileManager;
        }

        [HttpGet("[action]/{key}")]
        public IActionResult Image(System.Guid key)
        {
            try
            {
                var account = accountRepository.Get(System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value));

                var image = account.ImageBridges.First(b => b.ImageId == key).Image;

                return new FileContentResult(imageFileManager.GetImageFileContent(image), image.ContentType);
            }
            catch (System.IO.IOException e)
            {
                logger.LogError(e.Message, e.InnerException);
            }
            catch { }

            return new NoContentResult();
        }

        [HttpGet("[action]/{key}/w/{width}/h/{height}")]
        public IActionResult Image(System.Guid key, int width, int height)
        {
            try
            {
                var accountId = System.Guid.Parse(Request.Headers[Cloud.Api.Common.Providers.Constant.SecurityConstantProvider.ACCOUNT_ID_HEADER_KEY]);

                var protectedPlayerId = Request.Headers[Cloud.Api.Common.Providers.Constant.SecurityConstantProvider.PROTECTED_PLAYER_ID_HEADER_KEY];

                var playerId = Helpers.Security.PlayerSecurityHelper.UnprotectPlayerId(protectedPlayerId);

                var player = playerRepository.Get(playerId);

                if (player.Account.Id != accountId)
                {
                    return Forbid(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme);
                }
                
                var account = accountRepository.GetAll().Include(e => e.ImageBridges).ThenInclude(b => b.Image).First(e => e.Id == accountId);

                var image = account.ImageBridges.FirstOrDefault(b => b.ImageId == key)?.Image;

                if (image == null)
                {
                    if (account.LogoId == key)
                    {
                        image = account.Logo;
                    }
                    else if (account.HorizontalSplashId == key)
                    {
                        image = account.HorizontalSplash;
                    }
                    else if (account.VerticalSplashId == key)
                    {
                        image = account.VerticalSplash;
                    }
                    else
                    {
                        return new NoContentResult();
                    }
                }

                if (image.Width > 0 && image.Height > 0 && image.Width <= width && image.Height <= height)
                {
                    return new FileContentResult(imageFileManager.GetImageFileContent(image), image.ContentType);
                }

                byte[] resultContent = null;

                using (var imageStream = new System.IO.MemoryStream(imageFileManager.GetImageFileContent(image)))
                {
                    using (var imageSkData = SkiaSharp.SKData.Create(imageStream))
                    {
                        using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
                        {
                            SkiaSharp.SKImageInfo desiredImageInfo;

                            if (imageSkBitmap.Width <= width && imageSkBitmap.Height <= height)
                            {
                                return new FileContentResult(imageFileManager.GetImageFileContent(image), image.ContentType);
                            }

                            logger.LogDebug(">>>>>>>>>>>>>>>>> Image shake required!!");

                            if (imageSkBitmap.Width > imageSkBitmap.Height)
                            {
                                desiredImageInfo = imageSkBitmap.Info.LandscapeScale(width);
                            }
                            else
                            {
                                desiredImageInfo = imageSkBitmap.Info.PortraitScale(height);
                            }

                            using (var cropSkBitmap = imageSkBitmap.Resize(desiredImageInfo, SkiaSharp.SKFilterQuality.High))
                            {
                                using (var cropSkImage = SkiaSharp.SKImage.FromBitmap(cropSkBitmap))
                                {
                                    var cropSkData = cropSkImage.Encode(SkiaSharp.SKEncodedImageFormat.Png, 75);

                                    resultContent = cropSkData.ToArray();
                                }
                            }
                        }
                    }
                }

                return new FileContentResult(resultContent, "image/png");
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                logger.LogError(e.Message, e.InnerException);
            }
            catch { }

            return new NoContentResult();
        }

        [HttpGet("thumbnail/image/{key}")]
        public IActionResult ImageThumbnail(System.Guid key)
        {
            try
            {
                var account = accountRepository.Get(System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value));

                var thumbnail = account.ImageBridges.First(b => b.Image.Thumbnail.Id == key).Image.Thumbnail;

                return new FileContentResult(thumbnailFileManager.GetThumbnailFileContent(thumbnail), thumbnail.ContentType);
            }
            catch (System.IO.IOException e)
            {
                logger.LogError(e.Message, e.InnerException);
            }
            catch { }

            return new NoContentResult();
        }

        [HttpGet("thumbnail/video/{key}")]
        public IActionResult VideoThumbnail(System.Guid key)
        {
            try
            {
                var account = accountRepository.Get(System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value));

                var thumbnail = account.VideoBridges.First(b => b.Video.Thumbnail.Id == key).Video.Thumbnail;

                return new FileContentResult(thumbnailFileManager.GetThumbnailFileContent(thumbnail), thumbnail.ContentType);
            }
            catch (System.IO.IOException e)
            {
                logger.LogError(e.Message, e.InnerException);
            }
            catch { }

            return new NoContentResult();
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpGet("[action]/{key}")]
        public IActionResult Moderator(string key)
        {
            try
            {
                var account = accountRepository.Get(System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value));

                var moderator = account.ModeratorBridges.First(b => b.Moderator.UserId == key).Moderator;

                return new FileContentResult(imageFileManager.GetImageFileContent(moderator.Identity.Image), account.Logo.ContentType);
            }
            catch (System.IO.IOException e)
            {
                logger.LogError(e.Message, e.InnerException);
            }
            catch { }

            return new NoContentResult();
        }

        [HttpGet("[action]")]
        public IActionResult Logo()
        {
            try
            {
                var account = accountRepository.Get(System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value));

                return new FileContentResult(imageFileManager.GetImageFileContent(account.Logo), account.Logo.ContentType);
            }
            catch (System.IO.IOException e)
            {
                logger.LogError(e.Message, e.InnerException);
            }
            catch { }

            return new NoContentResult();
        }

        [HttpGet("[action]")]
        public IActionResult Splash()
        {
            try
            {
                var account = accountRepository.Get(System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value));

                return new FileContentResult(imageFileManager.GetImageFileContent(account.HorizontalSplash), account.HorizontalSplash.ContentType);
            }
            catch (System.IO.IOException e)
            {
                logger.LogError(e.Message, e.InnerException);
            }
            catch { }

            return new NoContentResult();
        }
    }
}
