namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.Entities
{
    public class AccountExtensionBridge : MOROPO.SAPRESSC.Core.Persistence.Entities.IPersistenceSignable
    {
        public System.Guid AccountId { get; set; }

        public Account Account { get; set; }

        public System.Guid ExtensionId { get; set; }

        public Extension Extension { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Signature { get; set; }

        public object GetSignableEntityModel()
        {
            return new { AccountId, ExtensionId };
        }
    }
}
