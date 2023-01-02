using Microsoft.EntityFrameworkCore;

namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.EF.Repositories
{
    public class ModeratorRepository : MOROPO.SAPRESSC.Core.Persistence.EF.Repositories.AbstractEntityRepository<Entities.Moderator, System.Guid>
    {
        public ModeratorRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override System.Linq.IQueryable<Entities.Moderator> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet()
                .Include(e => e.User)
                .Include(e => e.Identity).ThenInclude(e => e.Image)
                .Include(e => e.AccountBridges);
        }
    }
}
