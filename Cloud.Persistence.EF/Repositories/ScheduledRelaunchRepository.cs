namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.EF.Repositories
{
    public class ScheduledRelaunchRepository : MOROPO.SAPRESSC.Core.Persistence.EF.Repositories.AbstractEntityRepository<Entities.ScheduledRelaunch, System.Guid>
    {
        public ScheduledRelaunchRepository(Microsoft.EntityFrameworkCore.DbContext dbContext) : base(dbContext)
        {
        }

        public override System.Func<Entities.ScheduledRelaunch, object> GetPagingOrderSelector()
        {
            return entity => entity.Time;
        }
    }
}
