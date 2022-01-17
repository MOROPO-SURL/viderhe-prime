namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Helpers
{
    public class ImageUriFactoryHelper : Support.Presentation.Helpers.Facade.IImageUriFactoryHelper
    {
        public string BuildImageUrl(Support.Presentation.Providers.Facade.ICloudServerProviderFacade cloudServerProvider, System.Guid imageId)
        {
            return string.Format("http://{0}/media/default/image/{1}", cloudServerProvider.CloudServerHost, imageId);
        }
    }
}
