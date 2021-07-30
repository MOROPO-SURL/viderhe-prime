namespace SmartSolucionesCuba.Viderhe.Extend.UICCamaguey.Overlaying.Dashboard.Presentation.Models.Page
{
    public class RSSOverlayConfigurationPageModel : AbstractOverlayConfigurationPageModel
    {
        private string feedUrl;

        public string FeedUrl { get => feedUrl; set => SetProperty(ref feedUrl, value); }
    }
}
