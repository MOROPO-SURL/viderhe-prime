namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Dashboard.Converters
{
    public class PlacardToIsAnyPlayerOrientationSupportedConverter : Xamarin.Forms.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return false;

            var target = value as Presentation.Models.View.PlacardViewModel;

            if (target.Template == null) return false;

            if (target.PlayerOrientationSupportKind != null)
            {
                return target.PlayerOrientationSupportKind == Viderhe.Extend.Common.Templating.Enumerations.PlayerOrientationSupportKind.Any;
            }

            return target.Template.PlayerOrientationSupport == Viderhe.Extend.Common.Templating.Enumerations.PlayerOrientationSupportKind.Any;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
