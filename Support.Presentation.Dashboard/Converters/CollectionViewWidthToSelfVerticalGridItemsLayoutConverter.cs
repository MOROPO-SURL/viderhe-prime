namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Dashboard.Converters
{
    public class CollectionViewWidthToSelfVerticalGridItemsLayoutConverter : Xamarin.Forms.IValueConverter
    {
        public double ChildMinWidth { get; set; }

        public int ChildAbsoluteHorizontalMargin { get; set; }

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is double)) return new Xamarin.Forms.GridItemsLayout(1, Xamarin.Forms.ItemsLayoutOrientation.Vertical);

            var collectionViewWidth = (double)value;

            if (collectionViewWidth == -1) return new Xamarin.Forms.GridItemsLayout(1, Xamarin.Forms.ItemsLayoutOrientation.Vertical);

            var reazon = collectionViewWidth / (ChildMinWidth + ChildAbsoluteHorizontalMargin);

            int columnCount = System.Convert.ToInt32(System.Math.Round(reazon, System.MidpointRounding.ToEven));

            if (columnCount > reazon)
            {
                columnCount--;
            }

            if (columnCount <= 0)
            {
                columnCount = 1;
            }

            var spacing = Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android ? ChildAbsoluteHorizontalMargin : ChildAbsoluteHorizontalMargin / 2;

            return new Xamarin.Forms.GridItemsLayout(columnCount, Xamarin.Forms.ItemsLayoutOrientation.Vertical)
            {
                HorizontalItemSpacing = spacing,
                VerticalItemSpacing = spacing
            };
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
