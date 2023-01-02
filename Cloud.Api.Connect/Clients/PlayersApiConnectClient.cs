namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Connect.Clients
{
    public class PlayersApiConnectClient : MOROPO.SAPRESSC.Core.Api.Connect.Clients.AbstractEntityApiManagementClient<Persistence.Entities.Player, string, Common.Models.PlayerInputModel, Common.Models.PlayerDisplayModel>
    {
        public PlayersApiConnectClient(MOROPO.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider) : base(httpClientHelperProvider)
        {
        }

        protected override string TargetController => "account/players";

        public async System.Threading.Tasks.Task<Common.Models.AccountDisplayModel> GetAccountAsync()
        {
            return await httpClientHelperProvider.HttpClientHelper.GetAsync<Common.Models.AccountDisplayModel>(string.Format("{0}/owner", TargetController));
        }

        public async System.Threading.Tasks.Task<Common.Models.PlayerPlaybackConfigurationModel> GetPlaybackConfigurationAsync()
        {
            return await httpClientHelperProvider.HttpClientHelper.GetAsync<Common.Models.PlayerPlaybackConfigurationModel>(string.Format("{0}/playbackconfiguration", TargetController));
        }
    }
}
