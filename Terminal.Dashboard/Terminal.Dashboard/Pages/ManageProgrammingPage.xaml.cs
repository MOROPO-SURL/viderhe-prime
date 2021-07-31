using SmartSolucionesCuba.Viderhe.Support.Presentation.Extensions;
using SmartSolucionesCuba.Viderhe.Support.Presentation.Helpers.Facade;
using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Pages
{
    public partial class ManageProgrammingPage : XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.ManageProgrammingPageModel>
    {
        private const string OPTION_COMMON_PREVIEW = "Pre-Visualizar";
        private const string OPTION_COMMON_CANCEL = "Cancelar";
        private const string OPTION_COMMON_EDIT = "Editar";
        private const string OPTION_COMMON_DELETE = "Eliminar";

        private const string OPTION_FRAME_ORDERTO_BEFORE = "Prioridad (mostrar antes)";
        private const string OPTION_FRAME_ORDERTO_AFTER = "Prioridad (mostrar después)";
        private const string OPTION_FRAME_DELETE = "Quitar";

        private const string OPTION_PLACARD_SEQUENCE = "Agregar a Secuencia";

        private int currentPlacardsPageNumber;

        public ManageProgrammingPage() : base(null)
        {
            InitializeComponent();

            IsEnabled = false;

            BindingContext = new Models.Page.ManageProgrammingPageModel(ScreenplaySelection_Callback);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();            

            if (!IsEnabled)
            {
                ReloadScreenplays();
            }
        }

        private void AddPlacardButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            this.NavigationPushPageAsync(new TemplatePickPage(RequireReloadPlacards_Callback));
        }

        private void PlacardOverviewOptionsButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            var targetPlacardViewModel = (sender as Xamarin.Forms.BindableObject).BindingContext as Support.Presentation.Models.View.PlacardViewModel;

            var extraOptions = new System.Collections.Generic.List<string>
            {
                OPTION_COMMON_PREVIEW,
                OPTION_COMMON_EDIT,
                OPTION_COMMON_DELETE
            };

            if (BindingContextPageModel.SelectedScreenplay != null)
            {
                extraOptions.Insert(0, OPTION_PLACARD_SEQUENCE);
            }

            DisplayActionSheet("Opciones de Marco", OPTION_COMMON_CANCEL, null, extraOptions.ToArray()).ContinueWith(actionSheetTask =>
            {
                switch (actionSheetTask.Result)
                {
                    case OPTION_COMMON_DELETE:
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("Confirmar", "Esta seguro que desea eliminar el marco seleccionado?", OPTION_COMMON_DELETE, OPTION_COMMON_CANCEL).ContinueWith(async task =>
                            {
                                if (!task.Result) return;

                                BindingContextPageModel.IsBusy = true;

                                var client = new Cloud.Api.Connect.Clients.PlacardsApiConnectClient(Managers.SessionManager.Instance);

                                try
                                {
                                    await client.Delete(targetPlacardViewModel.Id);

                                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Presenters.NotificationPresenter.Instance.PresentNotification("Información", "Marco eliminado con exito.");

                                        LoadNextPlacardsPage(true, BindingContextPageModel.SelectedScreenplay != null);
                                    });
                                }
                                catch (System.Exception e)
                                {
                                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                    {
                                        BindingContextPageModel.IsBusy = false;

                                        (Xamarin.Forms.Application.Current as IExceptionHandleHelperFacade).HandleException(e);
                                    });
                                }
                            });
                        });

                        break;

                    case OPTION_COMMON_PREVIEW:
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            var screenplayPlayerPage = new Support.Presentation.Player.Pages.ScreenplayPlayerPage(new Support.Presentation.Models.View.PlacardViewModel[] { targetPlacardViewModel }, ScreenplayPresentationComplete_Callback, false, Providers.AccountInformationProvider.Instance, Providers.CloudServerProvider.Instance);

                            this.NavigationPushModalPageAsync(screenplayPlayerPage);
                        });

                        break;

                    case OPTION_COMMON_EDIT:
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            var placardEditPage = Providers.PlacardConfigurationPageProvider.Instance.GetEditionPageByDescriptor(targetPlacardViewModel, RequireReloadPlacards_Callback);

                            if (placardEditPage != null)
                            {
                                this.NavigationPushPageAsync(placardEditPage);
                            }
                            else
                            {
                                Presenters.NotificationPresenter.Instance.PresentNotification("Aviso", "Plantilla no compatible con la versión actual del sistema.");
                            }
                        });

                        break;

                    case OPTION_PLACARD_SEQUENCE:
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("Confirmar", "Esta seguro que desea agregar este marco a la secuencia del guión seleccionado?", "Agregar", OPTION_COMMON_CANCEL).ContinueWith(async task =>
                            {
                                if (!task.Result) return;

                                BindingContextPageModel.IsBusy = true;

                                var client = new Cloud.Api.Connect.Clients.FramesApiConnectClient(Managers.SessionManager.Instance);

                                try
                                {
                                    await client.CreateAsync(new Cloud.Api.Common.Models.FrameInputModel
                                    {
                                        ScreenplayId = BindingContextPageModel.SelectedScreenplay.Id,
                                        PlacardId = targetPlacardViewModel.Id,
                                        Priority = 1
                                    });

                                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Presenters.NotificationPresenter.Instance.PresentNotification("Información", "Marco agregado a la secuencia con exito.");

                                        LoadCurrentScreenplayFrames();
                                    });
                                }
                                catch (System.Exception ex)
                                {
                                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                    {
                                        BindingContextPageModel.IsBusy = false;

                                        (Xamarin.Forms.Application.Current as IExceptionHandleHelperFacade).HandleException(ex);
                                    });
                                }
                            });
                        });

                        break;

                    case OPTION_COMMON_CANCEL:
                    default:
                        break;
                }
            });
        }

        private void FrameOverviewOptionsButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            var targetFrameViewModel = (sender as Xamarin.Forms.BindableObject).BindingContext as Support.Presentation.Models.View.FrameViewModel;

            var extraOptions = new System.Collections.Generic.List<string>
            {
                OPTION_COMMON_PREVIEW
            };

            if (targetFrameViewModel.CanOrderUp)
            {
                extraOptions.Add(OPTION_FRAME_ORDERTO_BEFORE);
            }

            if (targetFrameViewModel.CanOrderDown)
            {
                extraOptions.Add(OPTION_FRAME_ORDERTO_AFTER);
            }

            extraOptions.Add(OPTION_FRAME_DELETE);

            DisplayActionSheet("Opciones de Marco en Secuencia", OPTION_COMMON_CANCEL, null, extraOptions.ToArray()).ContinueWith(actionSheetTask =>
            {
                switch (actionSheetTask.Result)
                {
                    case OPTION_FRAME_DELETE:
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("Confirmar", "Esta seguro que desea eliminar de la secuencia el marco seleccionado?", OPTION_FRAME_DELETE, OPTION_COMMON_CANCEL).ContinueWith(async dialogTask =>
                            {
                                if (!dialogTask.Result) return;

                                BindingContextPageModel.IsBusy = true;

                                var client = new Cloud.Api.Connect.Clients.FramesApiConnectClient(Managers.SessionManager.Instance);

                                try
                                {
                                    await client.Delete(targetFrameViewModel.Id);

                                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Presenters.NotificationPresenter.Instance.PresentNotification("Información", "Marco eliminado de la secuencia con exito.");

                                        LoadCurrentScreenplayFrames();
                                    });
                                }
                                catch (System.Exception ex)
                                {
                                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                    {
                                        BindingContextPageModel.IsBusy = false;

                                        (Xamarin.Forms.Application.Current as IExceptionHandleHelperFacade).HandleException(ex);
                                    });
                                }
                            });
                        });

                        break;

                    case OPTION_COMMON_PREVIEW:
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            var screenplayPlayerPage = new Support.Presentation.Player.Pages.ScreenplayPlayerPage(new Support.Presentation.Models.View.PlacardViewModel[] { targetFrameViewModel.Placard }, ScreenplayPresentationComplete_Callback, false, Providers.AccountInformationProvider.Instance, Providers.CloudServerProvider.Instance);

                            this.NavigationPushModalPageAsync(screenplayPlayerPage);
                        });

                        break;

                    case OPTION_FRAME_ORDERTO_BEFORE:
                        BindingContextPageModel.IsBusy = true;

                        System.Threading.Tasks.Task.Run(async () =>
                        {
                            var client = new Cloud.Api.Connect.Clients.FramesApiConnectClient(Managers.SessionManager.Instance);

                            await client.OrderUpAsync(targetFrameViewModel.Id);
                        }).ContinueWith(task =>
                        {
                            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                            {
                                if (task.Exception != null && !task.IsCanceled)
                                {
                                    BindingContextPageModel.IsBusy = false;

                                    (Xamarin.Forms.Application.Current as IExceptionHandleHelperFacade).HandleException(task.Exception);
                                }
                                else
                                {
                                    Presenters.NotificationPresenter.Instance.PresentNotification("Información", "Prioridad del marco aumentada con exito.");

                                    LoadCurrentScreenplayFrames();
                                }
                            });
                        });

                        break;

                    case OPTION_FRAME_ORDERTO_AFTER:
                        BindingContextPageModel.IsBusy = true;

                        System.Threading.Tasks.Task.Run(async () =>
                        {
                            var client = new Cloud.Api.Connect.Clients.FramesApiConnectClient(Managers.SessionManager.Instance);

                            await client.OrderDownAsync(targetFrameViewModel.Id);
                        }).ContinueWith(task =>
                        {
                            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                            {
                                if (task.Exception != null && !task.IsCanceled)
                                {
                                    BindingContextPageModel.IsBusy = false;

                                    (Xamarin.Forms.Application.Current as IExceptionHandleHelperFacade).HandleException(task.Exception);
                                }
                                else
                                {
                                    Presenters.NotificationPresenter.Instance.PresentNotification("Información", "Prioridad del marco disminuida con exito.");

                                    LoadCurrentScreenplayFrames();
                                }
                            });
                        });

                        break;

                    case OPTION_COMMON_CANCEL:
                    default:
                        break;
                }
            });
        }

        private void ScreenplayFramesPlayButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            var screenplayPlayerPage = new Support.Presentation.Player.Pages.ScreenplayPlayerPage(BindingContextPageModel.Frames.Select(m => m.Placard).ToArray(), ScreenplayPresentationComplete_Callback, false, Providers.AccountInformationProvider.Instance, Providers.CloudServerProvider.Instance);

            this.NavigationPushModalPageAsync(screenplayPlayerPage);
        }

        private void AddScreenplayButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            this.NavigationPushPageAsync(new ConfigureScreenplayPage(ReloadScreenplays));
        }

        private void ScreenplayOptionsButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            var targetScreenplayViewModel = (sender as Xamarin.Forms.BindableObject).BindingContext as Models.View.ScreenplayViewModel;

            DisplayActionSheet("Opciones de Guión", OPTION_COMMON_CANCEL, null, OPTION_COMMON_EDIT, OPTION_COMMON_DELETE).ContinueWith(task =>
            {
                switch (task.Result)
                {
                    case OPTION_COMMON_DELETE:
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("Confirmar", "Esta seguro que desea eliminar el guión seleccionado?", OPTION_COMMON_DELETE, OPTION_COMMON_CANCEL).ContinueWith(async dialogTask =>
                            {
                                if (!dialogTask.Result) return;

                                BindingContextPageModel.IsBusy = true;

                                var client = new Cloud.Api.Connect.Clients.ScreenplaysApiConnectClient(Managers.SessionManager.Instance);

                                try
                                {
                                    await client.Delete(targetScreenplayViewModel.Id);

                                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Presenters.NotificationPresenter.Instance.PresentNotification("Información", "Guión eliminado con exito.");

                                        LoadScreenplays();
                                    });
                                }
                                catch (System.Exception e)
                                {
                                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                    {
                                        BindingContextPageModel.IsBusy = false;

                                        (Xamarin.Forms.Application.Current as IExceptionHandleHelperFacade).HandleException(e);
                                    });
                                }
                            });
                        });

                        break;

                    case OPTION_COMMON_EDIT:
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            this.NavigationPushPageAsync(new ConfigureScreenplayPage(ReloadScreenplays, targetScreenplayViewModel));
                        });

                        break;

                    case OPTION_COMMON_CANCEL:
                    default:
                        break;
                }
            });
        }

        private void ReloadToolbarItem_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel.IsBusy) return;

            IsEnabled = false;

            BindingContextPageModel.IsBusy = true;

            LoadScreenplays();
        }

        private void ToogleUseLayoutVariantToolbarItem_Clicked(object sender, System.EventArgs eventArgs)
        {
            BindingContextPageModel.UseExpandedLayout = !BindingContextPageModel.UseExpandedLayout;
        }

        private void LoadNextPlacardsPageButtton_Clicked(object sender, System.EventArgs eventArgs)
        {
            LoadNextPlacardsPage();
        }

        private void ScreenplayCollectionView_SelectionChanged(object sender, Xamarin.Forms.SelectionChangedEventArgs eventArgs)
        {
            var selecction = (sender as Xamarin.Forms.CollectionView).SelectedItem;

            BindingContextPageModel.SelectedScreenplay = selecction != null ? selecction as Models.View.ScreenplayViewModel : null;
        }

        private void ScreenplaySelection_Callback(Models.View.ScreenplayViewModel target)
        {
            LoadCurrentScreenplayFrames();
        }

        private void ScreenplayPresentationComplete_Callback()
        {
            this.NavigationPopModalPageAsync();
        }

        private void ReloadScreenplays()
        {
            BindingContextPageModel.IsBusy = true;

            LoadScreenplays();
        }

        private void RequireReloadPlacards_Callback()
        {
            LoadNextPlacardsPage(true);
        }

        private void LoadScreenplays()
        {
            BindingContextPageModel.IsBusy = true;

            System.Threading.Tasks.Task.Run(async () =>
            {
                var client = new Cloud.Api.Connect.Clients.ScreenplaysApiConnectClient(Managers.SessionManager.Instance);

                return await client.GetAsync();
            }).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.Screenplays = task.Result.Select(m => new Models.View.ScreenplayViewModel
                    {
                        Id = m.Id,
                        Title = m.Title,
                        About = m.About
                    }).ToArray();

                    BindingContextPageModel.SelectedScreenplay = null;

                    if (task.Exception == null && !task.IsCanceled)
                    {
                        if (!IsEnabled)
                        {
                            IsEnabled = true;

                            RequireReloadPlacards_Callback();
                        }
                        else
                        {
                            BindingContextPageModel.IsBusy = false;
                        }
                    }
                    else
                    {
                        (Xamarin.Forms.Application.Current as IExceptionHandleHelperFacade).HandleException(task.Exception);

                        BindingContextPageModel.IsBusy = false;
                    }
                });
            });
        }

        private void LoadNextPlacardsPage(bool restart = false, bool reloadCurrentScreenplayFrames = false)
        {
            BindingContextPageModel.IsBusy = true;

            if (restart) currentPlacardsPageNumber = 0;

            var placardsApiConnectClient = new Cloud.Api.Connect.Clients.PlacardsApiConnectClient(Managers.SessionManager.Instance);

            placardsApiConnectClient.GetPaginateAsync(++currentPlacardsPageNumber, 15).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.Exception != null)
                    {
                        (Xamarin.Forms.Application.Current as IExceptionHandleHelperFacade).HandleException(task.Exception);
                    }
                    else
                    {
                        if (currentPlacardsPageNumber == 1)
                        {
                            BindingContextPageModel.Placards.Clear();
                        }

                        foreach (var item in task.Result.List)
                        {
                            BindingContextPageModel.Placards.Add(item.ToViewModel());
                        }

                        BindingContextPageModel.HasMorePlacards = task.Result.HasNext && task.Result.List.Count > 0;
                    }

                    if (reloadCurrentScreenplayFrames)
                    {
                        LoadCurrentScreenplayFrames();
                    }
                    else
                    {
                        BindingContextPageModel.IsBusy = false;
                    }
                });
            });

        }

        private void LoadCurrentScreenplayFrames()
        {
            BindingContextPageModel.IsBusy = true;

            System.Threading.Tasks.Task.Run(async () =>
            {
                var client = new Cloud.Api.Connect.Clients.FramesApiConnectClient(Managers.SessionManager.Instance);

                var result = await client.FilterAsync(new Cloud.Api.Common.Models.Filter.FrameFilterModel { ScreenplayId = BindingContextPageModel.SelectedScreenplay.Id });

                var index = 0;
                var count = result.Count();

                BindingContextPageModel.Frames = result.OrderBy(e => e.Priority).Select(m => new Support.Presentation.Models.View.FrameViewModel
                {
                    Id = m.Id,
                    Priority = m.Priority,
                    Placard = m.Placard.ToViewModel(),
                    CanOrderUp = index++ > 0,
                    CanOrderDown = index != count
                }).ToArray();
            }).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = false;

                    if (task.Exception != null || task.IsCanceled)
                    {
                        BindingContextPageModel.Frames = new Support.Presentation.Models.View.FrameViewModel[] { };

                        BindingContextPageModel.SelectedScreenplay = null;
                    }
                });
            });
        }
    }
}