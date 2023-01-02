namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class TemplateDisplayModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.Template, System.Guid>
    {
        public System.Guid Id { get; set; }

        public bool Enabled { get; set; }

        public void Import(Persistence.Entities.Template entity)
        {
            Id = entity.Id;
            Enabled = entity.Enabled;
        }
    }
}
