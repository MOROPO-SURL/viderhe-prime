using System.ComponentModel.DataAnnotations;

namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class FrameInputModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Persistence.Entities.Frame, long>
    {
        public long Id { get; set; }

        [Required]
        public System.Guid ScreenplayId { get; set; }

        [Required]
        public System.Guid PlacardId { get; set; }

        [Required, Range(1, 10)]
        public int Priority { get; set; }

        public Persistence.Entities.Frame Export()
        {
            var entity = new Persistence.Entities.Frame();

            Mergue(entity);

            return entity;
        }

        public void Import(Persistence.Entities.Frame entity)
        {
            Id = entity.Id;
            ScreenplayId = entity.ScreenplayId;
            PlacardId = entity.PlacardId;
            Priority = entity.Priority;
        }

        public void Mergue(Persistence.Entities.Frame entity)
        {
            entity.Priority = Priority;
        }
    }
}
