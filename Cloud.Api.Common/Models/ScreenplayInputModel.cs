namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class ScreenplayInputModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Persistence.Entities.Screenplay, System.Guid>
    {
        public System.Guid Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Title { get; set; }

        public string About { get; set; }

        public Persistence.Entities.Screenplay Export()
        {
            var entity = new Persistence.Entities.Screenplay();

            Mergue(entity);

            return entity;
        }

        public void Import(Persistence.Entities.Screenplay entity)
        {
            Id = entity.Id;
            Title = entity.Title;
            About = entity.About;
        }

        public void Mergue(Persistence.Entities.Screenplay entity)
        {
            entity.Title = Title;
            entity.About = About;
        }
    }
}
