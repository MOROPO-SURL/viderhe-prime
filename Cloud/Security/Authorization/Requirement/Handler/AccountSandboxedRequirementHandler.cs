using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Cloud.Security.Authorization.Requirement.Handler
{
    public class AccountSandboxedRequirementHandler : Microsoft.AspNetCore.Authorization.AuthorizationHandler<AccountSandboxedRequirement>
    {
        private readonly Persistence.EF.ApplicationDbContext dbContext;

        public AccountSandboxedRequirementHandler(Persistence.EF.ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override System.Threading.Tasks.Task HandleRequirementAsync(Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext context, AccountSandboxedRequirement requirement)
        {
            var player = dbContext.Players.Include(e => e.Account).FirstOrDefault(e => e.Id == context.User.Identity.Name);

            if (player != null && player.Account.Enabled && Helpers.Integrity.AccountIntegrityHelper.IsCorrectlySigned(player.Account))
            {
                context.Succeed(requirement);
            }

            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
