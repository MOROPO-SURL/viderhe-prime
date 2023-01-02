namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class OverlayDisplayModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.Overlay, System.Guid>
    {
        public System.Guid Id { get; set; }

        public bool Enabled { get; set; }

        public void Import(Persistence.Entities.Overlay entity)
        {
            Id = entity.Id;
            Enabled = entity.Enabled;
        }
    }
}
