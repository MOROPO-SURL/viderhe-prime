namespace SmartSolucionesCuba.Viderhe.Extend.OHCC.RailwayMuseum.Templating.Presentation.Converters
{
    public class ScreenCornerToHorizontalLayoutOptionConverter : Xamarin.Forms.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var corner = (Enumerations.ScreenCorner) value;

            switch (corner)
            {
                case Enumerations.ScreenCorner.TopLeft:
                case Enumerations.ScreenCorner.BottomLeft:
                    return Xamarin.Forms.LayoutOptions.Start;

                case Enumerations.ScreenCorner.TopRight:
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
