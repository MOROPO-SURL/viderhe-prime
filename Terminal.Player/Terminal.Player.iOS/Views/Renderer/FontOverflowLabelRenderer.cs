[assembly: Xamarin.Forms.ExportRenderer(typeof(SmartSolucionesCuba.Viderhe.Extend.Common.Templating.Presentation.Views.Rendered.FontOverflowLabel), typeof(SmartSolucionesCuba.Viderhe.Terminal.Player.iOS.Views.Renderer.FontOverflowLabelRenderer))]

namespace SmartSolucionesCuba.Viderhe.Terminal.Player.iOS.Views.Renderer
{
    public class FontOverflowLabelRenderer : Xamarin.Forms.Platform.iOS.LabelRenderer
    {
        protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<Xamarin.Forms.Label> e)
        {
            base.OnElementChanged(e);

            if (Control == null) return;

            Control.AdjustsFontForContentSizeCategory = true;
            Control.BaselineAdjustment = UIKit.UIBaselineAdjustment.None;
        }
    }
}
