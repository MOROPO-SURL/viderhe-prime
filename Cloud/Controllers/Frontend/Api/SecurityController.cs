using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Cloud.Controllers.Frontend.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : SAPRESSC.Core.Web.Common.Controllers.BaseWebController
    {
        private readonly Microsoft.Extensions.Options.IOptions<Options.JwtOptions> jwtOptions;
        private readonly SAPRESSC.Core.Web.Security.Providers.Facade.IIdentityProviderFacade identityProvider;
        private readonly SAPRESSC.Core.Persistence.Repositories.IEntityRepository<Persistence.Entities.Account, System.Guid> accountRepository;

        public SecurityController(SAPRESSC.Core.Persistence.Repositories.IEntityRepository<Persistence.Entities.Account, System.Guid> accountRepository, SAPRESSC.Core.Web.Security.Providers.Facade.IIdentityProviderFacade identityProvider, Microsoft.Extensions.Options.IOptions<Options.JwtOptions> jwtOptions, Microsoft.Extensions.Localization.IStringLocalizer<SecurityController> localizer, ILogger<SecurityController> logger) : base(localizer, logger)
        {
            this.jwtOptions = jwtOptions;
            this.identityProvider = identityProvider;
            this.accountRepository = accountRepository;
        }

        [HttpPost("[action]")]
        public ActionResult<Cloud.Api.Common.Models.AuthenticatedSessionModel> Authenticate([FromBody]Cloud.Api.Common.Models.AuthenticationIntentModel inputModel)
        {
            if (ModelState.IsValid)
            {
                var userIdentity = identityProvider.ProvideIdentity(inputModel.User, inputModel.Password, Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme);

                if (userIdentity != null)
                {
                    var account = accountRepository.FindOneBy(entity => entity.Enabled && entity.ModeratorBridges.Any(bridge => bridge.Moderator.UserId == userIdentity.Name));

                    if (userIdentity.HasClaim(match => match.Type.Equals(SAPRESSC.Core.Web.Security.ClaimTypes.AccountDisabled) && match.Value.Equals(true.ToString())) || account == null)
                    {
                        return Forbid(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme);
                    }

                    userIdentity.AddClaim(new System.Security.Claims.Claim(Security.Authorization.Policies.ACCOUNT_MODERATION, account.Id.ToString()));

                    var moderator = account.ModeratorBridges.First(b => b.Moderator.UserId == userIdentity.Name).Moderator;

                    return GenerateToken(userIdentity, moderator);
                }

                return Unauthorized();
            }

            return BadRequest();
        }

        [HttpPost("[action]")]
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = Security.Authorization.Policies.ACCOUNT_MODERATION)]
        public ActionResult ChangePassword([FromBody]Cloud.Api.Common.Models.UserPasswordChangeRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var identityManager = identityProvider as SAPRESSC.Core.Web.Security.Managers.Facade.IIdentityManagerFacade;

                if (identityManager.TryModifyPassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return Ok();
                }
            }

            return BadRequest();
        }

        #region Token Utility Methods

        private ActionResult GenerateToken(System.Security.Claims.ClaimsIdentity userClaims, Persistence.Entities.Moderator moderator)
        {
            var token = GenerateJwtToken(userClaims);

            var model = new Cloud.Api.Common.Models.ModeratorDisplayModel();

            model.Import(moderator);

            return Json(new Cloud.Api.Common.Models.AuthenticatedSessionModel
            {
                Token = token,
                Type = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
                ExpiresInSecond = jwtOptions.Value.ExpiresInSecond,
                Moderator = model
            });
        }

        private string GenerateJwtToken(System.Security.Claims.ClaimsIdentity userIdentity)
        {
            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtOptions.Value.Key));
            var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);
            var expires = System.DateTime.Now.AddSeconds(jwtOptions.Value.ExpiresInSecond);

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                jwtOptions.Value.Issuer,
                jwtOptions.Value.Issuer,
                userIdentity.Claims,
                expires: expires,
                signingCredentials: creds
            );

            return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}
