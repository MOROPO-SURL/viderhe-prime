namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class ScreenplayDisplayModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.Screenplay, System.Guid>
    {
        public System.Guid Id { get; set; }

        public string Title { get; set; }

        public string About { get; set; }

        public void Import(Persistence.Entities.Screenplay entity)
        {
            Id = entity.Id;
            Title = entity.Title;
            About = entity.About;
        }
    }
}
