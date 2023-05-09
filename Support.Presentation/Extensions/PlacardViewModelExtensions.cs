namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Extensions
{
    public static class PlacardViewModelExtensions
    {
        public static bool SupportedByTargetOrientation(this Models.View.PlacardViewModel target, System.Func<Xamarin.Essentials.DisplayOrientation> getOrientationFunction)
        {
            if (target == null) return false;

            if (target.PlayerOrientationSupportKind != null) return IsDisplayOrientationCompatible(target.PlayerOrientationSupportKind.Value, getOrientationFunction);

            if (target.Template == null) return false;

            return IsDisplayOrientationCompatible(target.Template.PlayerOrientationSupport, getOrientationFunction);
        }

        private static bool IsDisplayOrientationCompatible(Viderhe.Extend.Common.Templating.Enumerations.PlayerOrientationSupportKind supportedOrientationKind, System.Func<Xamarin.Essentials.DisplayOrientation> getOrientationFunction)
        {
            var orientation = getOrientationFunction.Invoke();

            switch (supportedOrientationKind)
            {
                case Viderhe.Extend.Common.Templating.Enumerations.PlayerOrientationSupportKind.Portrait:
                    return orientation == Xamarin.Essentials.DisplayOrientation.Portrait;

                case Viderhe.Extend.Common.Templating.Enumerations.PlayerOrientationSupportKind.Landscape:
                    return orientation != Xamarin.Essentials.DisplayOrientation.Portrait;

                case Viderhe.Extend.Common.Templating.Enumerations.PlayerOrientationSupportKind.Any:
                default:
                    return true;
            }
        }
    }
}
