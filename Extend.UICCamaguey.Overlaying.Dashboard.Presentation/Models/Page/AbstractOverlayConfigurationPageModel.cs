namespace SmartSolucionesCuba.Viderhe.Extend.UICCamaguey.Overlaying.Dashboard.Presentation.Models.Page
{
    public abstract class AbstractOverlayConfigurationPageModel : XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private string head;
        private string tail;

        public string Head { get => head; set => SetProperty(ref head, value); }

        public string Tail { get => tail; set => SetProperty(ref tail, value); }
    }
}
