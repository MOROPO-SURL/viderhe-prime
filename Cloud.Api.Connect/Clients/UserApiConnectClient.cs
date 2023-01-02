using SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models;

namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Connect.Clients
{
    public class UserApiConnectClient
    {
        private readonly MOROPO.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper httpClientHelper;

        public UserApiConnectClient(MOROPO.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper httpClientHelper)
        {
            this.httpClientHelper = httpClientHelper;
        }

        public async System.Threading.Tasks.Task<AuthenticatedSessionModel> AuthenticateAsync(AuthenticationIntentModel modelInput)
        {
            return await httpClientHelper.PostAsync<AuthenticationIntentModel, AuthenticatedSessionModel>("security/authenticate", modelInput);
        }

        public async System.Threading.Tasks.Task ChangePasswordAsync(UserPasswordChangeRequestModel model)
        {
            await httpClientHelper.PostAsync("security/changepassword", model);
        }
    }
}
