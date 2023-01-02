namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Connect.Clients
{
    public class ExtensionsApiConnectClient
    {
        private readonly MOROPO.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper httpClientHelper;

        public ExtensionsApiConnectClient(MOROPO.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper httpClientHelper)
        {
            this.httpClientHelper = httpClientHelper;
        }

        public async System.Threading.Tasks.Task<Common.Models.ExtensionBundleModuleModel[]> GetDashboardBundleModulesAsync(System.Guid id)
        {
            return await httpClientHelper.GetAsync<Common.Models.ExtensionBundleModuleModel[]>(string.Format("account/extensions/{0}/dashboard/bundle/modules", id));
        }

        public async System.Threading.Tasks.Task<Common.Models.ExtensionBundleModuleModel[]> GetPlayerBundleModulesAsync(System.Guid id)
        {
            return await httpClientHelper.GetAsync<Common.Models.ExtensionBundleModuleModel[]>(string.Format("account/extensions/{0}/player/bundle/modules", id));
        }
    }
}
