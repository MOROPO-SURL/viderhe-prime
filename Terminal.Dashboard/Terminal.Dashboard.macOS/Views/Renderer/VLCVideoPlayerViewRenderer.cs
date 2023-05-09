using Xamarin.Forms;

[assembly: Xamarin.Forms.ExportRenderer(typeof(SmartSolucionesCuba.XFToolKit.Presentation.Views.Rendered.VideoPlayerView), typeof(SmartSolucionesCuba.Viderhe.Terminal.Dashboard.macOS.Views.Renderer.VLCVideoPlayerViewRenderer))]

namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.macOS.Views.Renderer
{
    public class VLCVideoPlayerViewRenderer : Xamarin.Forms.Platform.MacOS.ViewRenderer<XFToolKit.Presentation.Views.Rendered.VideoPlayerView, LibVLCSharp.Platforms.Mac.VideoView>
    {
        private bool disposed;

        private LibVLCSharp.Shared.LibVLC libVLC;

        public VLCVideoPlayerViewRenderer()
        {
            disposed = false;
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.MacOS.ElementChangedEventArgs<XFToolKit.Presentation.Views.Rendered.VideoPlayerView> eventArgs)
        {
            base.OnElementChanged(eventArgs);

            if (eventArgs.NewElement != null)
            {
                if (Control == null)
                {
                    libVLC = new LibVLCSharp.Shared.LibVLC(false);

                    SetNativeControl(new LibVLCSharp.Platforms.Mac.VideoView { MediaPlayer = new LibVLCSharp.Shared.MediaPlayer(libVLC) });

                    Control.MediaPlayer.EndReached += MediaPlayer_EndReached;
                    Control.MediaPlayer.EncounteredError += MediaPlayer_EncounteredError;
                    Control.MediaPlayer.Playing += MediaPlayer_Playing;
                    Control.MediaPlayer.Stopped += MediaPlayer_Stopped;

                    Element.PlayRequested += Play_Requested;
                    Element.StopRequested += Stop_Requested;

                    SetSource();

                    SetIsMuted();

                    if (Element.AutoPlay)
                    {
                        Control.MediaPlayer.Play();
                    }
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null || disposed || Element == null || (Element.AutoPlay && (Element as XFToolKit.Presentation.Views.Rendered.IVideoPlayerController).Status == XFToolKit.Presentation.Views.Rendered.VideoStatus.Ended)) return;

            if (e.PropertyName == XFToolKit.Presentation.Views.Rendered.VideoPlayerView.SourceProperty.PropertyName)
            {
                SetSource();

                if (Element.AutoPlay)
                {
                    Control.MediaPlayer.Play();
                }
            }
            else if (e.PropertyName == XFToolKit.Presentation.Views.Rendered.VideoPlayerView.IsMutedProperty.PropertyName)
            {
                SetIsMuted();
            }
        }

        protected override void Dispose(bool disposing)
        {
            Element.PlayRequested -= Play_Requested;
            Element.StopRequested -= Stop_Requested;

            Control.MediaPlayer.EndReached -= MediaPlayer_EndReached;
            Control.MediaPlayer.EncounteredError -= MediaPlayer_EncounteredError;
            Control.MediaPlayer.Playing -= MediaPlayer_Playing;
            Control.MediaPlayer.Stopped -= MediaPlayer_Stopped;

            base.Dispose(disposing);

            libVLC.Dispose();

            libVLC = null;

            disposed = true;
        }

        private void MediaPlayer_Stopped(object sender, System.EventArgs e)
        {
            (Element as XFToolKit.Presentation.Views.Rendered.IVideoPlayerController).Status = XFToolKit.Presentation.Views.Rendered.VideoStatus.Ended;

            if (Element.Repeat)
            {
                Control.MediaPlayer.Play();
            }
        }

        private void MediaPlayer_Playing(object sender, System.EventArgs e)
        {
            (Element as XFToolKit.Presentation.Views.Rendered.IVideoPlayerController).Status = XFToolKit.Presentation.Views.Rendered.VideoStatus.Playing;
        }

        private void MediaPlayer_EncounteredError(object sender, System.EventArgs e)
        {
            (Element as XFToolKit.Presentation.Views.Rendered.IVideoPlayerController).Status = XFToolKit.Presentation.Views.Rendered.VideoStatus.Ended;

            if (Element.Repeat)
            {
                Control.MediaPlayer.Play();
            }
        }

        private void MediaPlayer_EndReached(object sender, System.EventArgs e)
        {
            (Element as XFToolKit.Presentation.Views.Rendered.IVideoPlayerController).Status = XFToolKit.Presentation.Views.Rendered.VideoStatus.Ended;

            if (Element.Repeat)
            {
                Control.MediaPlayer.Play();
            }
        }

        private void Stop_Requested(object sender, System.EventArgs eventArgs)
        {
            Control.MediaPlayer.Stop();
        }

        private void Play_Requested(object sender, System.EventArgs eventArgs)
        {
            (Element as XFToolKit.Presentation.Views.Rendered.IVideoPlayerController).Status = XFToolKit.Presentation.Views.Rendered.VideoStatus.NotReady;

            Control.MediaPlayer.Play();
        }

        private void SetIsMuted()
        {
            Control.MediaPlayer.Mute = Element.IsMuted;
        }

        private void SetSource()
        {
            if (Control == null || disposed) return;

            var hasSetSource = false;

            try
            {
                if (Element.Source is XFToolKit.Presentation.Sources.Video.UriVideoSource)
                {
                    var uri = (Element.Source as XFToolKit.Presentation.Sources.Video.UriVideoSource).Uri;

                    if (!string.IsNullOrWhiteSpace(uri))
                    {
                        Control.MediaPlayer.Media = new LibVLCSharp.Shared.Media(libVLC, new System.Uri(uri));

                        hasSetSource = true;
                    }
                }
                else if (Element.Source is XFToolKit.Presentation.Sources.Video.FileVideoSource)
                {
                    var file = (Element.Source as XFToolKit.Presentation.Sources.Video.FileVideoSource).File;

                    if (!string.IsNullOrWhiteSpace(file))
                    {
                        Control.MediaPlayer.Media = new LibVLCSharp.Shared.Media(libVLC, new System.Uri(file));

                        hasSetSource = true;
                    }
                }
                else if (Element.Source is XFToolKit.Presentation.Sources.Video.ResourceVideoSource)
                {
                    var path = (Element.Source as XFToolKit.Presentation.Sources.Video.ResourceVideoSource).Path;

                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        Control.MediaPlayer.Media = new LibVLCSharp.Shared.Media(libVLC, new System.Uri(path));

                        hasSetSource = true;
                    }
                }
            }
            catch
            {
            }

            if (!hasSetSource)
            {
                (Element as XFToolKit.Presentation.Views.Rendered.IVideoPlayerController).Status = XFToolKit.Presentation.Views.Rendered.VideoStatus.Ended;
            }
        }
    }
}

