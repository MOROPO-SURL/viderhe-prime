namespace SmartSolucionesCuba.Viderhe.Extend.Common.Templating.Presentation.Converters
{
    public class SizeToRelativeValueConverter : Xamarin.Forms.IValueConverter
    {
        private const double RELATIVE_REFERENCE_SIZE = 1920;

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var concreteValue = System.Convert.ToDouble(value);

            if (concreteValue == -1) return concreteValue;

            var applicationPageWidth = Xamarin.Forms.Application.Current.MainPage.Width;

            if (applicationPageWidth == RELATIVE_REFERENCE_SIZE) return concreteValue;

            return concreteValue * Xamarin.Forms.Application.Current.MainPage.Width / RELATIVE_REFERENCE_SIZE;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw null;
        }
    }
}
