using MOROPO.SAPRESSC.Core.Web.Security.Models.View.Contrat;

namespace SmartSolucionesCuba.Viderhe.Cloud.Managers
{
    public class UserProfileManager : MOROPO.SAPRESSC.Core.Web.Security.Managers.Facade.IUserProfileManagerFacade
    {
        public IUserProfileViewModel GetUserProfile(string username)
        {
            return new Models.View.UserProfileViewModel();
        }

        public void PreValidateUserProfile(string username, IUserProfileViewModel profileModel, Microsoft.AspNetCore.Mvc.Controller controller)
        {
        }

        public void UpdateUserProfile(string username, IUserProfileViewModel profileModel)
        {
        }
    }
}
