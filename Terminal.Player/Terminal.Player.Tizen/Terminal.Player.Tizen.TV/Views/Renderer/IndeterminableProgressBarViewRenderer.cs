[assembly: Xamarin.Forms.ExportRenderer(typeof(SmartSolucionesCuba.Viderhe.Terminal.Player.Views.Rendered.IndeterminableProgressBarView), typeof(SmartSolucionesCuba.Viderhe.Terminal.Player.Tizen.TV.Views.Renderer.IndeterminableProgressBarViewRenderer))]

namespace SmartSolucionesCuba.Viderhe.Terminal.Player.Tizen.TV.Views.Renderer
{
    public class IndeterminableProgressBarViewRenderer : Xamarin.Forms.Platform.Tizen.ProgressBarRenderer
    {
        protected override void OnElementChanged(Xamarin.Forms.Platform.Tizen.ElementChangedEventArgs<Xamarin.Forms.ProgressBar> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.IsPulseMode = true;

                Control.PlayPulse();
            }
        }

        //protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    base.OnElementPropertyChanged(sender, e);

        //    if (e.PropertyName == Player.Views.Rendered.IndeterminableProgressBarView.IsIndeterminateProperty.PropertyName)
        //    {
        //        Control.Style = "pending_list";
        //        Control.IsPulseMode = (Element as Player.Views.Rendered.IndeterminableProgressBarView).IsIndeterminate;
        //        Control.PlayPulse();
        //    }
        //}
    }
}
