namespace SmartSolucionesCuba.Viderhe.Extend.SocialWelfare.Templating.Presentation.Converters
{
    public class FloatToRoundedFloatConverter : Xamarin.Forms.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return value;

            var target = (float)value;

            return decimal.Round(System.Convert.ToDecimal(target), 0);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
