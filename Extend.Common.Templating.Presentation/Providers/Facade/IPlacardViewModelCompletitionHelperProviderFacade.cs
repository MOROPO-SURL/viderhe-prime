namespace SmartSolucionesCuba.Viderhe.Extend.Common.Templating.Presentation.Providers.Facade
{
    public interface IPlacardViewModelCompletitionHelperProviderFacade
    {
        Helpers.Facade.IPlacardViewModelCompletitionHelperFacade GetFor(Cloud.Api.Common.Models.PlacardDisplayModel model);
    }
}
