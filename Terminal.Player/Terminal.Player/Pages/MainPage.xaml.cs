using SmartSolucionesCuba.Viderhe.Support.Presentation.Extensions;
using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;
using SmartSolucionesCuba.XFToolKit.Presentation.Transform.Animations;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Terminal.Player.Pages
{
    public partial class MainPage : Xamarin.Forms.ContentPage
    {
        private Support.Presentation.Models.View.PlacardViewModel[] placards;

        private readonly System.Collections.Generic.Queue<Enumerations.ConfigurationStep> configurationStepQueue;

        private Extend.Common.Overlaying.Presentation.Views.BaseOverlayView overlayView;

        public MainPage()
        {
            InitializeComponent();

            configurationStepQueue = new System.Collections.Generic.Queue<Enumerations.ConfigurationStep>();

            IsEnabled = false;

            InitializeSteps();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!IsEnabled)
            {
                PresentNextConfigurationStep();
            }

            IsEnabled = true;
        }

        private void InitializeSteps()
        {
            configurationStepQueue.Enqueue(Enumerations.ConfigurationStep.Starting);
            configurationStepQueue.Enqueue(Enumerations.ConfigurationStep.PlayerRegistration);
            configurationStepQueue.Enqueue(Enumerations.ConfigurationStep.ScreenplayFetch);
        }

        private void PresentNextConfigurationStep()
        {
            if (configurationStepQueue.Count <= 0)
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        overlayView = Providers.AccountInformationProvider.Instance.AccountDefaultOverlayId == null ? null : Providers.OverlayPresentationViewProvider.Instance.GetOverlayPresentationView();

                        var screenplayPlayerPage = new Support.Presentation.Player.Pages.ScreenplayPlayerPage(placards, ScreenplayPresentationComplete_Callback, true, Providers.AccountInformationProvider.Instance, Managers.ConfigurationManager.Instance, PlacardPlayingCallback, overlayView, PlacardViewTransitionComplete_Callback)
                        {
                            BackgroundColor = Managers.SessionManager.Instance.AccountPrimaryColor,
                            BindingContext = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Orientation == Xamarin.Essentials.DisplayOrientation.Portrait ? Managers.SessionManager.Instance.AccountVerticalSplashImageSource : Managers.SessionManager.Instance.AccountHorizontalSplashImageSource
                        };

                        this.NavigationPushModalPageAsync(screenplayPlayerPage);

                        Hubs.Client.PlayerHubClient.Instance.GetReady();
                    });
                });

                return;
            }

            var configurationStep = configurationStepQueue.Dequeue();

            Models.View.ConfigurationStepViewModel model = null;

            switch (configurationStep)
            {
                case Enumerations.ConfigurationStep.PlayerRegistration:
                    model = new Models.View.ConfigurationStepViewModel
                    {
                        Title = "Registro",
                        DetailContentView = new Views.ConfigurationStepPlayerRegistrationDetailView()
                    };

                    break;
                case Enumerations.ConfigurationStep.ScreenplayFetch:
                    model = new Models.View.ConfigurationStepViewModel
                    {
                        Title = "Sincronización",
                        DetailContentView = new Views.ConfigurationStepScreenplayFetchingDetailView()
                    };

                    break;
                case Enumerations.ConfigurationStep.Starting:
                default:
                    model = new Models.View.ConfigurationStepViewModel
                    {
                        Title = "Iniciando",
                        DetailContentView = new Views.ConfigurationStepStartingDetailView()
                    };

                    break;
            }

            model.DetailContentView.Opacity = 0;

            var storyboard = new Storyboard
            {
                IsParallel = true,
                Delay = "250"
            };

            storyboard.Animations.Add(new FadeInAnimation { Target = StepTitleLabel, Direction = XFToolKit.Presentation.Transform.Enumerations.FadeDirection.Down });
            storyboard.Animations.Add(new FadeInAnimation { Target = model.DetailContentView, Direction = XFToolKit.Presentation.Transform.Enumerations.FadeDirection.Up });

            Models.View.ConfigurationStepViewModel completedConfigurationStepModel = null;

            if (BindingContext != null)
            {
                completedConfigurationStepModel = BindingContext as Models.View.ConfigurationStepViewModel;
            }

            BindingContext = model;

            completedConfigurationStepModel?.Dispose();

            if (configurationStep == Enumerations.ConfigurationStep.Starting)
            {
                storyboard.Animations.Add(new FadeToAnimation { Target = ProductIdentityContainer, Opacity = 1 });
                storyboard.Animations.Add(new FadeToAnimation { Target = CurrentTaskProgressBar, Opacity = 1 });

                System.Threading.Tasks.Task.Run(async () =>
                {
                    await storyboard.Begin();

                    await System.Threading.Tasks.Task.Delay(3000);

                    await RequestAccountAsync().ContinueWith(OnRequestAccountCallback);
                });
            }
            else if (configurationStep == Enumerations.ConfigurationStep.PlayerRegistration)
            {
                storyboard.Begin();
            }
            else if (configurationStep == Enumerations.ConfigurationStep.ScreenplayFetch)
            {
                System.Threading.Tasks.Task.Run(async () =>
                {
                    await storyboard.Begin();

                    await System.Threading.Tasks.Task.Delay(3000);

                    await RequestScreenplayPlacardsAsync().ContinueWith(OnRequestScreenplayPlacardsCallback);
                });
            }
        }

        private void CurentStepAnimateOut_Completed(object sender, System.EventArgs eventArgs)
        {
            (sender as AbstractAnimation).Completed -= CurentStepAnimateOut_Completed;

            System.Threading.Tasks.Task.Delay(System.TimeSpan.FromSeconds(1)).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(PresentNextConfigurationStep);
            });
        }

        private void ScreenplayPresentationComplete_Callback()
        {
            this.NavigationPopModalPageAsync().ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    InitializeSteps();

                    PresentNextConfigurationStep();
                });
            });
        }

        private void PlacardPlayingCallback(System.Guid placardId)
        {
            Hubs.Client.PlayerHubClient.Instance.NotifyPlayingPlacard(placardId);
        }

        private void PlacardViewTransitionComplete_Callback(Extend.Common.Presentation.Views.BasePlacardPresentationView incomingPlacardView, Extend.Common.Presentation.Views.BasePlacardPresentationView outgingPlacardView)
        {
            var incomingPlacardViewContext = incomingPlacardView.BindingContext as Extend.Common.Templating.Presentation.Models.View.AbstractPlacardViewModel;

            if (!(outgingPlacardView?.BindingContext is Extend.Common.Templating.Presentation.Models.View.AbstractPlacardViewModel outgoingPlacardViewContext))
            {
                if (incomingPlacardViewContext.RequireUnobstructedPresetation)
                {
                    overlayView?.ShowOut();
                }
            }
            else if (incomingPlacardViewContext.RequireUnobstructedPresetation != outgoingPlacardViewContext.RequireUnobstructedPresetation)
            {
                if (incomingPlacardViewContext.RequireUnobstructedPresetation)
                {
                    overlayView?.ShowOut();
                }
                else
                {
                    overlayView?.ShowIn();
                }
            }
        }

        #region Api Interaction Tasks and Callbacks

        private async System.Threading.Tasks.Task<Cloud.Api.Common.Models.AccountDisplayModel> RequestAccountAsync()
        {
            var client = new Cloud.Api.Connect.Clients.PlayersApiConnectClient(Managers.SessionManager.Instance);

            return await client.GetAccountAsync();
        }

        private async void OnRequestAccountCallback(System.Threading.Tasks.Task<Cloud.Api.Common.Models.AccountDisplayModel> task)
        {
            if (task.IsCanceled || task.Exception != null)
            {
                if (task.Exception != null)
                {
                    System.Exception targetException = task.Exception;

                    while (targetException is System.AggregateException)
                    {
                        targetException = targetException.InnerException;
                    }

                    if (targetException is SAPRESSC.Core.Api.Connect.UnspectedHttpStatusException && (targetException as SAPRESSC.Core.Api.Connect.UnspectedHttpStatusException).HttpStatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        if (!((BindingContext as Models.View.ConfigurationStepViewModel).DetailContentView is Views.ConfigurationStepPlayerRegistrationDetailView))
                        {
                            AnimateOutCurrentStep(CurentStepAnimateOut_Completed);
                        }
                    }
                }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                System.Threading.Tasks.Task.Delay(System.TimeSpan.FromSeconds(10)).ContinueWith(delayTask =>
                {
                    RequestAccountAsync().ContinueWith(OnRequestAccountCallback);
                });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
            else
            {
                Managers.SessionManager.Instance.AccountId = task.Result.Id;
                Managers.SessionManager.Instance.AccountName = task.Result.Name;
                Managers.SessionManager.Instance.AccountLogoImageSource = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(Managers.ConfigurationManager.Instance, task.Result.LogoImageId));
                Managers.SessionManager.Instance.AccountHorizontalSplashImageSource = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(Managers.ConfigurationManager.Instance, task.Result.HorizontalSplashImageId));
                Managers.SessionManager.Instance.AccountVerticalSplashImageSource = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(Managers.ConfigurationManager.Instance, task.Result.VerticalSplashImageId));
                Managers.SessionManager.Instance.AccountPrimaryColor = Xamarin.Forms.Color.FromHex(task.Result.PrimaryColor);
                Managers.SessionManager.Instance.AccountSecoundaryColor = Xamarin.Forms.Color.FromHex(task.Result.SecoundaryColor);
                Managers.SessionManager.Instance.AccountDefaultOverlayId = task.Result.DefaultOverlayId;
                Managers.SessionManager.Instance.AccountSerializedOverlayContext = task.Result.SerializedOverlayContext;

                var dynamicLoadExtensions = task.Result.Extensions.Where(item =>
                    item.Id != Extend.Essential.Descriptors.EssentialExtensionDescriptor.ID &&
                    item.Id != Extend.SpringMotion.Descriptors.SpringMotionExtensionDescriptor.ID &&
                    item.Id != Extend.SocialWelfare.Descriptors.SocialWelfareExtensionDescriptor.ID &&
                    item.Id != Extend.UICCamaguey.Descriptors.UICCamagueyExtensionDescriptor.ID &&
                    item.Id != Extend.OHCC.RailwayMuseum.Descriptors.RailwayMuseumOHCCExtensionDescriptor.ID)
                    .ToArray();

                foreach (var accountExtension in dynamicLoadExtensions)
                {
                    var extensionDirectoryInfo = new System.IO.DirectoryInfo(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "Extensions", accountExtension.Id.ToString(), accountExtension.Version));

                    // TODO Clear obsolete extension folder!!

                    if (!extensionDirectoryInfo.Exists)
                    {
                        extensionDirectoryInfo.Create();

                        await DownloadAccountExtensionBundleAsync(accountExtension.Id, extensionDirectoryInfo);
                    }

                    var extensionLibraryFileInfos = extensionDirectoryInfo.GetFiles();

                    if (extensionLibraryFileInfos.Length <= 0)
                    {
                        await DownloadAccountExtensionBundleAsync(accountExtension.Id, extensionDirectoryInfo);
                    }

                    foreach (var extensionLibraryFileInfo in extensionLibraryFileInfos)
                    {
                        var assembly = System.Reflection.Assembly.LoadFrom(extensionLibraryFileInfo.FullName);

                        System.AppDomain.CurrentDomain.Load(assembly.GetName());

                        var attributes = assembly.GetCustomAttributesSafe(typeof(Xamarin.Forms.ExportFontAttribute));

                        if (attributes == null || attributes.Length == 0) continue;

                        var length = attributes.Length;

                        for (var i = 0; i < length; i++)
                        {
                            var a = attributes[i];

                            var attribute = a as Xamarin.Forms.HandlerAttribute;

                            if (attribute == null && (a is Xamarin.Forms.ExportFontAttribute fa))
                            {
                                Xamarin.Forms.Internals.FontRegistrar.Register(fa, assembly);
                            }
                        }
                    }
                }

                if (configurationStepQueue.Count > 1)
                {
                    configurationStepQueue.Dequeue();
                }

                AnimateOutCurrentStep(CurentStepAnimateOut_Completed);
            }
        }

        private async System.Threading.Tasks.Task<Cloud.Api.Common.Models.PlacardDisplayModel[]> RequestScreenplayPlacardsAsync()
        {
            var client = new Cloud.Api.Connect.Clients.PlayersApiConnectClient(Managers.SessionManager.Instance);

            return await client.GetScreenplayPlacardsAsync();
        }

        private void OnRequestScreenplayPlacardsCallback(System.Threading.Tasks.Task<Cloud.Api.Common.Models.PlacardDisplayModel[]> task)
        {
            if (task.IsCanceled || task.Exception != null || task.Result == null)
            {
                System.Threading.Tasks.Task.Delay(System.TimeSpan.FromSeconds(10)).ContinueWith(delayTask =>
                {
                    RequestScreenplayPlacardsAsync().ContinueWith(OnRequestScreenplayPlacardsCallback);
                });
            }
            else
            {
                placards = task.Result.Select(m => m.ToViewModel()).ToArray();

                AnimateOutCurrentStep(CurentStepAnimateOut_Completed);
            }
        }

        #endregion

        private void AnimateOutCurrentStep(System.EventHandler completeEventHandler)
        {
            var model = BindingContext as Models.View.ConfigurationStepViewModel;

            var animationsStoryboard = new Storyboard
            {
                IsParallel = true
            };

            var lastAnimation = new FadeOutAnimation { Target = model.DetailContentView, Direction = XFToolKit.Presentation.Transform.Enumerations.FadeDirection.Up };

            lastAnimation.Completed += completeEventHandler;

            animationsStoryboard.Animations.Add(new FadeOutAnimation { Target = StepTitleLabel, Direction = XFToolKit.Presentation.Transform.Enumerations.FadeDirection.Down });
            animationsStoryboard.Animations.Add(lastAnimation);

            animationsStoryboard.Begin();
        }

        private async System.Threading.Tasks.Task DownloadAccountExtensionBundleAsync(System.Guid extensionId, System.IO.DirectoryInfo extensionDirectoryInfo)
        {
            var extensionsApiConnectClient = new Cloud.Api.Connect.Clients.ExtensionsApiConnectClient(Managers.SessionManager.Instance.HttpClientHelper);

            var extensionBundleModules = await extensionsApiConnectClient.GetPlayerBundleModulesAsync(extensionId);

            foreach (var extensionBundleModule in extensionBundleModules)
            {
                using (var extensionLibraryFileStream = System.IO.File.Create(System.IO.Path.Combine(extensionDirectoryInfo.FullName, extensionBundleModule.FileName)))
                {
                    extensionLibraryFileStream.Write(extensionBundleModule.FileContent, 0, extensionBundleModule.FileContent.Length);
                }
            }
        }
    }
}
