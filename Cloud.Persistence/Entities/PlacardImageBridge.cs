namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.Entities
{
    public class PlacardImageBridge
    {
        public System.Guid PlacardId { get; set; }

        public Placard Placard { get; set; }

        public System.Guid ImageId { get; set; }

        public MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Image Image { get; set; }
    }
}
