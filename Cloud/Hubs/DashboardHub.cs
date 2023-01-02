using Microsoft.AspNetCore.SignalR;
using MOROPO.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.Viderhe.Cloud.SignalR.Connect.Common.Enumerations;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Cloud.Hubs
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Policy = Security.Authorization.Policies.ACCOUNT_MODERATION)]
    public class DashboardHub : Hub<SignalR.Connect.Dashboard.Clients.IDashboardClient>, SignalR.Connect.Dashboard.Hubs.IDashboardHub
    {
        private readonly IHubContext<PlayerHub, SignalR.Connect.Player.Clients.IPlayerClient> playerHubContext;
        private readonly IEntityRepository<Persistence.Entities.Screenplay, System.Guid> screenplayRepository;
        private readonly IEntityRepository<Persistence.Entities.Placard, System.Guid> placardRepository;

        public DashboardHub(IEntityRepository<Persistence.Entities.Placard, System.Guid> placardRepository, IEntityRepository<Persistence.Entities.Screenplay, System.Guid> screenplayRepository, IHubContext<PlayerHub, SignalR.Connect.Player.Clients.IPlayerClient> playerHubContext)
        {
            this.playerHubContext = playerHubContext;
            this.screenplayRepository = screenplayRepository;
            this.placardRepository = placardRepository;
        }

        public async override System.Threading.Tasks.Task OnConnectedAsync()
        {
            var accountId = System.Guid.Parse(Context.User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            await Groups.AddToGroupAsync(Context.ConnectionId, accountId.ToString());

            await base.OnConnectedAsync();
        }

        public void PlayerPresentNextFrame(string playerId)
        {
            var accountId = System.Guid.Parse(Context.User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            try
            {
                var playerContext = Providers.PlayerContextsSignalRModelProvider.Instance.Contexts.First(m => m.AccountId == accountId && m.PlayerId == playerId);

                playerHubContext.Clients.Client(playerContext.ConnectionId).OnPresentNextFrameRequestAsync();
            }
            catch { }
        }

        public void PlayerPresentPreviousFrame(string playerId)
        {
            var accountId = System.Guid.Parse(Context.User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            try
            {
                var playerContext = Providers.PlayerContextsSignalRModelProvider.Instance.Contexts.First(m => m.AccountId == accountId && m.PlayerId == playerId);

                playerHubContext.Clients.Client(playerContext.ConnectionId).OnPresentPreviousFrameRequestAsync();
            }
            catch { }
        }

        public void PlayerPresentInterventionScreenplay(string playerId, System.Guid screenplayId)
        {
            var accountId = System.Guid.Parse(Context.User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            try
            {
                var playerContext = Providers.PlayerContextsSignalRModelProvider.Instance.Contexts.First(m => m.AccountId == accountId && m.PlayerId == playerId);

                var screenplay = screenplayRepository.Get(screenplayId);

                playerHubContext.Clients.Client(playerContext.ConnectionId).OnPresentInterventionPlacardsRequestAsync(screenplay.Frames.OrderBy(e => e.Priority).Where(e => e.Placard.Template.Enabled).Select((Persistence.Entities.Frame entity) =>
                {
                    var model = new Api.Common.Models.PlacardDisplayModel();

                    model.Import(entity.Placard);

                    return model;
                }).ToArray());
            }
            catch { }
        }

        public void PlayerPresentInterventionPlacard(string playerId, System.Guid placardId)
        {
            var accountId = System.Guid.Parse(Context.User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            try
            {
                var playerContext = Providers.PlayerContextsSignalRModelProvider.Instance.Contexts.First(m => m.AccountId == accountId && m.PlayerId == playerId);

                var entity = placardRepository.Get(placardId);

                var model = new Api.Common.Models.PlacardDisplayModel();

                model.Import(entity);

                playerHubContext.Clients.Client(playerContext.ConnectionId).OnPresentInterventionPlacardsRequestAsync(new Api.Common.Models.PlacardDisplayModel[] { model });
            }
            catch { }
        }

        public void PlayerRestarScreenplay(string playerId, ScreenplayHandlingImmediacyMode immediancyMode)
        {
            var accountId = System.Guid.Parse(Context.User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            try
            {
                var playerContext = Providers.PlayerContextsSignalRModelProvider.Instance.Contexts.First(m => m.AccountId == accountId && m.PlayerId == playerId);

                playerHubContext.Clients.Client(playerContext.ConnectionId).OnRestartScreenplayRequestAsync(immediancyMode);
            }
            catch { }
        }

        public void PlayerReloadScreenplay(string playerId, ScreenplayHandlingImmediacyMode immediancyMode)
        {
            var accountId = System.Guid.Parse(Context.User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            try
            {
                var playerContext = Providers.PlayerContextsSignalRModelProvider.Instance.Contexts.First(m => m.AccountId == accountId && m.PlayerId == playerId);

                playerHubContext.Clients.Client(playerContext.ConnectionId).OnReloadScreenplayRequestAsync(immediancyMode);
            }
            catch { }
        }

        public void PlayerSetAudioEnabled(string playerId, bool enabled)
        {
            var accountId = System.Guid.Parse(Context.User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            try
            {
                var playerContext = Providers.PlayerContextsSignalRModelProvider.Instance.Contexts.First(m => m.AccountId == accountId && m.PlayerId == playerId);

                playerHubContext.Clients.Client(playerContext.ConnectionId).OnSetAudioEnabledRequestAsync(enabled);
            }
            catch { }
        }

        public void PlayerSetAudioLevel(string playerId, int percent)
        {
            var accountId = System.Guid.Parse(Context.User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            try
            {
                var playerContext = Providers.PlayerContextsSignalRModelProvider.Instance.Contexts.First(m => m.AccountId == accountId && m.PlayerId == playerId);

                playerHubContext.Clients.Client(playerContext.ConnectionId).OnSetAudioLevelRequestAsync(percent);
            }
            catch { }
        }

        public void PlayerClose(string playerId, bool restart = false)
        {
            var accountId = System.Guid.Parse(Context.User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            try
            {
                var playerContext = Providers.PlayerContextsSignalRModelProvider.Instance.Contexts.First(m => m.AccountId == accountId && m.PlayerId == playerId);

                playerHubContext.Clients.Client(playerContext.ConnectionId).OnCloseRequestAsync(restart);
            }
            catch { }
        }

        public void PlayerShutdown(string playerId, bool restart = false)
        {
            var accountId = System.Guid.Parse(Context.User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            try
            {
                var playerContext = Providers.PlayerContextsSignalRModelProvider.Instance.Contexts.First(m => m.AccountId == accountId && m.PlayerId == playerId);

                playerHubContext.Clients.Client(playerContext.ConnectionId).OnShutdownRequestAsync(restart);
            }
            catch { }
        }
    }
}
