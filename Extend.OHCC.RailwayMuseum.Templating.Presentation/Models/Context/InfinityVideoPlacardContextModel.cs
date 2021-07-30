namespace SmartSolucionesCuba.Viderhe.Extend.OHCC.RailwayMuseum.Templating.Presentation.Models.Context
{
    public class InfinityVideoPlacardContextModel : Common.Templating.Presentation.Models.Context.AbstractPlacardContextModel
    {
        public System.Guid VideoId { get; set; }

        public string VideoExtension { get; set; }

        public System.Guid ThumbnailId { get; set; }

        public string ThumbnailContentType { get; set; }
    }
}
