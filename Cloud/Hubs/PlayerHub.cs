using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Cloud.Hubs
{
    public class PlayerHub : Hub<SignalR.Connect.Player.Clients.IPlayerClient>, SignalR.Connect.Player.Hubs.IPlayerHub
    {
        private readonly IHubContext<DashboardHub, SignalR.Connect.Dashboard.Clients.IDashboardClient> dashboardHubContext;
        private readonly MOROPO.SAPRESSC.Core.Persistence.Repositories.IEntityRepository<Persistence.Entities.Account, System.Guid> accountRepository;

        public PlayerHub(MOROPO.SAPRESSC.Core.Persistence.Repositories.IEntityRepository<Persistence.Entities.Account, System.Guid> accountRepository, IHubContext<DashboardHub, SignalR.Connect.Dashboard.Clients.IDashboardClient> dashboardHubContext)
        {
            this.dashboardHubContext = dashboardHubContext;
            this.accountRepository = accountRepository;
        }

        public async override System.Threading.Tasks.Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();

            var accountId = httpContext.Request.Headers[Api.Common.Providers.Constant.SecurityConstantProvider.ACCOUNT_ID_HEADER_KEY];

            var protectedPlayerId = Context.GetHttpContext().Request.Headers[Api.Common.Providers.Constant.SecurityConstantProvider.PROTECTED_PLAYER_ID_HEADER_KEY];

            var playerId = Helpers.Security.PlayerSecurityHelper.UnprotectPlayerId(protectedPlayerId);

            await dashboardHubContext.Clients.Group(accountId).OnPlayerConnectedAsync(playerId);

            await Groups.AddToGroupAsync(Context.ConnectionId, accountId.ToString());

            Providers.PlayerContextsSignalRModelProvider.Instance.Contexts.Add(new Models.SignalR.PlayerContextSignalRModel
            {
                ConnectionId = Context.ConnectionId,
                AccountId = System.Guid.Parse(accountId),
                PlayerId = playerId
            });

            await base.OnConnectedAsync();
        }

        public async override System.Threading.Tasks.Task OnDisconnectedAsync(System.Exception exception)
        {
            var playerContext = Providers.PlayerContextsSignalRModelProvider.Instance.Contexts.First(m => m.ConnectionId == Context.ConnectionId);

            Providers.PlayerContextsSignalRModelProvider.Instance.Contexts.Remove(playerContext);

            await dashboardHubContext.Clients.Group(playerContext.AccountId.ToString()).OnPlayerDisconnectedAsync(playerContext.PlayerId);

            await base.OnDisconnectedAsync(exception);
        }

        public void NotifyPlayingPlacard(System.Guid placardId)
        {
            var playerContext = Providers.PlayerContextsSignalRModelProvider.Instance.Contexts.First(m => m.ConnectionId == Context.ConnectionId);

            dashboardHubContext.Clients.Group(playerContext.AccountId.ToString()).OnPlayingPlacardAsync(playerContext.PlayerId, placardId);
        }

        public void UpdateEnvironmentConfiguration(int audioLevel, bool audioEnabled)
        {
            var playerContext = Providers.PlayerContextsSignalRModelProvider.Instance.Contexts.First(m => m.ConnectionId == Context.ConnectionId);

            playerContext.AudioEnabled = audioEnabled;
            playerContext.AudioLevel = audioLevel;

            dashboardHubContext.Clients.Group(playerContext.AccountId.ToString()).OnPlayerUpdateEnvironmentConfigurationAsync(playerContext.PlayerId, playerContext.AudioLevel, playerContext.AudioEnabled);
        }
    }
}
