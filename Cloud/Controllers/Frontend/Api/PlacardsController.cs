using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Cloud.Controllers.Frontend.Api
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Policy = Security.Authorization.Policies.ACCOUNT_MODERATION)]
    [Route("api/[area]/[controller]")]
    [Area("account")]
    [SAPRESSC.Core.Web.Security.Filters.ChangeUserPasswordRequiredAction]
    [SAPRESSC.Core.Web.Security.Filters.CatchChangeUserPasswordRequiredException]
    public class PlacardsController : SAPRESSC.Core.Web.Api.Controllers.AbstractEntityApiManagementController<Persistence.Entities.Placard, System.Guid, PlacardInputModel, PlacardDisplayModel>
    {
        private readonly IEntityRepository<Persistence.Entities.Account, System.Guid> accountRepository;
        private readonly IEntityRepository<Persistence.Entities.Template, System.Guid> templateRepository;

        public PlacardsController(IEntityRepository<Persistence.Entities.Template, System.Guid> templateRepository, IEntityRepository<Persistence.Entities.Account, System.Guid> accountRepository, IEntityRepository<Persistence.Entities.Placard, System.Guid> repository, Microsoft.Extensions.Localization.IStringLocalizer<PlacardsController> localizer, ILogger<PlacardsController> logger) : base(repository, localizer, logger)
        {
            this.accountRepository = accountRepository;
            this.templateRepository = templateRepository;
        }

        protected override System.Linq.Expressions.Expression<System.Func<Persistence.Entities.Placard, bool>> GetGlobalSelector()
        {
            var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            return e => e.Account.Id == accountId && e.Template.Enabled;
        }

        protected override void PreCreate(Persistence.Entities.Placard entity, PlacardInputModel inputModel)
        {
            base.PreCreate(entity, inputModel);

            entity.Account = accountRepository.Get(System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value));
            entity.Template = templateRepository.Get(inputModel.TemplateId);

            var dependencyImages = Support.Extend.Templating.Helpers.Provisioning.PlacardContextDependenciesProvisioningHelper.Instance.GetImages(inputModel.TemplateId, inputModel.SerializedContext);

            if (dependencyImages != null)
            {
                foreach (var item in dependencyImages)
                {
                    entity.ImageBridges.Add(new Persistence.Entities.PlacardImageBridge
                    {
                        Placard = entity,
                        ImageId = item.Id
                    });
                }
            }

            var dependencyVideos = Support.Extend.Templating.Helpers.Provisioning.PlacardContextDependenciesProvisioningHelper.Instance.GetVideos(inputModel.TemplateId, inputModel.SerializedContext);

            if (dependencyVideos != null)
            {
                foreach (var item in dependencyVideos)
                {
                    entity.VideoBridges.Add(new Persistence.Entities.PlacardVideoBridge
                    {
                        Placard = entity,
                        VideoId = item.Id
                    });
                }
            }
        }

        protected override void PreEdit(Persistence.Entities.Placard entity, PlacardInputModel inputModel)
        {
            base.PreEdit(entity, inputModel);

            var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            if (entity.Account.Id != accountId) throw new System.InvalidOperationException();

            entity.ImageBridges.Clear();
            entity.VideoBridges.Clear();

            var dependencyImages = Support.Extend.Templating.Helpers.Provisioning.PlacardContextDependenciesProvisioningHelper.Instance.GetImages(inputModel.TemplateId, inputModel.SerializedContext);

            if (dependencyImages != null)
            {
                foreach (var item in dependencyImages)
                {
                    entity.ImageBridges.Add(new Persistence.Entities.PlacardImageBridge
                    {
                        Placard = entity,
                        ImageId = item.Id
                    });
                }
            }

            var dependencyVideos = Support.Extend.Templating.Helpers.Provisioning.PlacardContextDependenciesProvisioningHelper.Instance.GetVideos(inputModel.TemplateId, inputModel.SerializedContext);

            if (dependencyVideos != null)
            {
                foreach (var item in dependencyVideos)
                {
                    entity.VideoBridges.Add(new Persistence.Entities.PlacardVideoBridge
                    {
                        Placard = entity,
                        VideoId = item.Id
                    });
                }
            }
        }

        protected override void PreDelete(Persistence.Entities.Placard entity)
        {
            base.PreDelete(entity);

            var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            if (entity.Account.Id != accountId) throw new System.InvalidOperationException();
        }
    }
}
