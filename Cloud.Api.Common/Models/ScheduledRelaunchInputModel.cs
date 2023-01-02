using System.ComponentModel.DataAnnotations;

namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class ScheduledRelaunchInputModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Persistence.Entities.ScheduledRelaunch, System.Guid>
    {
        public System.Guid Id { get; set; }

        [Required, RegularExpression(@"^[0-9A-Z]{7}$")]
        public string PlayerId { get; set; }

        public System.TimeSpan Time { get; set; }

        public Persistence.Entities.ScheduledRelaunch Export()
        {
            var entity = new Persistence.Entities.ScheduledRelaunch
            {
                Id = Id,
                PlayerId = PlayerId
            };

            Mergue(entity);

            return entity;
        }

        public void Import(Persistence.Entities.ScheduledRelaunch entity)
        {
            throw new System.InvalidOperationException();
        }

        public void Mergue(Persistence.Entities.ScheduledRelaunch entity)
        {
            entity.Time = Time;
        }
    }
}
