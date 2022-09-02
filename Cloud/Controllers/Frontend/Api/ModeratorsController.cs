using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.SAPRESSC.Core.Web.SecurityLocal.Extensions;
using SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Cloud.Controllers.Frontend.Api
{
    // TODO Firbid requiest for unprivileged moderators!!
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Policy = Security.Authorization.Policies.ACCOUNT_MODERATION)]
    [Route("api/[area]/[controller]")]
    [Area("account")]
    [ApiController]
    public class ModeratorsController : SAPRESSC.Core.Web.Api.Controllers.AbstractEntityApiManagementController<Persistence.Entities.Moderator, System.Guid, ModeratorInputModel, ModeratorDisplayModel>
    {
        private readonly SAPRESSC.Middleware.Media.Web.Common.Managers.Facade.IImageFileManagerFacade imageFileManager;

        public ModeratorsController(SAPRESSC.Middleware.Media.Web.Common.Managers.Facade.IImageFileManagerFacade imageFileManager, IEntityRepository<Persistence.Entities.Moderator, System.Guid> repository, Microsoft.Extensions.Localization.IStringLocalizer<ModeratorsController> localizer, ILogger<ModeratorsController> logger) : base(repository, localizer, logger)
        {
            this.imageFileManager = imageFileManager;
        }

        protected override System.Linq.Expressions.Expression<System.Func<Persistence.Entities.Moderator, bool>> GetGlobalSelector()
        {
            var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            return e => e.AccountBridges.Any(b => b.AccountId == accountId);
        }

        public override ActionResult<ModeratorDisplayModel[]> Get()
        {
            return Ok(Repository.FindBy(GetGlobalSelector()).Select(entity => EntityToDisplayModel(entity)).ToArray());
        }

        protected override ModeratorDisplayModel EntityToDisplayModel(Persistence.Entities.Moderator entity)
        {
            return base.EntityToDisplayModel(entity);
        }

        protected override void PreCreate(Persistence.Entities.Moderator entity, ModeratorInputModel inputModel)
        {
            base.PreCreate(entity, inputModel);

            entity.AccountBridges.Add(new Persistence.Entities.AccountModeratorBridge
            {
                ModeratorId = entity.Id,
                AccountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value)
            });

            if (inputModel.User.NewPassword != null)
            {
                entity.User.Password = inputModel.User.NewPassword.ToHashedPassword();
            }

            if (inputModel.Identity.Image != null)
            {
                using (var imageMemoryStream = new System.IO.MemoryStream(inputModel.Identity.Image.ContentData))
                {
                    using (var imageSkData = SkiaSharp.SKData.Create(imageMemoryStream))
                    {
                        using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
                        {
                            entity.Identity.Image = new SAPRESSC.Middleware.Media.Persistence.Entities.Image
                            {
                                ContentType = inputModel.Identity.Image.ContentType,
                                Length = inputModel.Identity.Image.ContentData.LongLength,
                                Width = imageSkBitmap.Width,
                                Height = imageSkBitmap.Height,
                                About = string.Format("Image for Moderator: {0}", inputModel.Id)
                            };

                            imageFileManager.SaveImageFileContent(entity.Identity.Image.Id, inputModel.Identity.Image.ContentData, inputModel.Identity.Image.ContentType);
                        }
                    }
                }
            }

            entity.User.Signature = SAPRESSC.Core.Web.SecurityLocal.Helpers.Integrity.UserIntegrityHelper.GetSignature(entity.User);
        }
    }
}
