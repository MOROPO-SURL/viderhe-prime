using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MOROPO.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models;
using SmartSolucionesCuba.Viderhe.Cloud.Persistence.Entities;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Cloud.Controllers.Frontend.Api
{
    // TODO Firbid requiest for unprivileged moderators!!
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Policy = Security.Authorization.Policies.ACCOUNT_VALIDATED_MODERATION)]
    [Route("api/[controller]")]
    [MOROPO.SAPRESSC.Core.Web.Security.Attributes.ChangeUserPasswordRequiredAction]
    [MOROPO.SAPRESSC.Core.Web.Security.Attributes.CatchChangeUserPasswordRequiredException]
    public class AccountController : MOROPO.SAPRESSC.Core.Web.Api.Controllers.AbstractEntityApiManagementController<Account, System.Guid, AccountInputModel, AccountDisplayModel>
    {
        private readonly MOROPO.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade.IImageFileManagerFacade imageFileManager;
        private readonly IEntityRepository<MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Image, System.Guid> imageRepository;
        private readonly IEntityRepository<Extension, System.Guid> extensionRepository;

        public AccountController(IEntityRepository<Extension, System.Guid> extensionRepository, IEntityRepository<MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Image, System.Guid> imageRepository, MOROPO.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade.IImageFileManagerFacade imageFileManager, IEntityRepository<Account, System.Guid> repository, Microsoft.Extensions.Localization.IStringLocalizer<AccountController> localizer, ILogger<AccountController> logger) : base(repository, localizer, logger)
        {
            this.imageFileManager = imageFileManager;
            this.imageRepository = imageRepository;
            this.extensionRepository = extensionRepository;
        }

        [HttpGet("[action]")]
        public ActionResult Current()
        {
            var entity = Repository.Get(System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value));

            var model = new AccountDisplayModel();

            model.Import(entity);

            foreach (var bridge in entity.ExtensionBridges)
            {
                var extension = extensionRepository.Get(bridge.ExtensionId);

                var extensionDisplayModel = new ExtensionDisplayModel();

                extensionDisplayModel.Import(extension);

                model.Extensions.Add(extensionDisplayModel);
            }

            return Ok(model);
        }

        public override ActionResult<AccountDisplayModel[]> Get()
        {
            return NotFound();
        }

        public override ActionResult<AccountDisplayModel> Get(System.Guid key)
        {
            return NotFound();
        }

        public override ActionResult<MOROPO.SAPRESSC.Core.Common.Models.PaginationModel<AccountDisplayModel>> GetPaginate(int sheet = 1, int limit = 25)
        {
            return NotFound();
        }

        public override ActionResult Create([FromBody] AccountInputModel inputModel)
        {
            return NotFound();
        }

        public override ActionResult Edit([FromBody] AccountInputModel inputModel)
        {
            if (inputModel.Id != System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value))
            {
                return Forbid();
            }

            return base.Edit(inputModel);
        }

        public override ActionResult Delete(System.Guid key)
        {
            return NotFound();
        }

        protected override void PreEdit(Account entity, AccountInputModel inputModel)
        {
            base.PreEdit(entity, inputModel);

            if (inputModel.LogoImageData != null)
            {
                using (var imageSkData = SkiaSharp.SKData.CreateCopy(inputModel.LogoImageData))
                {
                    using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
                    {
                        var image = new MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Image
                        {
                            ContentType = inputModel.LogoImageContentType,
                            Length = inputModel.LogoImageData.Length,
                            Width = imageSkBitmap.Width,
                            Height = imageSkBitmap.Height,
                            About = "Account Logo " + entity.Id
                        };

                        imageFileManager.SaveImageFileContent(image.Id, imageSkData.ToArray(), image.ContentType);

                        inputModel.LogoImageId = entity.Logo.Id;

                        entity.Logo = image;
                    }
                }
            }

            if (inputModel.HorizontalSplashImageData != null)
            {
                using (var imageSkData = SkiaSharp.SKData.CreateCopy(inputModel.HorizontalSplashImageData))
                {
                    using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
                    {
                        var image = new MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Image
                        {
                            ContentType = inputModel.HorizontalSplashImageContentType,
                            Length = inputModel.HorizontalSplashImageData.Length,
                            Width = imageSkBitmap.Width,
                            Height = imageSkBitmap.Height,
                            About = "Account Horizontal Splash " + entity.Id
                        };

                        imageFileManager.SaveImageFileContent(image.Id, imageSkData.ToArray(), image.ContentType);

                        inputModel.HorizontalSplashImageId = entity.HorizontalSplash.Id;

                        entity.HorizontalSplash = image;
                    }
                }
            }

            if (inputModel.VerticalSplashImageData != null)
            {
                using (var imageSkData = SkiaSharp.SKData.CreateCopy(inputModel.VerticalSplashImageData))
                {
                    using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
                    {
                        var image = new MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Image
                        {
                            ContentType = inputModel.VerticalSplashImageContentType,
                            Length = inputModel.VerticalSplashImageData.Length,
                            Width = imageSkBitmap.Width,
                            Height = imageSkBitmap.Height,
                            About = "Account Vertical Splash " + entity.Id
                        };

                        imageFileManager.SaveImageFileContent(image.Id, imageSkData.ToArray(), image.ContentType);

                        inputModel.VerticalSplashImageId = entity.VerticalSplash.Id;

                        entity.HorizontalSplash = image;
                    }
                }
            }

            entity.Signature = Helpers.Integrity.AccountIntegrityHelper.GetSignature(entity);
        }

        protected override void PostEdit(Account entity, AccountInputModel inputModel)
        {
            base.PostEdit(entity, inputModel);

            if (inputModel.LogoImageData != null)
            {
                var oldLogoImage = imageRepository.Get(inputModel.LogoImageId);

                imageRepository.Delete(oldLogoImage, User.Identity.Name);

                imageFileManager.DeleteImageFile(oldLogoImage);
            }

            if (inputModel.HorizontalSplashImageData != null)
            {
                var oldSplashImage = imageRepository.Get(inputModel.HorizontalSplashImageId);

                imageRepository.Delete(oldSplashImage, User.Identity.Name);

                imageFileManager.DeleteImageFile(oldSplashImage);
            }

            if (inputModel.VerticalSplashImageData != null)
            {
                var oldSplashImage = imageRepository.Get(inputModel.VerticalSplashImageId);

                imageRepository.Delete(oldSplashImage, User.Identity.Name);

                imageFileManager.DeleteImageFile(oldSplashImage);
            }
        }
    }
}
