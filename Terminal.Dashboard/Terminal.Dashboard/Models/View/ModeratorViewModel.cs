namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Models.View
{
    public class ModeratorViewModel
    {
        public Cloud.Api.Common.Models.ModeratorDisplayModel Model { get; set; }

        public Xamarin.Forms.ImageSource ImageSource
        {
            get
            {
                if (Model.Identity.Image != null) return Xamarin.Forms.ImageSource.FromUri(new System.Uri(string.Format("{0}://{1}/media/moderator/{2}", Support.Presentation.Managers.SessionMemoryManager.Instance.UseSSL ? "https" : "http", Support.Presentation.Managers.SessionMemoryManager.Instance.Host, Model.Id)));

                return Xamarin.Forms.ImageSource.FromUri(new System.Uri(Helpers.ConstantProvisioningHelper.Instance.FallbackUserProfileImageUrl));
            }
        }

        public string FullName => string.IsNullOrEmpty(Model.Identity.LastName) ? Model.Identity.Name : string.Format("{0} {1}", Model.Identity.Name, Model.Identity.LastName);

        public bool IsCurrentUser => Model.Id == Managers.SessionManager.Instance.Moderator.Id;
    }
}
