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
        private const string OPTION_COMMON_EDIT = "Modificar";
        private const string OPTION_COMMON_DELETE = "Eliminar";

        private const string OPTION_FRAME_PLACARD_EDIT = "Modificar Marco";
        private const string OPTION_FRAME_ORDER = "Cambiar Prioridad";
        private const string OPTION_FRAME_DELETE = "Quitar";

        private const string OPTION_PLACARD_SEQUENCE = "Agregar a Secuencia";

        private int currentPlacardsPageNumber;
        private int currentPlacardCollectionViewCellColumnCount = 3;
        private int placardPageLimit = 15;

        public ManageProgrammingPage() : base(null)
        {
            InitializeComponent();

            IsEnabled = false;

            BindingContext = new Models.Page.ManageProgrammingPageModel(ScreenplaySelection_Callback);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!IsEnabled) ReloadScreenplays();
        }

        private void AddPlacardButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            this.NavigationPushPageAsync(new TemplatePickPage(PlacardEditionCompleted_Callback));
        }

        private void PlacardOverviewOptionsButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            var target = (sender as Xamarin.Forms.BindableObject).BindingContext as Models.View.PlacardOverviewViewModel;

            var extraOptions = new System.Collections.Generic.List<string>
            {
                OPTION_COMMON_PREVIEW,
                OPTION_COMMON_EDIT
            };

            if (BindingContextPageModel.SelectedScreenplay != null)
            {
                extraOptions.Insert(0, OPTION_PLACARD_SEQUENCE);
            }

            if (target.Placard.Template == null)
            {
                extraOptions.Clear();
            }

            DisplayActionSheet("Opciones de Marco", OPTION_COMMON_CANCEL, OPTION_COMMON_DELETE, extraOptions.ToArray()).ContinueWith(actionSheetTask =>
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
                                    await client.Delete(target.Placard.Id);

                                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Información", "Marco eliminado con exito.");

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
                            var screenplayPlayerPage = new Support.Presentation.Player.Pages.ScreenplayPlayerPage(new Support.Presentation.Models.View.PlacardViewModel[] { target.Placard }, ScreenplayPresentationComplete_Callback, false);

                            this.NavigationPushModalPageAsync(screenplayPlayerPage);
                        });

                        break;

                    case OPTION_COMMON_EDIT:
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            var placardEditPage = Providers.PlacardConfigurationPageProvider.Instance.GetEditionPageByDescriptor(target.Placard, PlacardEditionCompleted_Callback);

                            if (placardEditPage != null)
                            {
                                this.NavigationPushPageAsync(placardEditPage);
                            }
                            else
                            {
                                Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Aviso", "Plantilla no compatible con la versión actual del sistema.");
                            }
                        });

                        break;

                    case OPTION_PLACARD_SEQUENCE:
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("Confirmar", "Esta seguro que desea agregar este marco a la secuencia del Guion seleccionado?", "Agregar", OPTION_COMMON_CANCEL).ContinueWith(async task =>
                            {
                                if (!task.Result) return;

                                BindingContextPageModel.IsBusy = true;

                                var client = new Cloud.Api.Connect.Clients.FramesApiConnectClient(Managers.SessionManager.Instance);

                                try
                                {
                                    await client.CreateAsync(new Cloud.Api.Common.Models.FrameInputModel
                                    {
                                        ScreenplayId = BindingContextPageModel.SelectedScreenplay.Id,
                                        PlacardId = target.Placard.Id,
                                        Priority = 1
                                    });

                                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Información", "Marco agregado a la secuencia con exito.");

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
                OPTION_FRAME_PLACARD_EDIT,
                OPTION_COMMON_PREVIEW
            };

            if (BindingContextPageModel.Frames.Length > 1)
            {
                extraOptions.Add(OPTION_FRAME_ORDER);
            }

            extraOptions.Add(OPTION_FRAME_DELETE);

            DisplayActionSheet("Opciones de Marco en Secuencia", OPTION_COMMON_CANCEL, null, extraOptions.ToArray()).ContinueWith(actionSheetTask =>
            {
                switch (actionSheetTask.Result)
                {
                    case OPTION_FRAME_PLACARD_EDIT:
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            var placardEditPage = Providers.PlacardConfigurationPageProvider.Instance.GetEditionPageByDescriptor(targetFrameViewModel.Placard, PlacardEditionCompleted_Callback);

                            if (placardEditPage != null)
                            {
                                this.NavigationPushPageAsync(placardEditPage);
                            }
                            else
                            {
                                Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Aviso", "Plantilla no compatible con la versión actual del sistema.");
                            }
                        });

                        break;

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
                                        Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Información", "Marco eliminado de la secuencia con exito.");

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
                            var screenplayPlayerPage = new Support.Presentation.Player.Pages.ScreenplayPlayerPage(new Support.Presentation.Models.View.PlacardViewModel[] { targetFrameViewModel.Placard }, ScreenplayPresentationComplete_Callback, false);

                            this.NavigationPushModalPageAsync(screenplayPlayerPage);
                        });

                        break;

                    case OPTION_FRAME_ORDER:
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            this.NavigationPushModalPageAsync(new FramePriorityChangePage(BindingContextPageModel.Frames, targetFrameViewModel, () => LoadCurrentScreenplayFrames()));
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
            var screenplayPlayerPage = new Support.Presentation.Player.Pages.ScreenplayPlayerPage(BindingContextPageModel.Frames.Select(m => m.Placard).ToArray(), ScreenplayPresentationComplete_Callback, false);

            this.NavigationPushModalPageAsync(screenplayPlayerPage);
        }

        private void AddScreenplayButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            this.NavigationPushPageAsync(new ConfigureScreenplayPage(ReloadScreenplays));
        }

        private void ScreenplayOptionsButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            var targetScreenplayViewModel = (sender as Xamarin.Forms.BindableObject).BindingContext as Models.View.ScreenplayViewModel;

            DisplayActionSheet("Opciones de Guion", OPTION_COMMON_CANCEL, null, OPTION_COMMON_EDIT, OPTION_COMMON_DELETE).ContinueWith(task =>
            {
                switch (task.Result)
                {
                    case OPTION_COMMON_DELETE:
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("Confirmar", "Esta seguro que desea eliminar el Guion seleccionado?", OPTION_COMMON_DELETE, OPTION_COMMON_CANCEL).ContinueWith(async dialogTask =>
                            {
                                if (!dialogTask.Result) return;

                                BindingContextPageModel.IsBusy = true;

                                var client = new Cloud.Api.Connect.Clients.ScreenplaysApiConnectClient(Managers.SessionManager.Instance);

                                try
                                {
                                    await client.Delete(targetScreenplayViewModel.Id);

                                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Información", "Guion eliminado con exito.");

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

        private void PlacardOverviewCollectionView_Scrolled(object sender, Xamarin.Forms.ItemsViewScrolledEventArgs eventArgs)
        {
            UpdatePlacardOverviewLoadByScrollContext(eventArgs);
        }

        private void PlacardOverviewCollectionWrapperView_SizeChanged(object sender, System.EventArgs eventArgs)
        {
            if (PlacardOverviewCollectionView.Width < 0 || PlacardOverviewCollectionView.Height < 0) return;

            if (BindingContextPageModel.IsBusy) return;

            if (!IsEnabled) return;

            PlacardEditionCompleted_Callback();
        }

        private void ScreenplaySelection_Callback(Models.View.ScreenplayViewModel target)
        {
            LoadCurrentScreenplayFrames();
        }

        private void ScreenplayPresentationComplete_Callback()
        {
            this.NavigationPopModalPageAsync();
        }

        private void PlacardEditionCompleted_Callback()
        {
            if (BindingContextPageModel.SelectedScreenplay == null)
            {
                LoadNextPlacardsPage(true);
            }
            else
            {
                LoadCurrentScreenplayFrames(() => LoadNextPlacardsPage(true));
            }
        }

        private void ReloadScreenplays()
        {
            BindingContextPageModel.IsBusy = true;

            LoadScreenplays();
        }

        private void LoadScreenplays()
        {
            BindingContextPageModel.IsBusy = true;

            var screenplaysApiConnectClient = new Cloud.Api.Connect.Clients.ScreenplaysApiConnectClient(Managers.SessionManager.Instance);

            screenplaysApiConnectClient.GetAsync().ContinueWith(task =>
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

                            if (PlacardOverviewCollectionView.Width > 0)
                            {
                                PlacardEditionCompleted_Callback();
                            }
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

            if (restart)
            {
                CalculatePlacardPresentationMetrics();

                currentPlacardsPageNumber = 0;

                foreach (var item in BindingContextPageModel.PlacardOverviews)
                {
                    item?.Dispose();
                }

                BindingContextPageModel.PlacardOverviews.Clear();

                BindingContextPageModel.PlacardOverviews = null;
            }

            var placardsApiConnectClient = new Cloud.Api.Connect.Clients.PlacardsApiConnectClient(Managers.SessionManager.Instance);

            _ = placardsApiConnectClient.GetPaginateAsync(++currentPlacardsPageNumber, placardPageLimit).ContinueWith(task =>
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
                              BindingContextPageModel.PlacardOverviews = new System.Collections.ObjectModel.ObservableCollection<Models.View.PlacardOverviewViewModel>(task.Result.List.Select(item => new Models.View.PlacardOverviewViewModel(item.ToViewModel())));
                          }
                          else
                          {
                              foreach (var item in task.Result.List)
                              {
                                  BindingContextPageModel.PlacardOverviews.Add(new Models.View.PlacardOverviewViewModel(item.ToViewModel(), true));
                              }
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

        private void CalculatePlacardPresentationMetrics()
        {
            var collumnReazon = PlacardOverviewCollectionView.Width / (200 + 10);

            currentPlacardCollectionViewCellColumnCount = System.Convert.ToInt32(System.Math.Round(collumnReazon, System.MidpointRounding.ToEven));

            if (currentPlacardCollectionViewCellColumnCount > collumnReazon)
            {
                currentPlacardCollectionViewCellColumnCount--;
            }

            if (currentPlacardCollectionViewCellColumnCount <= 0)
            {
                currentPlacardCollectionViewCellColumnCount = 1;
            }

            var collectionViewCellWidth = (PlacardOverviewCollectionView.Width - ((currentPlacardCollectionViewCellColumnCount - 1) * 10)) / currentPlacardCollectionViewCellColumnCount;

            var collectionViewCellHeight = collectionViewCellWidth + (collectionViewCellWidth * 0.20);

            var availableVerticalHeight = PlacardOverviewCollectionView.Height;

            var rowCount = 0;

            while (availableVerticalHeight > 0)
            {
                rowCount++;

                availableVerticalHeight -= collectionViewCellHeight + 10;
            }

            placardPageLimit = currentPlacardCollectionViewCellColumnCount * rowCount;
        }

        private void LoadCurrentScreenplayFrames(System.Action callback = null)
        {
            if (BindingContextPageModel.SelectedScreenplay == null)
            {
                BindingContextPageModel.Frames = new Support.Presentation.Models.View.FrameViewModel[] { };

                callback?.Invoke();

                return;
            }

            BindingContextPageModel.IsBusy = true;

            var client = new Cloud.Api.Connect.Clients.FramesApiConnectClient(Managers.SessionManager.Instance);

            client.FilterAsync(new Cloud.Api.Common.Models.Filter.FrameFilterModel { ScreenplayId = BindingContextPageModel.SelectedScreenplay.Id }).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.Exception != null || task.IsCanceled)
                    {
                        BindingContextPageModel.IsBusy = false;

                        BindingContextPageModel.Frames = new Support.Presentation.Models.View.FrameViewModel[] { };

                        BindingContextPageModel.SelectedScreenplay = null;
                    }
                    else
                    {
                        BindingContextPageModel.Frames = task.Result.OrderBy(e => e.Priority).Select(m => new Support.Presentation.Models.View.FrameViewModel
                        {
                            Id = m.Id,
                            Priority = m.Priority,
                            Placard = m.Placard.ToViewModel()
                        }).ToArray();

                        if (callback == null)
                        {
                            BindingContextPageModel.IsBusy = false;
                        }
                        else
                        {
                            callback.Invoke();
                        }
                    }
                });
            });
        }

        private void UpdatePlacardOverviewLoadByScrollContext(Xamarin.Forms.ItemsViewScrolledEventArgs scrollEventArgs)
        {
            if (scrollEventArgs.FirstVisibleItemIndex < 0 || scrollEventArgs.LastVisibleItemIndex < 0) return;

            if (scrollEventArgs.FirstVisibleItemIndex > 0)
            {
                for (int index = 0; index < scrollEventArgs.FirstVisibleItemIndex; index++)
                {
                    if (BindingContextPageModel.PlacardOverviews == null || BindingContextPageModel.PlacardOverviews.Count <= index) return;

                    BindingContextPageModel.PlacardOverviews.ElementAtOrDefault(index)?.ReleasePlacardModel();
                }
            }

            if (BindingContextPageModel.PlacardOverviews == null) return;

            var loadToEnd = BindingContextPageModel.PlacardOverviews.Count - scrollEventArgs.LastVisibleItemIndex - 1 <= currentPlacardCollectionViewCellColumnCount * 2;

            var effectiveLastVisibleItemIndex = loadToEnd ? BindingContextPageModel.PlacardOverviews.Count - 1 : scrollEventArgs.LastVisibleItemIndex + (currentPlacardCollectionViewCellColumnCount * 2);

            for (int index = scrollEventArgs.FirstVisibleItemIndex; index <= effectiveLastVisibleItemIndex; index++)
            {
                if (BindingContextPageModel.PlacardOverviews == null || BindingContextPageModel.PlacardOverviews.Count <= index) return;

                BindingContextPageModel.PlacardOverviews.ElementAt(index).LoadPlacardModel();
            }

            // TODO Find a better way to handle scrollEventArgs values for UWP
            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.UWP) return;

            if (loadToEnd) return;

            if (BindingContextPageModel.PlacardOverviews == null) return;

            if (effectiveLastVisibleItemIndex < BindingContextPageModel.PlacardOverviews.Count - 1)
            {
                for (int index = effectiveLastVisibleItemIndex + 1; index < BindingContextPageModel.PlacardOverviews.Count; index++)
                {
                    if (BindingContextPageModel.PlacardOverviews == null || BindingContextPageModel.PlacardOverviews.Count <= index) break;

                    BindingContextPageModel.PlacardOverviews.ElementAtOrDefault(index)?.ReleasePlacardModel();
                }
            }
        }
    }
}