namespace SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Presentation.Views
{
    public partial class SimpleVideoPlacardView : Common.Templating.Presentation.Views.BaseTemplatedPlacardPresentationView
    {
        private Common.Transitioning.Presentation.Transitions.Contract.AbstractTransition transition;

        public SimpleVideoPlacardView(Models.View.SimpleVideoPlacardViewModel model, Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider)
        {
            InitializeComponent();

            BindingContext = model;
        }

        public override Xamarin.Forms.Layout<Xamarin.Forms.View> WrapLayout => Wrapper;

        public override Xamarin.Forms.View PlacardView => VideoPlayerView;

        public override void ConfigureLayout(Xamarin.Forms.View view)
        {
            Xamarin.Forms.Grid.SetColumn(view, 0);
            Xamarin.Forms.Grid.SetRow(view, 0);
        }

        private void VideoPlayer_StatusUpdated(object sender, System.EventArgs eventArgs)
        {
            var videoPlayer = sender as XFToolKit.Presentation.Views.Rendered.VideoPlayerView;

            if (videoPlayer.Status == XFToolKit.Presentation.Views.Rendered.VideoStatus.Ended)
            {
                videoPlayer.StatusUpdated -= VideoPlayer_StatusUpdated;

                EndPresentation();
            }
            else if (videoPlayer.Status == XFToolKit.Presentation.Views.Rendered.VideoStatus.Playing)
            {
                transition?.PerformMotion();

                transition = null;
            }
        }

        public override void StartShowInWhenReady(Common.Transitioning.Presentation.Transitions.Contract.AbstractTransition transition)
        {
            if (VideoPlayerView.Status == XFToolKit.Presentation.Views.Rendered.VideoStatus.Playing)
            {
                base.StartShowInWhenReady(transition);

                return;
            }

            this.transition = transition;
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            if (VideoPlayerView.Status == XFToolKit.Presentation.Views.Rendered.VideoStatus.Playing)
            {
                VideoPlayerView.StatusUpdated -= VideoPlayer_StatusUpdated;

                VideoPlayerView.Stop();
            }

            transition = null;
        }
    }
}