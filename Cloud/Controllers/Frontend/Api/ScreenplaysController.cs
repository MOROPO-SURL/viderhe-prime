using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Cloud.Controllers.Frontend.Api
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Policy = Security.Authorization.Policies.ACCOUNT_MODERATION)]
    [Route("api/[area]/[controller]")]
    [Area("account")]
    [SAPRESSC.Core.Web.Security.Filters.ChangeUserPasswordRequiredAction]
    [SAPRESSC.Core.Web.Security.Filters.CatchChangeUserPasswordRequiredException]
    public class ScreenplaysController : SAPRESSC.Core.Web.Api.Controllers.AbstractEntityApiManagementController<Persistence.Entities.Screenplay, System.Guid, ScreenplayInputModel, ScreenplayDisplayModel>
    {
        private readonly IEntityRepository<Persistence.Entities.Account, System.Guid> accountRepository;
        private readonly IEntityRepository<Persistence.Entities.Player, string> playerRepository;

        public ScreenplaysController(IEntityRepository<Persistence.Entities.Player, string> playerRepository, IEntityRepository<Persistence.Entities.Account, System.Guid> accountRepository, IEntityRepository<Persistence.Entities.Screenplay, System.Guid> repository, Microsoft.Extensions.Localization.IStringLocalizer<ScreenplaysController> localizer, ILogger<ScreenplaysController> logger) : base(repository, localizer, logger)
        {
            this.accountRepository = accountRepository;
            this.playerRepository = playerRepository;
        }

        protected override System.Linq.Expressions.Expression<System.Func<Persistence.Entities.Screenplay, bool>> GetGlobalSelector()
        {
            var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            return e => e.Account.Id == accountId;
        }

        public override ActionResult<ScreenplayDisplayModel[]> Get()
        {
            return Ok(Repository.GetAll().Where(GetGlobalSelector()).Select(entity => EntityToDisplayModel(entity)).ToArray());
        }

        protected override void PreCreate(Persistence.Entities.Screenplay entity, ScreenplayInputModel inputModel)
        {
            base.PreCreate(entity, inputModel);

            entity.Account = accountRepository.Get(System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value));
        }

        protected override void PreEdit(Persistence.Entities.Screenplay entity, ScreenplayInputModel inputModel)
        {
            base.PreEdit(entity, inputModel);

            var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            if (entity.Account.Id != accountId) throw new System.InvalidOperationException();
        }

        protected override void PreDelete(Persistence.Entities.Screenplay entity)
        {
            base.PreDelete(entity);

            var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            if (entity.Account.Id != accountId) throw new System.InvalidOperationException();
        }

        protected override void PostDelete(Persistence.Entities.Screenplay entity)
        {
            base.PostDelete(entity);

            var relatedPlayers = playerRepository.FindBy(item => item.ScreenplayId == entity.Id).ToArray();

            if (relatedPlayers.Length <= 0) return;

            var relatedPlayerQueue = new System.Collections.Generic.Queue<Persistence.Entities.Player>(relatedPlayers);

            while (relatedPlayerQueue.Count > 1)
            {
                var player = relatedPlayerQueue.Dequeue();

                player.Screenplay = null;
                player.ScreenplayId = null;

                playerRepository.Update(player, User.Identity.Name, false);
            }

            var lastPlayer = relatedPlayerQueue.Dequeue();

            lastPlayer.Screenplay = null;
            lastPlayer.ScreenplayId = null;

            playerRepository.Update(lastPlayer, User.Identity.Name, true);
        }
    }
}
