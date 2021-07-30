namespace SmartSolucionesCuba.Viderhe.Extend.OHCC.RailwayMuseum.Templating.Presentation.Models.Context
{
    public abstract class AbstractCornerTileVideoPresentingPlacardContextModel : Common.Templating.Presentation.Models.Context.AbstractPlacardContextModel
    {
        public Common.Templating.Presentation.Models.Context.VideoContextModel PresenterTileVideo { get; set; }

        public Enumerations.ScreenCorner PresenterTileVideoScreenCorner { get; set; }

        public System.TimeSpan PresenterTileVideoPlayDelay { get; set; }
    }
}
