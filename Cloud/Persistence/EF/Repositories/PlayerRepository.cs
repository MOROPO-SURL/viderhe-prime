using Microsoft.EntityFrameworkCore;

namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.EF.Repositories
{
    public class PlayerRepository : SAPRESSC.Core.Persistence.EF.Repositories.AbstractEntityRepository<Entities.Player, string>
    {
        public PlayerRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override System.Linq.IQueryable<Entities.Player> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet()
                .Include(entity => entity.Account).ThenInclude(entity => entity.Logo)
                .Include(entity => entity.Account).ThenInclude(entity => entity.HorizontalSplash)
                .Include(entity => entity.Account).ThenInclude(entity => entity.ExtensionBridges)
                .Include(entity => entity.Screenplay).ThenInclude(entity => entity.Frames).ThenInclude(entity => entity.Placard).ThenInclude(entity => entity.Template);
        }

        public override System.Func<Entities.Player, object> GetPagingOrderSelector()
        {
            return entity => entity.Name;
        }

        public override bool IsPagingOrderDescending()
        {
            return true;
        }
    }
}
