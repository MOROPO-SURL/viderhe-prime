namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class TransitionDisplayModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.Transition, System.Guid>
    {
        public System.Guid Id { get; set; }

        public bool Enabled { get; set; }

        public void Import(Persistence.Entities.Transition entity)
        {
            Id = entity.Id;
            Enabled = entity.Enabled;
        }
    }
}
