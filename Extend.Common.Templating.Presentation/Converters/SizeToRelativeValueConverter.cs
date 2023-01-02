namespace SmartSolucionesCuba.Viderhe.Extend.Common.Templating.Presentation.Converters
{
    public class SizeToRelativeValueConverter : Xamarin.Forms.IValueConverter
    {
        private const double RELATIVE_REFERENCE_SIZE = 1920;

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var concreteValue = System.Convert.ToDouble(value);

            if (concreteValue == -1) return concreteValue;

            var applicationPageLargeSize = System.Math.Max(Xamarin.Forms.Application.Current.MainPage.Width, Xamarin.Forms.Application.Current.MainPage.Height);

            if (applicationPageLargeSize == RELATIVE_REFERENCE_SIZE || applicationPageLargeSize <= 0) return concreteValue;

            return concreteValue * applicationPageLargeSize / RELATIVE_REFERENCE_SIZE;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw null;
        }
    }
}
