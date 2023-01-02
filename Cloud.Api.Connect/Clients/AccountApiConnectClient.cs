namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Connect.Clients
{
    public class AccountApiConnectClient : MOROPO.SAPRESSC.Core.Api.Connect.Clients.AbstractEntityApiManagementClient<Persistence.Entities.Account, System.Guid, Common.Models.AccountInputModel, Common.Models.AccountDisplayModel>
    {
        public AccountApiConnectClient(MOROPO.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider) : base(httpClientHelperProvider)
        {
        }

        protected override string TargetController => "account";

        public async System.Threading.Tasks.Task<Common.Models.AccountDisplayModel> GetCurrentAsync()
        {
            return await httpClientHelperProvider.HttpClientHelper.GetAsync<Common.Models.AccountDisplayModel>(string.Format("{0}/current", TargetController));
        }
    }
}
