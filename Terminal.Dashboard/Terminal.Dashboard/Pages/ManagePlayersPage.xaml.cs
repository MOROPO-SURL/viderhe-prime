using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Pages
{
    public partial class ManagePlayersPage : XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.ManagePlayersPageModel>
    {
        private const string SCREENPLAY_HANDLING_MODE_CURRENT_FRAME_PRESENTATION_COMPLETE = "Completado el Marco Actual";
        private const string SCREENPLAY_HANDLING_MODE_FRAMES_PRESENTATION_COMPLETE = "Completado el Ciclo Actual";
        private const string SCREENPLAY_HANDLING_MODE_IMMEDIATELY = "Inmediatamente";

        private const string PLAYER_EDIT_DEFAULT = "Información y Configuración";
        private const string PLAYER_EDIT_SCHEDULED_RELAUNCH = "Relanzamientos Programados";

        private const string PLAYER_HANDLING_RELAUNCH = "Relanzar";
        private const string PLAYER_HANDLING_CLOSE = "Cerrar";
        private const string PLAYER_HANDLING_SHUTDOWN = "Apagar";
        private const string PLAYER_HANDLING_RESTART = "Reiniciar";

        public ManagePlayersPage() : base(new Models.Page.ManagePlayersPageModel { IsBusy = true })
        {
            InitializeComponent();

            IsEnabled = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!IsEnabled)
            {
                IsEnabled = true;

                LoadPlayers();
            }
        }

        private void AddToolbarItem_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel.IsBusy) return;

            this.NavigationPushPageAsync(new ConfigurePlayerPage(ReloadPlayer));
        }

        private void ReloadToolbarItem_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel.IsBusy) return;

            ReloadPlayer();
        }

        private void PlayerNextButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel.IsBusy) return;

            var model = (sender as Xamarin.Forms.Button).BindingContext as Models.View.PlayerViewModel;

            Hubs.Client.DashboardHubClient.Instance.RequestPlayerPresentNextFrameAsync(model.Id).ContinueWith(hubTask =>
            {
                System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(1));

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = false;

                    Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Reproducir Siguiente Marco", "Solicitud de reproducción de siguiente marco enviada con éxito.");
                });
            });
        }

        private void PlayerPreviousButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel.IsBusy) return;

            var model = (sender as Xamarin.Forms.Button).BindingContext as Models.View.PlayerViewModel;

            Hubs.Client.DashboardHubClient.Instance.RequestPlayerPresentPreviousFrameAsync(model.Id).ContinueWith(hubTask =>
            {
                System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(1));

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = false;

                    Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Reproducir Marco Anterior", "Solicitud de reproducción de marco anterior enviada con éxito.");
                });
            });
        }

        private void PlayerInterventionButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel.IsBusy) return;

            this.NavigationPushPageAsync(new Support.Presentation.Dashboard.Pages.PlacardPickPage(PlayerIterventionPlacardPicked_Callback, (sender as Xamarin.Forms.Button).BindingContext as Models.View.PlayerViewModel));
        }

        private void PlayerAudioEnabledSwitchButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel.IsBusy) return;

            var model = (sender as Xamarin.Forms.Button).BindingContext as Models.View.PlayerViewModel;

            Hubs.Client.DashboardHubClient.Instance.RequestPlayerSetAudioEnabledAsync(model.Id, !model.AudioEnabled).ContinueWith(hubTask =>
            {
                System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(1));

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = false;
                });
            });
        }

        private void PlayerAudioVolumeUpButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel.IsBusy) return;

            var model = (sender as Xamarin.Forms.Button).BindingContext as Models.View.PlayerViewModel;

            Hubs.Client.DashboardHubClient.Instance.RequestPlayerAudioVolumeChangeAsync(model.Id, model.AudioLevel + 10).ContinueWith(hubTask =>
            {
                System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(1));

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = false;
                });
            });
        }

        private void PlayerAudioVolumeDownButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel.IsBusy) return;

            var model = (sender as Xamarin.Forms.Button).BindingContext as Models.View.PlayerViewModel;

            Hubs.Client.DashboardHubClient.Instance.RequestPlayerAudioVolumeChangeAsync(model.Id, model.AudioLevel - 10).ContinueWith(hubTask =>
            {
                System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(1));

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = false;
                });
            });
        }

        private void PlayerTerminateButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel.IsBusy) return;

            var model = (sender as Xamarin.Forms.Button).BindingContext as Models.View.PlayerViewModel;

            DisplayActionSheet("Dispositivo de Reproducción", "Cancelar", null, PLAYER_HANDLING_RESTART, PLAYER_HANDLING_SHUTDOWN).ContinueWith(shutdownModelPickDialogTask =>
            {
                if (shutdownModelPickDialogTask.Result == PLAYER_HANDLING_RESTART)
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayAlert("Confirmar", "Está seguro que desea reiniciar este dispositivo?", "Reiniciar", "Cancelar").ContinueWith(confirmationDialogTask =>
                        {
                            if (!confirmationDialogTask.Result) return;

                            Hubs.Client.DashboardHubClient.Instance.RequestPlayerTerminateAsync(model.Id, true).ContinueWith(hubTask =>
                            {
                                System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(1));

                                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                {
                                    (BindingContext as Models.Page.ManagePlayersPageModel).IsBusy = false;

                                    Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Reiniciar Dispositivo", "Solicitud de reinicio de dispositivo enviada con éxito.");
                                });
                            });
                        });
                    });

                    return;
                }

                if (shutdownModelPickDialogTask.Result == PLAYER_HANDLING_SHUTDOWN)
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayAlert("Confirmar", "Está seguro que desea apagar este dispositivo?", "Apagar", "Cancelar").ContinueWith(confirmationDialogTask =>
                        {
                            if (!confirmationDialogTask.Result) return;

                            Hubs.Client.DashboardHubClient.Instance.RequestPlayerTerminateAsync(model.Id, false).ContinueWith(hubTask =>
                            {
                                System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(1));

                                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                {
                                    (BindingContext as Models.Page.ManagePlayersPageModel).IsBusy = false;

                                    Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Apagar Dispositivo", "Solicitud de apagar dispositivo enviada con éxito.");
                                });
                            });
                        });
                    });

                    return;
                }

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = false;
                });
            });
        }

        private void PlayerCloseButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel.IsBusy) return;

            var model = (sender as Xamarin.Forms.Button).BindingContext as Models.View.PlayerViewModel;

            DisplayActionSheet("Aplicación de Reproducción", "Cancelar", null, PLAYER_HANDLING_RELAUNCH, PLAYER_HANDLING_CLOSE).ContinueWith(shutdownModelPickDialogTask =>
            {
                if (shutdownModelPickDialogTask.Result == PLAYER_HANDLING_RELAUNCH)
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayAlert("Confirmar", "Está seguro que desea reiniciar la aplicación?", "Reiniciar", "Cancelar").ContinueWith(confirmationDialogTask =>
                        {
                            if (!confirmationDialogTask.Result) return;

                            Hubs.Client.DashboardHubClient.Instance.RequestPlayerCloseAsync(model.Id, true).ContinueWith(hubTask =>
                            {
                                System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(1));

                                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                {
                                    (BindingContext as Models.Page.ManagePlayersPageModel).IsBusy = false;

                                    Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Reiniciar Aplicación", "Solicitud de reinicio de aplicación enviada con éxito.");
                                });
                            });
                        });
                    });

                    return;
                }

                if (shutdownModelPickDialogTask.Result == PLAYER_HANDLING_CLOSE)
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayAlert("Confirmar", "Está seguro que desea cerrar aplicación?", "Apagar", "Cancelar").ContinueWith(confirmationDialogTask =>
                        {
                            if (!confirmationDialogTask.Result) return;

                            Hubs.Client.DashboardHubClient.Instance.RequestPlayerCloseAsync(model.Id, false).ContinueWith(hubTask =>
                            {
                                System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(1));

                                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                {
                                    (BindingContext as Models.Page.ManagePlayersPageModel).IsBusy = false;

                                    Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Cerrar Aplicación", "Solicitud de cerrar aplicación enviada con éxito.");
                                });
                            });
                        });
                    });

                    return;
                }

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = false;
                });
            });
        }

        private void PlayerBackwardButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel.IsBusy) return;

            var model = (sender as Xamarin.Forms.Button).BindingContext as Models.View.PlayerViewModel;

            DisplayActionSheet("Reiniciar Guion", "Cancelar", null, SCREENPLAY_HANDLING_MODE_CURRENT_FRAME_PRESENTATION_COMPLETE, SCREENPLAY_HANDLING_MODE_IMMEDIATELY).ContinueWith(task =>
            {
                if (task.Result == SCREENPLAY_HANDLING_MODE_CURRENT_FRAME_PRESENTATION_COMPLETE)
                {
                    Hubs.Client.DashboardHubClient.Instance.RequestPlayerRestartScreenplayAsync(model.Id, Cloud.SignalR.Connect.Common.Enumerations.ScreenplayHandlingImmediacyMode.CurrentFramePresentationComplete).ContinueWith(hubTask =>
                    {
                        System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(1));

                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            (BindingContext as Models.Page.ManagePlayersPageModel).IsBusy = false;

                            Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Reiniciar Guion", "Solicitud de reinicio de Guion cuando termine el marco actual enviada con éxito.");
                        });
                    });

                    return;
                }

                if (task.Result == SCREENPLAY_HANDLING_MODE_IMMEDIATELY)
                {
                    Hubs.Client.DashboardHubClient.Instance.RequestPlayerRestartScreenplayAsync(model.Id, Cloud.SignalR.Connect.Common.Enumerations.ScreenplayHandlingImmediacyMode.Immediately).ContinueWith(hubTask =>
                    {
                        System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(1));

                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            (BindingContext as Models.Page.ManagePlayersPageModel).IsBusy = false;

                            Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Reiniciar Guion", "Solicitud de reinicio de Guion inmediatamente enviada con éxito.");
                        });
                    });

                    return;
                }

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = false;
                });
            });
        }

        private void PlayerReloadButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel.IsBusy) return;

            var model = (sender as Xamarin.Forms.Button).BindingContext as Models.View.PlayerViewModel;

            DisplayActionSheet("Recargar Guion", "Cancelar", null, SCREENPLAY_HANDLING_MODE_FRAMES_PRESENTATION_COMPLETE, SCREENPLAY_HANDLING_MODE_CURRENT_FRAME_PRESENTATION_COMPLETE, SCREENPLAY_HANDLING_MODE_IMMEDIATELY).ContinueWith(task =>
            {
                if (task.Result == SCREENPLAY_HANDLING_MODE_FRAMES_PRESENTATION_COMPLETE)
                {
                    Hubs.Client.DashboardHubClient.Instance.RequestPlayerReloadScreenplayAsync(model.Id, Cloud.SignalR.Connect.Common.Enumerations.ScreenplayHandlingImmediacyMode.FramesPresentationComplete).ContinueWith(hubTask =>
                    {
                        System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(1));

                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            BindingContextPageModel.IsBusy = false;

                            Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Recargar Guion", "Solicitud de recarga de Guion cuando termine el ciclo actual enviada con éxito.");
                        });
                    });

                    return;
                }

                if (task.Result == SCREENPLAY_HANDLING_MODE_CURRENT_FRAME_PRESENTATION_COMPLETE)
                {
                    Hubs.Client.DashboardHubClient.Instance.RequestPlayerReloadScreenplayAsync(model.Id, Cloud.SignalR.Connect.Common.Enumerations.ScreenplayHandlingImmediacyMode.CurrentFramePresentationComplete).ContinueWith(hubTask =>
                    {
                        System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(1));

                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            BindingContextPageModel.IsBusy = false;

                            Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Recargar Guion", "Solicitud de recarga de Guion cuando termine el marco actual enviada con éxito.");
                        });
                    });

                    return;
                }

                if (task.Result == SCREENPLAY_HANDLING_MODE_FRAMES_PRESENTATION_COMPLETE)
                {
                    Hubs.Client.DashboardHubClient.Instance.RequestPlayerReloadScreenplayAsync(model.Id, Cloud.SignalR.Connect.Common.Enumerations.ScreenplayHandlingImmediacyMode.FramesPresentationComplete).ContinueWith(hubTask =>
                    {
                        System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(1));

                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            BindingContextPageModel.IsBusy = false;

                            Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Recargar Guion", "Solicitud de recarga de Guion cuando termine el ciclo actual enviada con éxito.");
                        });
                    });

                    return;
                }

                if (task.Result == SCREENPLAY_HANDLING_MODE_IMMEDIATELY)
                {
                    Hubs.Client.DashboardHubClient.Instance.RequestPlayerReloadScreenplayAsync(model.Id, Cloud.SignalR.Connect.Common.Enumerations.ScreenplayHandlingImmediacyMode.Immediately).ContinueWith(hubTask =>
                    {
                        System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(1));

                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            BindingContextPageModel.IsBusy = false;

                            Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Recargar Guion", "Solicitud de recarga de Guion inmediatamente enviada con éxito.");
                        });
                    });

                    return;
                }

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = false;
                });
            });
        }

        private void PlayerOverviewEditButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel.IsBusy) return;

            var model = (sender as Xamarin.Forms.ImageButton).BindingContext as Models.View.PlayerViewModel;

            DisplayActionSheet("Opciones de Edición", "Cancelar", null, PLAYER_EDIT_DEFAULT, PLAYER_EDIT_SCHEDULED_RELAUNCH).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.Result == PLAYER_EDIT_SCHEDULED_RELAUNCH)
                    {
                        this.NavigationPushPageAsync(new ManagePlayerScheduledRelaunchsPage(model));
                    }
                    else if (task.Result == PLAYER_EDIT_DEFAULT)
                    {
                        this.NavigationPushPageAsync(new ConfigurePlayerPage(ReloadPlayer, model));
                    }
                });                
            });            
        }

        private void PlayerOverviewDeleteButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            DisplayAlert("Confirmar", "Está seguro que desea eliminar este reproductor?", "Aceptar", "Cancelar").ContinueWith(async task =>
            {
                if (!task.Result) return;

                BindingContextPageModel.IsBusy = true;

                var client = new Cloud.Api.Connect.Clients.PlayersApiConnectClient(Managers.SessionManager.Instance);

                var target = (sender as Xamarin.Forms.BindableObject).BindingContext as Models.View.PlayerViewModel;

                System.Exception failException = null;

                try
                {
                    await client.Delete(target.Id);
                }
                catch (System.Exception ex)
                {
                    failException = ex;
                }

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    if (failException == null)
                    {
                        BindingContextPageModel.Items.Remove(target);

                        Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Información", "Reproductor eliminado con éxito.");
                    }
                    else
                    {
                        (Xamarin.Forms.Application.Current as Support.Presentation.Helpers.Facade.IExceptionHandleHelperFacade).HandleException(failException);
                    }

                    BindingContextPageModel.IsBusy = false;
                });
            });
        }

        private void PlayerIterventionPlacardPicked_Callback(Cloud.Api.Common.Models.PlacardDisplayModel target, object param)
        {
            DisplayAlert("Confirmar", string.Format("Esta seguro que desea intervenir este reproductor con el cartel titulado \"{0}\"?", target.Label), "Intervenir", "Cancelar").ContinueWith(task =>
            {
                if (!task.Result) return;

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    Hubs.Client.DashboardHubClient.Instance.PlayerPresentInterventionPlacardAsync((param as Models.View.PlayerViewModel).Id, target.Id).ContinueWith(hubTask =>
                    {
                        System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(1));

                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            BindingContextPageModel.IsBusy = false;

                            Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Intervenir", "Solicitud de intervención con marco enviada con éxito.");
                        });
                    });
                });
            });
        }

        private void ReloadPlayer()
        {
            BindingContextPageModel.Items.Clear();

            LoadPlayers();
        }

        private void LoadPlayers()
        {
            BindingContextPageModel.IsBusy = true;

            System.Threading.Tasks.Task.Run(async () =>
            {
                var client = new Cloud.Api.Connect.Clients.PlayersApiConnectClient(Managers.SessionManager.Instance);

                return await client.GetPaginateAsync();
            }).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.Exception != null || task.IsCanceled)
                    {
                        (Xamarin.Forms.Application.Current as Support.Presentation.Helpers.Facade.IExceptionHandleHelperFacade).HandleException(task.Exception);
                    }
                    else
                    {
                        foreach (var model in task.Result.List)
                        {
                            BindingContextPageModel.Items.Add(new Models.View.PlayerViewModel
                            {
                                Id = model.Id,
                                Name = model.Name,
                                About = model.About,
                                Screenplay = model.Screenplay,
                                IsConnected = model.IsConnected,
                                AudioLevel = model.AudioLevel,
                                AudioEnabled = model.AudioEnabled,
                                OverlayId = model.OverlayId,
                                SerializedOverlayContext = model.SerializedOverlayContext
                            });
                        }

                        Hubs.Client.DashboardHubClient.Instance.GetReady(BindingContextPageModel.Items);
                    }

                    BindingContextPageModel.IsBusy = false;
                });
            });
        }
    }
}