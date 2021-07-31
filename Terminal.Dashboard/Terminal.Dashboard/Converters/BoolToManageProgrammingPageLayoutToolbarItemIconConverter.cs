namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Converters
{
    public class BoolToManageProgrammingPageLayoutToolbarItemIconConverter : Xamarin.Forms.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var expanded = (bool)value;

            Xamarin.Forms.FontImageSource result;

            if (expanded)
            {
                result = new Xamarin.Forms.FontImageSource
                {
                    FontFamily = "Material Design Icons",
                    Glyph = "\uf615"
                };
            }
            else
            {
                result = new Xamarin.Forms.FontImageSource
                {
                    FontFamily = "Material Design Icons",
                    Glyph = "\uf616"
                };
            }

            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.UWP)
            {
                result.Color = Xamarin.Forms.Color.Black;
            }

            return result;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
