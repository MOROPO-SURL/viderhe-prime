namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Models.View
{
    public class ModeratorViewModel
    {
        public Cloud.Api.Common.Models.ModeratorDisplayModel Model { get; set; }

        public Xamarin.Forms.ImageSource ImageSource => Model.Identity.Image != null ?
            Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(Model.Identity.Image.Id, true)) :
            Xamarin.Forms.ImageSource.FromUri(new System.Uri(Helpers.ConstantProvisioningHelper.Instance.FallbackUserProfileImageUrl));

        public string FullName => string.IsNullOrEmpty(Model.Identity.LastName) ? Model.Identity.Name : string.Format("{0} {1}", Model.Identity.Name, Model.Identity.LastName);

        public bool IsCurrentUser => Model.Id == Managers.SessionManager.Instance.Moderator.Id;
    }
}
