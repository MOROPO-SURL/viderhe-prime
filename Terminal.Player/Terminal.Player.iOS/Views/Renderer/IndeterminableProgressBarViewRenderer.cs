using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.ExportRenderer(typeof(SmartSolucionesCuba.Viderhe.Terminal.Player.Views.Rendered.IndeterminableProgressBarView), typeof(SmartSolucionesCuba.Viderhe.Terminal.Player.iOS.Views.Renderer.IndeterminableProgressBarViewRenderer))]

namespace SmartSolucionesCuba.Viderhe.Terminal.Player.iOS.Views.Renderer
{
    public class IndeterminableProgressBarViewRenderer : ViewRenderer<Player.Views.Rendered.IndeterminableProgressBarView, LinearProgressBarView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Player.Views.Rendered.IndeterminableProgressBarView> args)
        {
            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                if (Control == null)
                {
                    SetNativeControl(new LinearProgressBarView());

                    BackgroundColorPropertyChanged_Callback();

                    ForegroundColorPropertyCahnged_Callback();

                    IndetermiatePropertyChanged_Callback();
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            if (args.PropertyName == Player.Views.Rendered.IndeterminableProgressBarView.IsIndeterminateProperty.PropertyName)
            {
                IndetermiatePropertyChanged_Callback();
            }
            else if (args.PropertyName == Xamarin.Forms.VisualElement.BackgroundColorProperty.PropertyName)
            {
                BackgroundColorPropertyChanged_Callback();
            }
            else if (args.PropertyName == Xamarin.Forms.ProgressBar.ProgressColorProperty.PropertyName)
            {
                ForegroundColorPropertyCahnged_Callback();
            }
        }

        private void IndetermiatePropertyChanged_Callback()
        {
            if (Element.IsIndeterminate)
            {
                Control.StartAnimation();
            }
            else
            {
                Control.StopAnimation();
            }
        }

        private void BackgroundColorPropertyChanged_Callback()
        {
            
        }

        private void ForegroundColorPropertyCahnged_Callback()
        {
            Control.ProgressColor = Element.ProgressColor.ToUIColor();
        }
    }
}