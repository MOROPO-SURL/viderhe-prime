namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.Entities
{
    public class Template : MOROPO.SAPRESSC.Core.Persistence.Entities.IHabilitableEntity<System.Guid>
    {
        public Template()
        {
            Enabled = true;
        }

        [System.ComponentModel.DataAnnotations.Key]
        public System.Guid Id { get; set; }

        public bool Enabled { get; set; }
    }
}
