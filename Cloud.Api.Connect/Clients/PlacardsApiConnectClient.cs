namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Connect.Clients
{
    public class PlacardsApiConnectClient : MOROPO.SAPRESSC.Core.Api.Connect.Clients.AbstractEntityApiManagementClient<Persistence.Entities.Placard, System.Guid, Common.Models.PlacardInputModel, Common.Models.PlacardDisplayModel>
    {
        public PlacardsApiConnectClient(MOROPO.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider) : base(httpClientHelperProvider)
        {
        }

        protected override string TargetController => "account/placards";
    }
}
