using SmartSolucionesCuba.Viderhe.Extend.Common.Templating.Presentation.Extensions;
using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace SmartSolucionesCuba.Viderhe.Extend.SocialWelfare.Templating.Dashboard.Presentation.Pages
{
    public partial class InmersiveCityWeatherOverviewConfigurationPage : XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.InmersiveCityWeatherOverviewConfigurationPageModel>
    {
        private Support.Presentation.Models.View.PlacardViewModel placardViewModel;
        private SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider;
        private Support.Presentation.Helpers.Facade.IExceptionHandleHelperFacade exceptionHandleHelper;
        private Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade notificationPresenter;
        private System.Action successfulCompleteCallbackAction;

        public InmersiveCityWeatherOverviewConfigurationPage(SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider, Support.Presentation.Helpers.Facade.IExceptionHandleHelperFacade exceptionHandleHelper, Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade notificationPresenter, System.Action successfulCompleteCallbackAction)
            : this(null, httpClientHelperProvider, exceptionHandleHelper, notificationPresenter, successfulCompleteCallbackAction)
        {
        }

        public InmersiveCityWeatherOverviewConfigurationPage(Support.Presentation.Models.View.PlacardViewModel placardViewModel, SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider, Support.Presentation.Helpers.Facade.IExceptionHandleHelperFacade exceptionHandleHelper, Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade notificationPresenter, System.Action successfulCompleteCallbackAction)
            : base(Factories.InmersiveCityWeatherOverviewConfigurationPageModelFactory.Build(placardViewModel))
        {
            InitializeComponent();

            this.placardViewModel = placardViewModel;
            this.httpClientHelperProvider = httpClientHelperProvider;
            this.exceptionHandleHelper = exceptionHandleHelper;
            this.notificationPresenter = notificationPresenter;
            this.successfulCompleteCallbackAction = successfulCompleteCallbackAction;

            if (placardViewModel == null)
            {
                var templateDescriptor = new Descriptors.InmersiveCityWeatherOverviewTemplateDescriptor();

                Title = string.Format("Nuevo Marco ({0})", templateDescriptor.Name);

                IsEnabled = false;

                BindingContextPageModel.Service = Templating.Presentation.Enumerations.WeatherDataProvisionService.OpenWeatherMap;
            }
            else
            {
                Title = string.Format("Modificando Marco ({0})", placardViewModel.Template.Name);
            }

            BindingContextPageModel.PropertyChanged += BindingContextPageModel_PropertyChanged;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!IsEnabled)
            {
                IsEnabled = true;

                return;
            }

            ReloadRequiredData();
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            BindingContextPageModel.PropertyChanged -= BindingContextPageModel_PropertyChanged;

            placardViewModel = null;
            exceptionHandleHelper = null;
            httpClientHelperProvider = null;
            notificationPresenter = null;
            successfulCompleteCallbackAction = null;
        }

        private void BindingContextPageModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName == "WeatherCacheMicroServerHost" || eventArgs.PropertyName == "Service")
            {
                if (IsEnabled)
                {
                    BindingContextPageModel.City = null;
                }

                ReloadRequiredData();
            }
        }

        private void BackgroundImageGestureRecognizer_Tapped(object sender, System.EventArgs eventArgs)
        {
            IsEnabled = false;

            this.NavigationPushPageAsync(new Support.Presentation.Dashboard.Pages.ImagePickPage(exceptionHandleHelper, httpClientHelperProvider, BackgroundImagePicked_Callback, BindingContextPageModel.BackgroundCityImage));
        }

        private void ApplyToolbarItem_Clicked(object sender, System.EventArgs eventArgs)
        {
            BindingContextPageModel.IsBusy = true;

            // TODO Check configuration first!

            System.Threading.Tasks.Task.Run(async () =>
            {
                var client = new Cloud.Api.Connect.Clients.PlacardsApiConnectClient(httpClientHelperProvider);

                if (placardViewModel == null)
                {
                    await client.CreateAsync(new Cloud.Api.Common.Models.PlacardInputModel
                    {
                        Label = BindingContextPageModel.Label,
                        TemplateId = Descriptors.InmersiveCityWeatherOverviewTemplateDescriptor.ID,
                        SerializedContext = Newtonsoft.Json.JsonConvert.SerializeObject(new Templating.Presentation.Models.Context.InmersiveCityWeatherOverviewContextModel
                        {
                            RequireUnobstructedPresetation = BindingContextPageModel.RequireUnobstructedPresetation,
                            ScreenPresentationTime = System.TimeSpan.FromSeconds(BindingContextPageModel.ScreenPresentationTimeSecounds),
                            BackgroundCityImage = BindingContextPageModel.BackgroundCityImage.ToContextModel(),
                            WeatherCacheMicroServerHost = BindingContextPageModel.WeatherCacheMicroServerHost,
                            IsWeatherCacheMicroServerRequireSecureConnection = BindingContextPageModel.IsWeatherCacheMicroServerRequireSecureConnection,
                            IsWeatherCacheMicroServerSecureConnectionStrict = BindingContextPageModel.IsWeatherCacheMicroServerSecureConnectionStrict,
                            Service = BindingContextPageModel.Service,
                            City = BindingContextPageModel.City
                        })
                    });
                }
                else
                {
                    await client.EditAsync(new Cloud.Api.Common.Models.PlacardInputModel
                    {
                        Id = placardViewModel.Id,
                        Label = BindingContextPageModel.Label,
                        TemplateId = placardViewModel.Template.Id,
                        SerializedContext = Newtonsoft.Json.JsonConvert.SerializeObject(new Templating.Presentation.Models.Context.InmersiveCityWeatherOverviewContextModel
                        {
                            RequireUnobstructedPresetation = BindingContextPageModel.RequireUnobstructedPresetation,
                            ScreenPresentationTime = System.TimeSpan.FromSeconds(BindingContextPageModel.ScreenPresentationTimeSecounds),
                            BackgroundCityImage = BindingContextPageModel.BackgroundCityImage.ToContextModel(),
                            WeatherCacheMicroServerHost = BindingContextPageModel.WeatherCacheMicroServerHost,
                            IsWeatherCacheMicroServerRequireSecureConnection = BindingContextPageModel.IsWeatherCacheMicroServerRequireSecureConnection,
                            IsWeatherCacheMicroServerSecureConnectionStrict = BindingContextPageModel.IsWeatherCacheMicroServerSecureConnectionStrict,
                            Service = BindingContextPageModel.Service,
                            City = BindingContextPageModel.City
                        })
                    });
                }
            }).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = false;

                    if (task.Exception != null || task.IsCanceled)
                    {
                        exceptionHandleHelper.HandleException(task.Exception);
                    }
                    else
                    {
                        notificationPresenter.PresentNotification("Aviso", "Cambios aplicados con éxito.");

                        this.NavigationPopPageAsync();

                        successfulCompleteCallbackAction.Invoke();
                    }
                });
            });
        }

        private void CancelToolbarItem_Clicked(object sender, System.EventArgs eventArgs)
        {
            this.NavigationPopPageAsync();
        }

        private void SetWeatherCacheMicroServerHostButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            DisplayPromptAsync("Servidor Viderhe Weather", null, "Aceptar", "Cancelar").ContinueWith(task =>
            {
                if (task.IsCanceled || string.IsNullOrEmpty(task.Result) || string.IsNullOrWhiteSpace(task.Result)) return;

                BindingContextPageModel.WeatherCacheMicroServerHost = task.Result;
            });
        }

        private void CityPicker_SelectedIndexChanged(object sender, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel == null) return;

            BindingContextPageModel.City = (sender as Xamarin.Forms.Picker).SelectedItem as Weather.Api.Common.Models.CityModel;
        }

        private void BackgroundImagePicked_Callback(Support.Presentation.Models.View.ImageViewModel selection)
        {
            BindingContextPageModel.BackgroundCityImage = selection;

            if (string.IsNullOrEmpty(BindingContextPageModel.Label))
            {
                BindingContextPageModel.Label = selection.About;
            }
        }

        private void ReloadRequiredData()
        {
            var protocol = BindingContextPageModel.IsWeatherCacheMicroServerRequireSecureConnection ? "https" : "http";

            var baseUrl = string.Format("{0}://{1}/api/", protocol, BindingContextPageModel.WeatherCacheMicroServerHost);

            BindingContextPageModel.IsBusy = true;

            if (!System.Uri.IsWellFormedUriString(baseUrl, System.UriKind.Absolute)) return;

            var weatherApiConnectClient = new Weather.Api.Connect.Clients.OpenWeatherMapApiConnectClient(new SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper(baseUrl));

            weatherApiConnectClient.GetCitiesAsync().ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.IsCanceled || task.Exception != null)
                    {
                        exceptionHandleHelper.HandleException(task.Exception);                        
                    }
                    else
                    {
                        BindingContextPageModel.AvailableCities = task.Result;
                    }

                    BindingContextPageModel.PropertyChanged -= BindingContextPageModel_PropertyChanged;

                    if (placardViewModel != null)
                    {
                        CityPicker.SelectedItem = BindingContextPageModel.City;
                    }

                    BindingContextPageModel.Consolidate();

                    BindingContextPageModel.PropertyChanged += BindingContextPageModel_PropertyChanged;

                    BindingContextPageModel.IsBusy = false;

                    IsEnabled = true;
                });
            });
        }
    }
}