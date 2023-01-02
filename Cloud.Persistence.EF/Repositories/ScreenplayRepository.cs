using Microsoft.EntityFrameworkCore;

namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.EF.Repositories
{
    public class ScreenplayRepository : MOROPO.SAPRESSC.Core.Persistence.EF.Repositories.AbstractEntityRepository<Entities.Screenplay, System.Guid>
    {
        public ScreenplayRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override System.Linq.IQueryable<Entities.Screenplay> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet()
                .Include(entity => entity.Account)
                .Include(entity => entity.Frames).ThenInclude(entity => entity.Placard).ThenInclude(entity => entity.Template);
        }
    }
}
