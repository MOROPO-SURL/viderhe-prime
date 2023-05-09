namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Dashboard.Converters
{
    public class AllTrueMultiConverter : Xamarin.Forms.IMultiValueConverter
    {
        public object Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null || !targetType.IsAssignableFrom(typeof(bool)))
            {
                return false;
            }

            foreach (var value in values)
            {
                if (!(value is bool b))
                { return false; }
                else
                { if (!b) return false; }
            }

            return true;
        }

        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
