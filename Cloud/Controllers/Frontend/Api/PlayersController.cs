using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MOROPO.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Cloud.Controllers.Frontend.Api
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Policy = Security.Authorization.Policies.ACCOUNT_MODERATION)]
    [Route("api/[area]/[controller]")]
    [Area("account")]
    public class PlayersController : MOROPO.SAPRESSC.Core.Web.Api.Controllers.AbstractEntityApiManagementController<Persistence.Entities.Player, string, PlayerInputModel, PlayerDisplayModel>
    {
        private readonly IEntityRepository<Persistence.Entities.Account, System.Guid> accountRepository;
        private readonly IEntityRepository<Persistence.Entities.Extension, System.Guid> extensionRepository;
        private readonly Microsoft.Extensions.Options.IOptions<Options.JwtOptions> jwtOptions;

        public PlayersController(Microsoft.Extensions.Options.IOptions<Options.JwtOptions> jwtOptions, IEntityRepository<Persistence.Entities.Extension, System.Guid> extensionRepository, IEntityRepository<Persistence.Entities.Account, System.Guid> accountRepository, IEntityRepository<Persistence.Entities.Player, string> repository, Microsoft.Extensions.Localization.IStringLocalizer<PlayersController> localizer, ILogger<PlayersController> logger) : base(repository, localizer, logger)
        {
            this.accountRepository = accountRepository;
            this.extensionRepository = extensionRepository;
            this.jwtOptions = jwtOptions;
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpGet("[action]")]
        public ActionResult<AccountDisplayModel> Owner()
        {
            var protectedPlayerId = Request.Headers[Cloud.Api.Common.Providers.Constant.SecurityConstantProvider.PROTECTED_PLAYER_ID_HEADER_KEY];

            var playerId = Helpers.Security.PlayerSecurityHelper.UnprotectPlayerId(protectedPlayerId);

            try
            {
                var player = Repository.Get(playerId);

                var model = new AccountDisplayModel();

                model.Import(player.Account);

                foreach (var bridge in player.Account.ExtensionBridges)
                {
                    var extension = extensionRepository.Get(bridge.ExtensionId);

                    var extensionDisplayModel = new ExtensionDisplayModel();

                    extensionDisplayModel.Import(extension);

                    model.Extensions.Add(extensionDisplayModel);
                }

                if (player.OverlayId != null)
                {
                    model.DefaultOverlayId = player.OverlayId;
                    model.SerializedOverlayContext = player.SerializedOverlayContext;
                }

                Response.Headers.Add(Request.Headers.First(header => header.Key == Cloud.Api.Common.Providers.Constant.SecurityConstantProvider.PROTECTED_PLAYER_ID_HEADER_KEY));

                return Ok(model);
            }
            catch
            {
                return Forbid(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme);
            }
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpGet("[action]")]
        public ActionResult<PlayerPlaybackConfigurationModel> PlaybackConfiguration()
        {
            try
            {
                var accountId = Request.Headers[Cloud.Api.Common.Providers.Constant.SecurityConstantProvider.ACCOUNT_ID_HEADER_KEY];

                var protectedPlayerId = Request.Headers[Cloud.Api.Common.Providers.Constant.SecurityConstantProvider.PROTECTED_PLAYER_ID_HEADER_KEY];

                var playerId = Helpers.Security.PlayerSecurityHelper.UnprotectPlayerId(protectedPlayerId);

                var player = Repository.Get(playerId);

                if (player.Account.Id.ToString() != accountId.ToString())
                {
                    return Forbid(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme);
                }

                Response.Headers.Add(Request.Headers.First(header => header.Key == Cloud.Api.Common.Providers.Constant.SecurityConstantProvider.PROTECTED_PLAYER_ID_HEADER_KEY));

                var claims = new System.Collections.Generic.List<System.Security.Claims.Claim>
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, playerId),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimsIdentity.DefaultNameClaimType, playerId),
                    new System.Security.Claims.Claim(Security.Authorization.Claims.ACCOUNT_CLAIM, accountId),
                    new System.Security.Claims.Claim(Security.Authorization.Policies.ACCOUNT_MODERATION, accountId)
                };

                var authenticationType = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;

                return Ok(new PlayerPlaybackConfigurationModel
                {
                    Placards = player.Screenplay?.Frames.OrderBy(e => e.Priority).Where(e => e.Placard.Template.Enabled).Select((Persistence.Entities.Frame entity) =>
                    {
                        var model = new PlacardDisplayModel();

                        model.Import(entity.Placard);

                        return model;
                    }).ToArray() ?? new PlacardDisplayModel[0],
                    ScheduledRelaunches = player.ScheduledRelaunches.Select(entity => new ScheduledRelaunchDisplayModel
                    {
                        Id = entity.Id,
                        Time = entity.Time
                    }).ToArray(),
                    AuthenticationType = authenticationType,
                    AuthenticationToken = GenerateJwtToken(new System.Security.Claims.ClaimsIdentity(claims, authenticationType))
                });
            }
            catch
            {
                return NoContent();
            }
        }

        public override ActionResult Create([FromBody] PlayerInputModel inputModel)
        {
            var account = accountRepository.Get(System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value));

            if (account.MaxPlayerCount <= Repository.Count())
            {
                return Forbid(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme);
            }

            return base.Create(inputModel);
        }

        protected override System.Linq.Expressions.Expression<System.Func<Persistence.Entities.Player, bool>> GetGlobalSelector()
        {
            var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            return e => e.Account.Id == accountId;
        }

        protected override void PreCreate(Persistence.Entities.Player entity, PlayerInputModel inputModel)
        {
            base.PreCreate(entity, inputModel);

            entity.Account = accountRepository.Get(System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value));

            if (inputModel.ScreenplayId != System.Guid.Empty)
            {
                entity.Screenplay = entity.Account.Screenplays.First(e => e.Id == inputModel.ScreenplayId);
            }

            entity.Signature = Helpers.Integrity.PlayerIntegrityHelper.GetSignature(entity);
        }

        protected override void PreEdit(Persistence.Entities.Player entity, PlayerInputModel inputModel)
        {
            base.PreEdit(entity, inputModel);

            var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            if (entity.Account.Id != accountId) throw new System.InvalidOperationException();

            if (entity.Screenplay == null || entity.Screenplay.Id != inputModel.ScreenplayId)
            {
                if (inputModel.ScreenplayId == System.Guid.Empty)
                {
                    entity.Screenplay = null;
                }
                else
                {
                    var account = accountRepository.Get(accountId);

                    entity.Screenplay = account.Screenplays.First(e => e.Id == inputModel.ScreenplayId);
                }
            }

            entity.Signature = Helpers.Integrity.PlayerIntegrityHelper.GetSignature(entity);
        }

        protected override void PreDelete(Persistence.Entities.Player entity)
        {
            base.PreDelete(entity);

            var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            if (entity.Account.Id != accountId) throw new System.InvalidOperationException();
        }

        protected override PlayerDisplayModel EntityToDisplayModel(Persistence.Entities.Player entity)
        {
            var model = base.EntityToDisplayModel(entity);

            var playerSignalRContext = Providers.PlayerContextsSignalRModelProvider.Instance.Contexts.FirstOrDefault(m => m.PlayerId == entity.Id);

            if (playerSignalRContext != null)
            {
                model.IsConnected = playerSignalRContext != null;
                model.AudioEnabled = playerSignalRContext.AudioEnabled;
                model.AudioLevel = playerSignalRContext.AudioLevel;
            }

            return model;
        }

        #region Token Utility Methods

        private string GenerateJwtToken(System.Security.Claims.ClaimsIdentity userIdentity)
        {
            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtOptions.Value.Key));
            var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                jwtOptions.Value.Issuer,
                jwtOptions.Value.Issuer,
                userIdentity.Claims,
                expires: null,
                signingCredentials: creds
            );

            return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}
