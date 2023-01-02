namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Connect.Clients
{
    public class ScreenplaysApiConnectClient : MOROPO.SAPRESSC.Core.Api.Connect.Clients.AbstractEntityApiManagementClient<Persistence.Entities.Screenplay, System.Guid, Common.Models.ScreenplayInputModel, Common.Models.ScreenplayDisplayModel>
    {
        public ScreenplaysApiConnectClient(MOROPO.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider) : base(httpClientHelperProvider)
        {
        }

        protected override string TargetController => "account/screenplays";
    }
}
