namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class ScheduledRelaunchDisplayModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.ScheduledRelaunch, System.Guid>
    {
        public System.Guid Id { get; set; }

        public System.TimeSpan Time { get; set; }

        public void Import(Persistence.Entities.ScheduledRelaunch entity)
        {
            Id = entity.Id;
            Time = entity.Time;
        }
    }
}
