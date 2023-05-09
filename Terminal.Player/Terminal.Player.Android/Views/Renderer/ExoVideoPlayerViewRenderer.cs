using Com.Google.Android.Exoplayer2;
using Com.Google.Android.Exoplayer2.UI;
using Com.Google.Android.Exoplayer2.Upstream;
using Java.Lang;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.ExportRenderer(typeof(SmartSolucionesCuba.XFToolKit.Presentation.Views.Rendered.VideoPlayerView), typeof(SmartSolucionesCuba.Viderhe.Terminal.Player.Droid.Views.Renderer.ExoVideoPlayerViewRenderer))]

namespace SmartSolucionesCuba.Viderhe.Terminal.Player.Droid.Views.Renderer
{
    public class ExoVideoPlayerViewRenderer : ViewRenderer<XFToolKit.Presentation.Views.Rendered.VideoPlayerView, PlayerView>, IPlayerEventListener
    {
        private bool buffered;

        private float volumenBackup;

        public ExoVideoPlayerViewRenderer(Android.Content.Context context) : base(context)
        {
            buffered = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<XFToolKit.Presentation.Views.Rendered.VideoPlayerView> args)
        {
            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                if (Control == null)
                {
                    var playerView = new PlayerView(Context)
                    {
                        Player = ExoPlayerFactory.NewSimpleInstance(Context, new Com.Google.Android.Exoplayer2.Trackselection.DefaultTrackSelector()),
                        ControllerAutoShow = false,
                        UseController = false,
                        ControllerHideOnTouch = false,
                    };

                    playerView.Player.AddListener(this);

                    SetNativeControl(playerView);

                    volumenBackup = (Control.Player as SimpleExoPlayer).Volume;
                }

                SetAreTransportControlsEnabled();

                SetAspectStrech();

                SetIsMuted();

                SetAutoPlay();

                if (Element.AutoPlay)
                {
                    SetSource();
                }

                args.NewElement.PlayRequested += OnPlayRequested;
                args.NewElement.StopRequested += OnStopRequested;
            }

            if (args.OldElement != null)
            {
                args.NewElement.PlayRequested += OnPlayRequested;
                args.OldElement.StopRequested -= OnStopRequested;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                Control.Player.RemoveListener(this);

                Control.Player.Release();

                Control.Player.Dispose();

                Control.Player = null;
            }

            base.Dispose(disposing);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            if (args.PropertyName == XFToolKit.Presentation.Views.Rendered.VideoPlayerView.ShowControlsProperty.PropertyName)
            {
                SetAreTransportControlsEnabled();
            }
            else if (args.PropertyName == XFToolKit.Presentation.Views.Rendered.VideoPlayerView.StrechAspectProperty.PropertyName)
            {
                SetAspectStrech();
            }
            else if (args.PropertyName == XFToolKit.Presentation.Views.Rendered.VideoPlayerView.SourceProperty.PropertyName)
            {
                SetSource();
            }
            else if (args.PropertyName == XFToolKit.Presentation.Views.Rendered.VideoPlayerView.AutoPlayProperty.PropertyName)
            {
                SetAutoPlay();
            }
            else if (args.PropertyName == XFToolKit.Presentation.Views.Rendered.VideoPlayerView.IsMutedProperty.PropertyName)
            {
                SetIsMuted();
            }
        }

        private void SetAreTransportControlsEnabled()
        {
            if (Element.ShowControls)
            {
                Control.ShowController();
            }
            else
            {
                Control.HideController();
            }
        }

        private void SetAspectStrech()
        {
            switch (Element.StrechAspect)
            {
                case Xamarin.Forms.Aspect.AspectFit:
                    Control.ResizeMode = AspectRatioFrameLayout.ResizeModeFit;

                    break;
                case Xamarin.Forms.Aspect.AspectFill:
                    Control.ResizeMode = AspectRatioFrameLayout.ResizeModeZoom;

                    break;
                case Xamarin.Forms.Aspect.Fill:
                default:
                    Control.ResizeMode = AspectRatioFrameLayout.ResizeModeFill;

                    break;
            }
        }

        private void SetAutoPlay()
        {
            Control.Player.PlayWhenReady = Element.AutoPlay;
        }

        private void SetIsMuted()
        {
            (Control.Player as SimpleExoPlayer).Volume = Element.IsMuted ? 0f : volumenBackup;
        }

