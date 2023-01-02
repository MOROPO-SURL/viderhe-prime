namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Models.Page
{
    public class ConfigureModeratorPageModel : Support.Presentation.Dashboard.Models.Page.FormPageModel
    {
        private bool targetPrivileged;
        private string targetUserId;
        private bool targetUserEnabled;
        private bool targetUserPasswordChangeRequired;
        private string targetUserNewPassword;
        private string targetUserNewPasswordConfirmation;
        private string targetIdentityName;
        private string targetIdentityLastName;
        private Xamarin.Forms.ImageSource targetIdentityImageSource;
        //private System.DateTime targetIdentityBornDate;
        //private SAPRESSC.Middleware.Identity.Persistence.Enumerations.CivilStatus targetIdentityCivilStatus;
        private MOROPO.SAPRESSC.Middleware.Identity.Persistence.Enumerations.Sex targetIdentitySex;
        //private string targetIdentityAlias;
        private string targetIdentityAbout;

        public System.Guid TargetId { get; set; }

        public bool TargetPrivileged { get => targetPrivileged; set => SetProperty(ref targetPrivileged, value); }

        public string TargetUserId { get => targetUserId; set => SetProperty(ref targetUserId, value); }

        public bool TargetUserEnabled { get => targetUserEnabled; set => SetProperty(ref targetUserEnabled, value); }

        public bool TargetUserPasswordChangeRequired { get => targetUserPasswordChangeRequired; set => SetProperty(ref targetUserPasswordChangeRequired, value); }

        public string TargetUserNewPassword { get => targetUserNewPassword; set => SetProperty(ref targetUserNewPassword, value); }

        public string TargetUserNewPasswordConfirmation { get => targetUserNewPasswordConfirmation; set => SetProperty(ref targetUserNewPasswordConfirmation, value); }

        public System.Guid TargetIdentityId { get; set; }

        public string TargetIdentityName { get => targetIdentityName; set => SetProperty(ref targetIdentityName, value); }

        public string TargetIdentityLastName { get => targetIdentityLastName; set => SetProperty(ref targetIdentityLastName, value); }

        public Xamarin.Forms.ImageSource TargetIdentityImageSource { get => targetIdentityImageSource; set => SetProperty(ref targetIdentityImageSource, value); }

        public string TargetIdentityImageContentType { get; set; }

        //public System.DateTime TargetIdentityBornDate { get => targetIdentityBornDate; set => SetProperty(ref targetIdentityBornDate, value); }

        //public SAPRESSC.Middleware.Identity.Persistence.Enumerations.CivilStatus TargetIdentityCivilStatus { get => targetIdentityCivilStatus; set => SetProperty(ref targetIdentityCivilStatus, value); }

        public MOROPO.SAPRESSC.Middleware.Identity.Persistence.Enumerations.Sex TargetIdentitySex { get => targetIdentitySex; set => SetProperty(ref targetIdentitySex, value); }

        //public string TargetIdentityAlias { get => targetIdentityAlias; set => SetProperty(ref targetIdentityAlias, value); }

        public string TargetIdentityAbout { get => targetIdentityAbout; set => SetProperty(ref targetIdentityAbout, value); }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            TargetIdentityImageSource = null;
        }
    }
}
