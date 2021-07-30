namespace SmartSolucionesCuba.Viderhe.Extend.OHCC.RailwayMuseum.Templating.Presentation.Converters
{
    public class ScreenCornerToVerticalLayoutOptionConverter : Xamarin.Forms.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var corner = (Enumerations.ScreenCorner)value;

            switch (corner)
            {
                case Enumerations.ScreenCorner.TopLeft:
                case Enumerations.ScreenCorner.TopRight:
                    return Xamarin.Forms.LayoutOptions.Start;

                case Enumerations.ScreenCorner.BottomLeft:
                case Enumerations.ScreenCorner.BottonRight:
                default:
                    return Xamarin.Forms.LayoutOptions.End;
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
