using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models;
using SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models.Filter;

namespace SmartSolucionesCuba.Viderhe.Cloud.Controllers.Frontend.Api
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Policy = Security.Authorization.Policies.ACCOUNT_MODERATION)]
    [Route("api/[area]/[controller]")]
    [Area("account")]
    public class ScheduledRelaunchsController : MOROPO.SAPRESSC.Core.Web.Api.Controllers.AbstractEntityApiManagementFilteredController<Persistence.Entities.ScheduledRelaunch, System.Guid, ScheduledRelaunchInputModel, ScheduledRelaunchDisplayModel, ScheduledRelaunchFilterModel>
    {
        public ScheduledRelaunchsController(MOROPO.SAPRESSC.Core.Persistence.Repositories.IEntityRepository<Persistence.Entities.ScheduledRelaunch, System.Guid> repository, Microsoft.Extensions.Localization.IStringLocalizer<ScheduledRelaunchsController> localizer, ILogger<ScheduledRelaunchsController> logger) : base(repository, localizer, logger)
        {
        }
    }
}
