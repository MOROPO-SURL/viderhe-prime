namespace SmartSolucionesCuba.Viderhe.Extend.OHCC.RailwayMuseum.Templating.Presentation.Models.View
{
    public abstract class AbstractCornerTileVideoPresentingPlacardViewModel : Common.Templating.Presentation.Models.View.AbstractPlacardViewModel
    {
        public XFToolKit.Presentation.Sources.Video.VideoSource PresenterTileVideoSource { get; set; }

        public Enumerations.ScreenCorner PresenterTileVideoScreenCorner { get; set; }

        public System.TimeSpan PresenterTileVideoPlayDelay { get; set; }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            PresenterTileVideoSource = null;
        }
    }
}
