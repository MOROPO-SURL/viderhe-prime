using UIKit;
using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(SmartSolucionesCuba.Viderhe.Terminal.Player.iOS.Helpers.TextWidthCalculationHelper))]

namespace SmartSolucionesCuba.Viderhe.Terminal.Player.iOS.Helpers
{
    public class TextWidthCalculationHelper : Extend.Common.Overlaying.Presentation.Helpers.Facade.ITextWidthCalculationHelperFacade
    {
        public double CalculateTextWidth(string text, Xamarin.Forms.Font font)
        {
            var label = new UILabel();

            label.Text = text;
            label.Font = font.ToUIFont();

            var measureSize = label.Text.StringSize(label.Font);

            return measureSize.Width;
        }
    }
}
