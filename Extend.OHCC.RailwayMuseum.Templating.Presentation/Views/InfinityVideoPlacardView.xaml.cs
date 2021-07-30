namespace SmartSolucionesCuba.Viderhe.Extend.OHCC.RailwayMuseum.Templating.Presentation.Views
{
    public partial class InfinityVideoPlacardView : Common.Templating.Presentation.Views.BaseTemplatedPlacardPresentationView
    {
        private Common.Transitioning.Presentation.Transitions.AbstractTransition transition;

        public InfinityVideoPlacardView(Models.View.InfinityVideoPlacardViewModel model)
        {
            InitializeComponent();

            BindingContext = model;
        }

        public override Xamarin.Forms.Layout<Xamarin.Forms.View> WrapLayout => Wrapper;

        public override Xamarin.Forms.View PlacardView => PlacardLayout;

        public override void ConfigureLayout(Xamarin.Forms.View view)
        {
            Xamarin.Forms.Grid.SetColumn(view, 0);
            Xamarin.Forms.Grid.SetRow(view, 0);
        }

        private void VideoPlayer_StatusUpdated(object sender, System.EventArgs eventArgs)
        {
            var videoPlayer = sender as XFToolKit.Presentation.Views.Rendered.VideoPlayerView;

            if (videoPlayer.Status == XFToolKit.Presentation.Views.Rendered.VideoStatus.Playing)
            {
                if (transition != null)
                {
                    transition?.PerformMotion();

                    transition = null;
                }
                else if (!videoPlayer.IsVisible)
                {
                    VideoPlayerView.IsVisible = true;
                }
            }
            else if (transition == null && videoPlayer.Status != XFToolKit.Presentation.Views.Rendered.VideoStatus.Playing)
            {
                VideoPlayerView.IsVisible = false;
            }
        }

        public override void StartShowInWhenReady(Common.Transitioning.Presentation.Transitions.AbstractTransition transition)
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