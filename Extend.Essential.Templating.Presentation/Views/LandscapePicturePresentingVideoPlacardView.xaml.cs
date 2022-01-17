namespace SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Presentation.Views
{
    public partial class LandscapePicturePresentingVideoPlacardView : Common.Templating.Presentation.Views.BaseTemplatedPlacardPresentationView
    {
        private Common.Transitioning.Presentation.Transitions.Contract.AbstractTransition transition;

        public LandscapePicturePresentingVideoPlacardView(Models.View.LandscapePicturePresentingVideoPlacardViewModel model)
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
            if (PresentedVideo.Status == XFToolKit.Presentation.Views.Rendered.VideoStatus.Playing)
            {
                base.StartShowInWhenReady(transition);

                return;
            }

            this.transition = transition;
        }

        public override void PerformPlacardStarup()
        {
            base.PerformPlacardStarup();

            var model = BindingContext as Models.View.LandscapePicturePresentingVideoPlacardViewModel;

            var width = PlacardLayout.Height * model.PresenterImageWidth / model.PresenterImageHeigth;

            if (model.PresenterSide == Enumerations.LandscapePresentationSide.Left)
            {
                PresenterImage.TranslationX = width * -1;
            }
            else
            {
                PresenterImage.TranslationX = width;
            }

            PresenterImage.IsVisible = true;

            var internalShowInStoryboard = new XFToolKit.Presentation.Transform.Animations.Storyboard
            {
                Delay = "2000",
                IsParallel = true
            };

            internalShowInStoryboard.Animations.Add(new XFToolKit.Presentation.Transform.Animations.TranslateToXAnimation
            {
                Target = PresenterImage,
                TranslateX = 0,
                EasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicOut
            });

            if (model.PresentedVideoCenteredAtPresentation)
            {
                internalShowInStoryboard.Animations.Add(new XFToolKit.Presentation.Transform.Animations.TranslateToXAnimation
                {
                    Target = PresentedVideo,
                    TranslateX = model.PresenterSide == Enumerations.LandscapePresentationSide.Left ? width / 2 : width / 2 * -1,
                    EasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicOut,
                    Delay = "100"
                });
            }

            internalShowInStoryboard.Begin();
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            if (PresentedVideo.Status == XFToolKit.Presentation.Views.Rendered.VideoStatus.Playing)
            {
                PresentedVideo.StatusUpdated -= VideoPlayer_StatusUpdated;

                PresentedVideo.Stop();
            }

            transition = null;
        }
    }
}