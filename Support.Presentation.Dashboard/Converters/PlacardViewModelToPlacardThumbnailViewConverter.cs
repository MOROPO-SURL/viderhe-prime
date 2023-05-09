namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Dashboard.Converters
{
    public class PlacardViewModelToPlacardThumbnailViewConverter : Xamarin.Forms.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return value;

            return Xamarin.Forms.DependencyService.Get<Helpers.Facade.IPlacardThumbnailViewProvisioningHelperFacade>().GetPlacardThumnailView(value as Presentation.Models.View.PlacardViewModel);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
