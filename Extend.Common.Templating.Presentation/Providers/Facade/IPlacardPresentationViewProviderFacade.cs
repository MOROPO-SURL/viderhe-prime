namespace SmartSolucionesCuba.Viderhe.Extend.Common.Templating.Presentation.Providers.Facade
{
    public interface IPlacardPresentationViewProviderFacade
    {
        Common.Presentation.Views.BasePlacardPresentationView GetFor(Support.Presentation.Models.View.PlacardViewModel model, System.Func<Xamarin.Essentials.DisplayOrientation> getOrientationFunction);
    }
}
