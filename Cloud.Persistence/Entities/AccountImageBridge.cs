namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.Entities
{
    public class AccountImageBridge
    {
        public System.Guid AccountId { get; set; }

        public Account Account { get; set; }

        public System.Guid ImageId { get; set; }

        public MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Image Image { get; set; }
    }
}
