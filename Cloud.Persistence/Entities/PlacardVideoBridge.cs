namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.Entities
{
    public class PlacardVideoBridge
    {
        public System.Guid PlacardId { get; set; }

        public Placard Placard { get; set; }

        public System.Guid VideoId { get; set; }

        public MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Video Video { get; set; }
    }
}
