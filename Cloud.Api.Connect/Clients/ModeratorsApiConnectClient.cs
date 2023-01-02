namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Connect.Clients
{
    public class ModeratorsApiConnectClient : MOROPO.SAPRESSC.Core.Api.Connect.Clients.AbstractEntityApiManagementClient<Persistence.Entities.Moderator, System.Guid, Common.Models.ModeratorInputModel, Common.Models.ModeratorDisplayModel>
    {
        public ModeratorsApiConnectClient(MOROPO.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider) : base(httpClientHelperProvider)
        {
        }

        protected override string TargetController => "account/moderators";
    }
}
