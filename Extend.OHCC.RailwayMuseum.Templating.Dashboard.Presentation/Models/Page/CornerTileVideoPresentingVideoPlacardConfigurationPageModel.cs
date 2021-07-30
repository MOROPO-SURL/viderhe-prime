namespace SmartSolucionesCuba.Viderhe.Extend.OHCC.RailwayMuseum.Templating.Dashboard.Presentation.Models.Page
{
    public class CornerTileVideoPresentingVideoPlacardConfigurationPageModel : AbstractCornerTileVideoPresentingPlacardConfigurationPageModel
    {
        private Support.Presentation.Models.View.VideoViewModel presentedVideo;

        public Support.Presentation.Models.View.VideoViewModel PresentedVideo { get => presentedVideo; set => SetProperty(ref presentedVideo, value); }
    }
}
