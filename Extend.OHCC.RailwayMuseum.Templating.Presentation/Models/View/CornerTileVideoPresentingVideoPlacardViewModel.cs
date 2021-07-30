namespace SmartSolucionesCuba.Viderhe.Extend.OHCC.RailwayMuseum.Templating.Presentation.Models.View
{
    public class CornerTileVideoPresentingVideoPlacardViewModel : AbstractCornerTileVideoPresentingPlacardViewModel
    {
        public XFToolKit.Presentation.Sources.Video.VideoSource PresentedVideoSource { get; set; }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            PresentedVideoSource = null;
        }
    }
}
