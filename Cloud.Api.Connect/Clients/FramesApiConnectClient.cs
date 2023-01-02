namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Connect.Clients
{
    public class FramesApiConnectClient : MOROPO.SAPRESSC.Core.Api.Connect.Clients.AbstractEntityApiManagementFilteredClient<Persistence.Entities.Frame, long, Common.Models.FrameInputModel, Common.Models.FrameDisplayModel, Common.Models.Filter.FrameFilterModel>
    {
        public FramesApiConnectClient(MOROPO.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider) : base(httpClientHelperProvider)
        {
        }

        public async System.Threading.Tasks.Task OrderAsync(long id, int priority)
        {
            await httpClientHelperProvider.HttpClientHelper.PutAsync(string.Format("{0}/{1}/order/{2}", TargetController, id, priority));
        }

        protected override string TargetController => "account/frames";
    }
}
