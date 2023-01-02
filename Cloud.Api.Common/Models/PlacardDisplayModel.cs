namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class PlacardDisplayModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.Placard, System.Guid>
    {
        public System.Guid Id { get; set; }

        public string Label { get; set; }

        public TemplateDisplayModel Template { get; set; }

        public Extend.Common.Templating.Enumerations.PlayerOrientationSupportKind? PlayerOrientationSupportKind { get; set; }

        public string SerializedContext { get; set; }

        public void Import(Persistence.Entities.Placard entity)
        {
            Id = entity.Id;
            Label = entity.Label;
            PlayerOrientationSupportKind = entity.PlayerOrientationSupportKind;
            SerializedContext = entity.SerializedContext;

            Template = new TemplateDisplayModel();

            Template.Import(entity.Template);
        }
    }
}
