using Microsoft.EntityFrameworkCore;

namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.EF.Repositories
{
    public class PlacardRepository : MOROPO.SAPRESSC.Core.Persistence.EF.Repositories.AbstractEntityRepository<Entities.Placard, System.Guid>
    {
        public PlacardRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override System.Linq.IQueryable<Entities.Placard> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet()
                .Include(e => e.Account)
                .Include(e => e.Template)
                .Include(e => e.ImageBridges)
                .Include(e => e.VideoBridges);
        }

        public override System.Func<Entities.Placard, object> GetPagingOrderSelector()
        {
            return e => e.CreateAt;
        }

        public override bool IsPagingOrderDescending()
        {
            return true;
        }
    }
}
