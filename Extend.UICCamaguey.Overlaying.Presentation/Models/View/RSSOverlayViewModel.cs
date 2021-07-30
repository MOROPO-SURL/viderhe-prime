namespace SmartSolucionesCuba.Viderhe.Extend.UICCamaguey.Overlaying.Presentation.Models.View
{
    public class RSSOverlayViewModel : TextOverlayViewModel
    {
        private System.Uri feedUrl;

        public System.Uri FeedUrl { get => feedUrl; set => SetProperty(ref feedUrl, value); }
    }
}
