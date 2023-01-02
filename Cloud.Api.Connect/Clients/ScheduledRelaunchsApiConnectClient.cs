namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Connect.Clients
{
    public class ScheduledRelaunchsApiConnectClient : MOROPO.SAPRESSC.Core.Api.Connect.Clients.AbstractEntityApiManagementFilteredClient<Persistence.Entities.ScheduledRelaunch, System.Guid, Common.Models.ScheduledRelaunchInputModel, Common.Models.ScheduledRelaunchDisplayModel, Common.Models.Filter.ScheduledRelaunchFilterModel>
    {
        public ScheduledRelaunchsApiConnectClient(MOROPO.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider) : base(httpClientHelperProvider)
        {
        }

        protected override string TargetController => "account/scheduledrelaunchs";
    }
}
