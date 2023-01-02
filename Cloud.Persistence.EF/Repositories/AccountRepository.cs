using Microsoft.EntityFrameworkCore;

namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.EF.Repositories
{
    public class AccountRepository : MOROPO.SAPRESSC.Core.Persistence.EF.Repositories.AbstractEntityRepository<Entities.Account, System.Guid>
    {
        public AccountRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override System.Linq.IQueryable<Entities.Account> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet()
                .Include(entity => entity.Logo)
                .Include(entity => entity.HorizontalSplash)
                .Include(entity => entity.VerticalSplash)
                .Include(entity => entity.Screenplays)
                .Include(entity => entity.DefaultOverlay)
                .Include(entity => entity.ExtensionBridges)
                .Include(entity => entity.ImageBridges).ThenInclude(bridge => bridge.Image)
                .Include(entity => entity.ModeratorBridges).ThenInclude(bridge => bridge.Moderator).ThenInclude(e => e.Identity).ThenInclude(e => e.Image)
                .Include(entity => entity.Players).ThenInclude(entity => entity.Screenplay);
        }
    }
}
