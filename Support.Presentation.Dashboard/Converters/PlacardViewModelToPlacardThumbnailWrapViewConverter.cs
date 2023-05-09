namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Dashboard.Converters
{
    public class PlacardViewModelToPlacardThumbnailWrapViewConverter : Xamarin.Forms.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return value;

            var model = value as Presentation.Models.View.PlacardViewModel;

            if (model.Template == null)
            {
                return new Views.UnsupportedPlacardThumbnailView();
            }

            if (model.PlayerOrientationSupportKind != null)
            {
                if (model.PlayerOrientationSupportKind != Viderhe.Extend.Common.Templating.Enumerations.PlayerOrientationSupportKind.Portrait)
                {
                    return new Views.LandscapePlacardThumbnailWrapView(model);
                }
                else
                {
                    return new Views.PortraitPlacardThumbnailWrapView(model);
                }
            }
            else if (model.Template.PlayerOrientationSupport != Viderhe.Extend.Common.Templating.Enumerations.PlayerOrientationSupportKind.Portrait)
            {
                return new Views.LandscapePlacardThumbnailWrapView(model);
            }
            else
            {
                return new Views.PortraitPlacardThumbnailWrapView(model);
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