        private void SetSource()
        {
            var mediaSourceFactory = new Com.Google.Android.Exoplayer2.Source.ExtractorMediaSource.Factory(new DefaultDataSourceFactory(Context, "Viderhe ExoPlayer"));

            Com.Google.Android.Exoplayer2.Source.IMediaSource mediaSource = null;

            if (Element.Source is XFToolKit.Presentation.Sources.Video.UriVideoSource)
            {
                var uri = (Element.Source as XFToolKit.Presentation.Sources.Video.UriVideoSource).Uri;

                if (!string.IsNullOrWhiteSpace(uri))
                {
                    mediaSource = mediaSourceFactory.CreateMediaSource(Android.Net.Uri.Parse(uri));
                }
            }
            else if (Element.Source is XFToolKit.Presentation.Sources.Video.FileVideoSource)
            {
                var filename = (Element.Source as XFToolKit.Presentation.Sources.Video.FileVideoSource).File;

                if (!string.IsNullOrWhiteSpace(filename))
                {
                    mediaSource = mediaSourceFactory.CreateMediaSource(Android.Net.Uri.Parse(filename));
                }
            }
            else if (Element.Source is XFToolKit.Presentation.Sources.Video.ResourceVideoSource)
            {
                var package = Context.PackageName;

                var path = (Element.Source as XFToolKit.Presentation.Sources.Video.ResourceVideoSource).Path;

                if (!string.IsNullOrWhiteSpace(path))
                {
                    var filename = System.IO.Path.GetFileNameWithoutExtension(path).ToLowerInvariant();
                    var uri = "android.resource://" + package + "/raw/" + filename;

                    mediaSource = mediaSourceFactory.CreateMediaSource(Android.Net.Uri.Parse(uri));
                }
            }

            if (mediaSource != null)
            {
                (Control.Player as SimpleExoPlayer).Prepare(Element.Repeat ? new Com.Google.Android.Exoplayer2.Source.LoopingMediaSource(mediaSource) : mediaSource);

                if (Element.AutoPlay)
                {
                    Control.Player.PlayWhenReady = true;
                }
            }
        }

        private void OnPlayRequested(object sender, System.EventArgs args)
        {
            Control.Player.PlayWhenReady = true;

            SetSource();
        }

        private void OnStopRequested(object sender, System.EventArgs args)
        {
            try { Control.Player.Stop(); } catch { }
        }

        #region IPlayerEventListener implementation

        public void OnLoadingChanged(bool p0)
        {
            //throw new NotImplementedException();
        }

        public void OnPlaybackParametersChanged(PlaybackParameters p0)
        {
            //throw new NotImplementedException();
        }

        public void OnPlayerError(ExoPlaybackException p0)
        {
            Control.Player.Stop();
        }

        public void OnPlayerStateChanged(bool playWhenReady, int playbackState)
        {
            var videoStatus = XFToolKit.Presentation.Views.Rendered.VideoStatus.NotReady;

            switch (playbackState)
            {
                case Com.Google.Android.Exoplayer2.Player.StateEnded:
                    videoStatus = XFToolKit.Presentation.Views.Rendered.VideoStatus.Ended;

                    break;
                case Com.Google.Android.Exoplayer2.Player.StateIdle:
                    if (!Element.AutoPlay) break;

                    videoStatus = XFToolKit.Presentation.Views.Rendered.VideoStatus.Ended;

                    break;

                case Com.Google.Android.Exoplayer2.Player.StateReady:
                    videoStatus = XFToolKit.Presentation.Views.Rendered.VideoStatus.Playing;

                    buffered = true;

                    break;

                case Com.Google.Android.Exoplayer2.Player.StateBuffering:
                    if (buffered)
                    {
                        Control.Player.Stop();

                        break;
                    }

                    break;
            }

            (Element as XFToolKit.Presentation.Views.Rendered.IVideoPlayerController).Status = videoStatus;
        }

        public void OnPositionDiscontinuity()
        {
            //throw new NotImplementedException();
        }

        public void OnRepeatModeChanged(int p0)
        {
            //throw new NotImplementedException();
        }

        public void OnTimelineChanged(Timeline p0, Java.Lang.Object p1)
        {
            //throw new NotImplementedException();
        }

        public void OnTracksChanged(Com.Google.Android.Exoplayer2.Source.TrackGroupArray p0, Com.Google.Android.Exoplayer2.Trackselection.TrackSelectionArray p1)
        {
            //throw new NotImplementedException();
        }

        public void OnPositionDiscontinuity(int p0)
        {
            //throw new System.NotImplementedException();
        }

        public void OnSeekProcessed()
        {
            //throw new System.NotImplementedException();
        }

        public void OnShuffleModeEnabledChanged(bool p0)
        {
            //throw new System.NotImplementedException();
        }

        public void OnTimelineChanged(Timeline p0, Object p1, int p2)
        {
            //throw new System.NotImplementedException();
        }

        #endregion
    }
}