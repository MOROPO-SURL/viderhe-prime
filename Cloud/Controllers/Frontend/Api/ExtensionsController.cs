using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Cloud.Controllers.Frontend.Api
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Policy = Security.Authorization.Policies.ACCOUNT_MODERATION)]
    [Route("api/[area]/[controller]")]
    [Area("account")]
    [ApiController]
    public class ExtensionsController : SAPRESSC.Core.Web.Common.Controllers.BaseWebController
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;

        public ExtensionsController(Microsoft.Extensions.Configuration.IConfiguration configuration, Microsoft.Extensions.Localization.IStringLocalizer<ExtensionsController> localizer, ILogger<ExtensionsController> logger) : base(localizer, logger)
        {
            this.configuration = configuration;
        }

        [HttpGet("{key}/dashboard/bundle/modules")]
        public ActionResult<Cloud.Api.Common.Models.ExtensionBundleModuleModel[]> DashboardBundleModules(System.Guid key)
        {
            var extensionRootDirectoryPath = configuration["Viderhe:ExtensionRootDirectoryPath"];

            var extensionRootDirectoryInfo = new System.IO.DirectoryInfo(extensionRootDirectoryPath);

            if (!extensionRootDirectoryInfo.Exists)
            {
                return NoContent();
            }

            var extensionLibraryFileInfos = Providers.ViderheExtensionLibraryFileInfosProvider.GetRelevantToDashboard(extensionRootDirectoryInfo.GetDirectories().First(d => d.Name == key.ToString()));

            return Ok(BuildResultModel(extensionLibraryFileInfos));
        }

        [HttpGet("{key}/player/bundle/modules")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public ActionResult<Cloud.Api.Common.Models.ExtensionBundleModuleModel[]> PlayerBundleModules(System.Guid key)
        {
            var extensionRootDirectoryPath = configuration["Viderhe:ExtensionRootDirectoryPath"];

            var extensionRootDirectoryInfo = new System.IO.DirectoryInfo(extensionRootDirectoryPath);

            if (!extensionRootDirectoryInfo.Exists)
            {
                return NoContent();
            }

            var extensionLibraryFileInfos = Providers.ViderheExtensionLibraryFileInfosProvider.GetRelevantToPlayer(extensionRootDirectoryInfo.GetDirectories().First(d => d.Name == key.ToString()));            

            return Ok(BuildResultModel(extensionLibraryFileInfos));
        }

        private Cloud.Api.Common.Models.ExtensionBundleModuleModel[] BuildResultModel(System.Collections.Generic.IEnumerable<System.IO.FileInfo> extensionLibraryFileInfos)
        {
            var model = new Cloud.Api.Common.Models.ExtensionBundleModuleModel[extensionLibraryFileInfos.Count()];

            var iterationCount = 0;

            foreach (var extensionLibraryFileInfo in extensionLibraryFileInfos)
            {
                using (var extensionLibraryMemoryStream = new System.IO.MemoryStream((int)extensionLibraryFileInfo.Length))
                {
                    using (var extensionLibraryFileStrean = extensionLibraryFileInfo.OpenRead())
                    {
                        var extensionLibraryFileContent = new byte[extensionLibraryFileInfo.Length];

                        extensionLibraryFileStrean.CopyTo(extensionLibraryMemoryStream);

                        model[iterationCount++] = new Cloud.Api.Common.Models.ExtensionBundleModuleModel
                        {
                            FileName = extensionLibraryFileInfo.Name,
                            FileContent = extensionLibraryMemoryStream.ToArray()
                        };
                    }
                }
            }

            return model;
        }
    }
}
