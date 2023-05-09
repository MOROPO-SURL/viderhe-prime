namespace SmartSolucionesCuba.Viderhe.Extend.Common.Templating.Dashboard.Presentation.Models.Page
{
    public abstract class AbstractPlacardConfigurationPageModel : Support.Presentation.Dashboard.Models.Page.FormPageModel
    {
        private bool requireUnobstructedPresetation;
        private Enumerations.ConfigurationPlayerOrientationSupportKind playerOrientationSupportKind;
        private System.DateTime beginAt;
        private System.DateTime endAt;
        private bool defineBeginAt;
        private bool defineEndAt;

        public AbstractPlacardConfigurationPageModel()
        {
            AvailableConfigurationPlayerOrientationSupportKinds = new[] { Enumerations.ConfigurationPlayerOrientationSupportKind.Inherited, Enumerations.ConfigurationPlayerOrientationSupportKind.Landscape, Enumerations.ConfigurationPlayerOrientationSupportKind.Portrait };
        }

        public bool RequireUnobstructedPresetation { get => requireUnobstructedPresetation; set => SetProperty(ref requireUnobstructedPresetation, value); }

        public bool DefineBeginAt { get => defineBeginAt; set => SetProperty(ref defineBeginAt, value); }

        public System.DateTime BeginAt { get => beginAt; set => SetProperty(ref beginAt, value); }

        public bool DefineEndAt { get => defineEndAt; set => SetProperty(ref defineEndAt, value); }

        public System.DateTime EndAt { get => endAt; set => SetProperty(ref endAt, value); }

        public Enumerations.ConfigurationPlayerOrientationSupportKind PlayerOrientationSupportKind { get => playerOrientationSupportKind; set => SetProperty(ref playerOrientationSupportKind, value); }

        public Enumerations.ConfigurationPlayerOrientationSupportKind[] AvailableConfigurationPlayerOrientationSupportKinds { get; private set; }
    }
}
