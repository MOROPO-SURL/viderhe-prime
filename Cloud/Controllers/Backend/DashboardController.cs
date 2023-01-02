using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.Viderhe.Cloud.Extensions;

namespace SmartSolucionesCuba.Viderhe.Cloud.Controllers.Backend
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [MOROPO.SAPRESSC.Core.Web.Security.Attributes.RequirePermissions(MOROPO.SAPRESSC.Core.Security.Common.Web.Backend.Constants.Permission.ShellManager)]
    [MOROPO.SAPRESSC.Core.Web.Security.Attributes.ChangeUserPasswordRequiredAction]
    [MOROPO.SAPRESSC.Core.Web.Security.Attributes.CatchChangeUserPasswordRequiredException]
    [Route("management/[controller]")]
    [Route("{culture:regex(^[[a-z]]{{2}}(?:-[[A-Z]]{{2}})?$)}/management/[controller]")]
    public class DashboardController : MOROPO.SAPRESSC.Core.Web.Common.Controllers.BaseWebController
    {
        private readonly System.Collections.Generic.IEnumerable<MOROPO.SAPRESSC.Core.Web.Backend.Providers.Facade.IManagementDashboardSectionProviderFacade> managementDashboardSectionProviders;

        public DashboardController(System.Collections.Generic.IEnumerable<MOROPO.SAPRESSC.Core.Web.Backend.Providers.Facade.IManagementDashboardSectionProviderFacade> managementDashboardSectionProviders, Microsoft.Extensions.Localization.IStringLocalizer<DashboardController> localizer, ILogger<DashboardController> logger) : base(localizer, logger)
        {
            this.managementDashboardSectionProviders = managementDashboardSectionProviders;
        }

        public IActionResult Index()
        {
            return View(new Models.View.ManagementDashboardViewModel { Sections = managementDashboardSectionProviders.ToManagementDashboardSectionGroups() });
        }
    }
}
