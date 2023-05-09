using SmartSolucionesCuba.Viderhe.Extend.Common.Presentation.Views;

namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Player.Pages
{
    public partial class ScreenplayPlayerPage : XFToolKit.Mvvm.Pages.ContentPage
    {
        private Viderhe.Extend.Common.Templating.Presentation.Presenters.ScreenplayPresenter presenter;

        private readonly bool demo;
        private readonly bool empty;

        public ScreenplayPlayerPage(Models.View.PlacardViewModel[] placards, System.Action presentationEndedCallbackAction, bool loop, System.Action<System.Guid> placardPlayingCallbackAction = null, System.Action screenplayPresentIterationCompleteCallbackAction = null, Viderhe.Extend.Common.Overlaying.Presentation.Views.BaseOverlayView overlay = null, System.Action<BasePlacardPresentationView, BasePlacardPresentationView> placardViewTransitionCompleteCallbackAction = null)
        {
            InitializeComponent();

            presenter = new Viderhe.Extend.Common.Templating.Presentation.Presenters.ScreenplayPresenter(FramesContainer, PlacardViewModelToPresentationView, placards, Extend.Transitioning.Presentation.Providers.TransitionProvider.Instance, presentationEndedCallbackAction, loop, placardPlayingCallbackAction, screenplayPresentIterationCompleteCallbackAction, placardViewTransitionCompleteCallbackAction);

            Xamarin.Forms.DependencyService.RegisterSingleton<Viderhe.Extend.Common.Templating.Presentation.Presenters.Facade.IOvarlablePresenter>(presenter);

            if (overlay != null)
            {
                OverlayContainer.Content = overlay;
            }

            demo = !loop;
            empty = placards.Length <= 0;

            IsEnabled = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (IsEnabled) return;

            IsEnabled = true;

            Xamarin.Essentials.DeviceDisplay.MainDisplayInfoChanged += DeviceDisplay_MainDisplayInfoChanged;

            if (!demo)
            {
                if (empty) return;

                System.Threading.Tasks.Task.Delay(System.TimeSpan.FromSeconds(5)).ContinueWith(task =>
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(presenter.BeginPresentation);
                });

                return;
            }

            presenter.BeginPresentation();
        }

        private void DeviceDisplay_MainDisplayInfoChanged(object sender, Xamarin.Essentials.DisplayInfoChangedEventArgs e)
        {
            RequestPresentNextFrame();
        }

        public void RequestPresentNextFrame()
        {
            presenter.RequestPresentNextFrame();
        }

        public void RequestPresentPreviousFrame()
        {
            presenter.RequestPresentPreviousFrame();
        }

        public void PresentInterventionPlacards(Models.View.PlacardViewModel[] placards)
        {
            presenter.PresentInterventionPlacards(placards);
        }

        public void RequestRestart(Cloud.SignalR.Connect.Common.Enumerations.ScreenplayHandlingImmediacyMode immediacyMode)
        {
            presenter.RequestRestart(immediacyMode);
        }

        public void RequestReload(Cloud.SignalR.Connect.Common.Enumerations.ScreenplayHandlingImmediacyMode immediacyMode)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                presenter.RequestReload(immediacyMode);
            });
        }

        public void RequestPausePresentation()
        {

        }

        private BasePlacardPresentationView PlacardViewModelToPresentationView(Models.View.PlacardViewModel model, System.Func<Xamarin.Essentials.DisplayOrientation> getOrientationFunction)
        {
            return Extend.Templating.Presentation.Providers.PlacardPresentationViewProvider.Instance.GetFor(model, getOrientationFunction);
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            Xamarin.Essentials.DeviceDisplay.MainDisplayInfoChanged -= DeviceDisplay_MainDisplayInfoChanged;

            presenter.Dispose();

            presenter = null;

            Xamarin.Forms.DependencyService.RegisterSingleton<Viderhe.Extend.Common.Templating.Presentation.Presenters.Facade.IOvarlablePresenter>(null);
        }
    }
}