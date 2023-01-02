using Microsoft.EntityFrameworkCore;

namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.EF.Repositories
{
    public class FrameRepository : MOROPO.SAPRESSC.Core.Persistence.EF.Repositories.AbstractEntityRepository<Entities.Frame, long>
    {
        public FrameRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public override System.Func<Entities.Frame, object> GetPagingOrderSelector()
        {
            return e => e.Priority;
        }

        protected override System.Linq.IQueryable<Entities.Frame> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet()
                .Include(entiy => entiy.Screenplay).ThenInclude(e => e.Frames)
                .Include(entity => entity.Placard).ThenInclude(entity => entity.Template)
                .Include(entity => entity.Placard).ThenInclude(entity => entity.Account);
        }
    }
}
