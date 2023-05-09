using Microsoft.AspNetCore.SignalR.Client;
using SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models;
using SmartSolucionesCuba.Viderhe.Cloud.SignalR.Connect.Common.Enumerations;
using SmartSolucionesCuba.Viderhe.Support.Presentation.Extensions;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Terminal.Player.Hubs.Client
{
    public class PlayerHubClient : Cloud.SignalR.Connect.Player.Clients.IPlayerClient, System.IDisposable
    {
        private static PlayerHubClient instance;

        public static PlayerHubClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayerHubClient();
                }

                return instance;
            }
        }

        private HubConnection connection;

        public PlayerHubClient()
        {
            connection = new HubConnectionBuilder()
                .WithUrl(string.Format("{0}://{1}/hubs/player", Support.Presentation.Managers.SessionMemoryManager.Instance.UseSSL ? "https" : "http", Support.Presentation.Managers.SessionMemoryManager.Instance.Host), options =>
                {
                    options.Headers = new System.Collections.Generic.Dictionary<string, string>();

                    var securityHeaders = Managers.SessionManager.Instance.ExtraHeaders;

                    foreach (var header in securityHeaders)
                    {
                        options.Headers.Add(header);
                    }
                })
                .WithAutomaticReconnect()
                .Build();

            connection.ServerTimeout = System.TimeSpan.FromSeconds(10);
            connection.HandshakeTimeout = System.TimeSpan.FromSeconds(10);

            connection.On(nameof(OnPresentNextFrameRequestAsync), OnPresentNextFrameRequestAsync);
            connection.On(nameof(OnPresentPreviousFrameRequestAsync), OnPresentPreviousFrameRequestAsync);
            connection.On<PlacardDisplayModel[]>(nameof(OnPresentInterventionPlacardsRequestAsync), OnPresentInterventionPlacardsRequestAsync);
            connection.On<ScreenplayHandlingImmediacyMode>(nameof(OnRestartScreenplayRequestAsync), OnRestartScreenplayRequestAsync);
            connection.On<ScreenplayHandlingImmediacyMode>(nameof(OnReloadScreenplayRequestAsync), OnReloadScreenplayRequestAsync);
            connection.On<bool>(nameof(OnSetAudioEnabledRequestAsync), OnSetAudioEnabledRequestAsync);
            connection.On<int>(nameof(OnSetAudioLevelRequestAsync), OnSetAudioLevelRequestAsync);
            connection.On<bool>(nameof(OnCloseRequestAsync), OnCloseRequestAsync);
            connection.On<bool>(nameof(OnShutdownRequestAsync), OnShutdownRequestAsync);
        }

        public void GetReady()
        {
            if (connection.State == HubConnectionState.Disconnected)
            {
                connection.Closed += Connection_Closed;

                connection.StartAsync().ContinueWith(ConnectionAsyncStart_Callback);
            }
        }

        private void ConnectionAsyncStart_Callback(System.Threading.Tasks.Task task)
        {   
            if (task.IsCanceled || task.Exception != null)
            {
                System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(15));

                connection.StartAsync().ContinueWith(ConnectionAsyncStart_Callback);

                return;
            }

            Xamarin.Forms.Device.BeginInvokeOnMainThread(UpdateEnvironmentConfiguration);
        }

        private async System.Threading.Tasks.Task Connection_Closed(System.Exception arg)
        {
            await System.Threading.Tasks.Task.Delay(System.TimeSpan.FromSeconds(15));

            await connection.StartAsync().ContinueWith(ConnectionAsyncStart_Callback);
        }

        public void MakeReleased()
        {
            connection.Closed -= Connection_Closed;

            connection.StopAsync();
        }

        public void UpdateEnvironmentConfiguration()
        {
            var platformControlHelper = Xamarin.Forms.DependencyService.Get<Helpers.Facade.IPlatformControlHelperFacade>();

            connection.InvokeAsync(nameof(Cloud.SignalR.Connect.Player.Hubs.IPlayerHub.UpdateEnvironmentConfiguration), platformControlHelper.AudioLevel, platformControlHelper.IsAudioEnabled);
        }

        public void UpdateEnvironmentConfiguration(int audioLevel, bool isAudioEnabled)
        {
            connection.InvokeAsync(nameof(Cloud.SignalR.Connect.Player.Hubs.IPlayerHub.UpdateEnvironmentConfiguration), audioLevel, isAudioEnabled);
        }

        public void NotifyPlayingPlacard(System.Guid placardId)
        {
            connection.InvokeAsync(nameof(Cloud.SignalR.Connect.Player.Hubs.IPlayerHub.NotifyPlayingPlacard), placardId);
        }

        public System.Threading.Tasks.Task OnPresentNextFrameRequestAsync()
        {
            if (Xamarin.Forms.Application.Current.MainPage.Navigation.ModalStack.Count > 0)
            {
                var screenplayPlayerPage = Xamarin.Forms.Application.Current.MainPage.Navigation.ModalStack.First() as Support.Presentation.Player.Pages.ScreenplayPlayerPage;

                screenplayPlayerPage.RequestPresentNextFrame();
            }

            return System.Threading.Tasks.Task.CompletedTask;
        }

        public System.Threading.Tasks.Task OnPresentPreviousFrameRequestAsync()
        {
            if (Xamarin.Forms.Application.Current.MainPage.Navigation.ModalStack.Count > 0)
            {
                var screenplayPlayerPage = Xamarin.Forms.Application.Current.MainPage.Navigation.ModalStack.First() as Support.Presentation.Player.Pages.ScreenplayPlayerPage;

                screenplayPlayerPage.RequestPresentPreviousFrame();
            }

            return System.Threading.Tasks.Task.CompletedTask;
        }

        public System.Threading.Tasks.Task OnPresentInterventionPlacardsRequestAsync(PlacardDisplayModel[] placards)
        {
            if (Xamarin.Forms.Application.Current.MainPage.Navigation.ModalStack.Count > 0)
            {
                var screenplayPlayerPage = Xamarin.Forms.Application.Current.MainPage.Navigation.ModalStack.First() as Support.Presentation.Player.Pages.ScreenplayPlayerPage;

                screenplayPlayerPage.PresentInterventionPlacards(placards.Select(m => m.ToViewModel()).ToArray());
            }

            return System.Threading.Tasks.Task.CompletedTask;
        }

        public System.Threading.Tasks.Task OnRestartScreenplayRequestAsync(ScreenplayHandlingImmediacyMode immediacyMode)
        {
            if (Xamarin.Forms.Application.Current.MainPage.Navigation.ModalStack.Count > 0)
            {
                var screenplayPlayerPage = Xamarin.Forms.Application.Current.MainPage.Navigation.ModalStack.First() as Support.Presentation.Player.Pages.ScreenplayPlayerPage;

                screenplayPlayerPage.RequestRestart(immediacyMode);
            }

            return System.Threading.Tasks.Task.CompletedTask;
        }

        public System.Threading.Tasks.Task OnReloadScreenplayRequestAsync(ScreenplayHandlingImmediacyMode immediacyMode)
        {
            if (Xamarin.Forms.Application.Current.MainPage.Navigation.ModalStack.Count > 0)
            {
                var screenplayPlayerPage = Xamarin.Forms.Application.Current.MainPage.Navigation.ModalStack.First() as Support.Presentation.Player.Pages.ScreenplayPlayerPage;

                screenplayPlayerPage.RequestReload(immediacyMode);

                MakeReleased();
            }

            return System.Threading.Tasks.Task.CompletedTask;
        }

        public System.Threading.Tasks.Task OnSetAudioLevelRequestAsync(int percent)
        {
            Xamarin.Forms.DependencyService.Get<Helpers.Facade.IPlatformControlHelperFacade>().SetAudioLevel(percent);

            return System.Threading.Tasks.Task.CompletedTask;
        }

        public System.Threading.Tasks.Task OnSetAudioEnabledRequestAsync(bool enabled)
        {
            Xamarin.Forms.DependencyService.Get<Helpers.Facade.IPlatformControlHelperFacade>().SetAudioEnabled(enabled);

            return System.Threading.Tasks.Task.CompletedTask;
        }

        public System.Threading.Tasks.Task OnCloseRequestAsync(bool restart)
        {
            Xamarin.Forms.DependencyService.Get<Helpers.Facade.IPlatformControlHelperFacade>().Close(restart);

            return System.Threading.Tasks.Task.CompletedTask;
        }

        public System.Threading.Tasks.Task OnShutdownRequestAsync(bool restart)
        {
            return Xamarin.Forms.DependencyService.Get<Helpers.Facade.IPlatformControlHelperFacade>().ShutdownAsync(restart);
        }

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar llamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: elimine el estado administrado (objetos administrados).
                }

                // TODO: libere los recursos no administrados (objetos no administrados) y reemplace el siguiente finalizador.
                // TODO: configure los campos grandes en nulos.

                connection.Closed -= Connection_Closed;

                connection.Remove(nameof(OnPresentNextFrameRequestAsync));
                connection.Remove(nameof(OnPresentPreviousFrameRequestAsync));
                connection.Remove(nameof(OnPresentInterventionPlacardsRequestAsync));
                connection.Remove(nameof(OnRestartScreenplayRequestAsync));
                connection.Remove(nameof(OnReloadScreenplayRequestAsync));
                connection.Remove(nameof(OnSetAudioEnabledRequestAsync));
                connection.Remove(nameof(OnSetAudioLevelRequestAsync));
                connection.Remove(nameof(OnCloseRequestAsync));
                connection.Remove(nameof(OnShutdownRequestAsync));

                connection.StopAsync().ContinueWith(task =>
                {
                    connection.DisposeAsync();
                });

                disposedValue = true;
            }
        }

        // TODO: reemplace un finalizador solo si el anterior Dispose(bool disposing) tiene código para liberar los recursos no administrados.
        // ~PlayerHubClient() {
        //   // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
        //   Dispose(false);
        // }

        // Este código se agrega para implementar correctamente el patrón descartable.
        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
            Dispose(true);
            // TODO: quite la marca de comentario de la siguiente línea si el finalizador se ha reemplazado antes.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
