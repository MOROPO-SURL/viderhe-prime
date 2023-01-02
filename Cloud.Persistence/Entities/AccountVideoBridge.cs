namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.Entities
{
    public class AccountVideoBridge
    {
        public System.Guid AccountId { get; set; }

        public Account Account { get; set; }

        public System.Guid VideoId { get; set; }

        public MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Video Video { get; set; }
    }
}
