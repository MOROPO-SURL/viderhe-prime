namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Helpers.Facade
{
    public interface IImageUriFactoryHelper
    {
        string BuildImageUrl(Providers.Facade.ICloudServerProviderFacade cloudServerProvider, System.Guid imageId, bool original = false);
    }
}
