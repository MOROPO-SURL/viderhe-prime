namespace SmartSolucionesCuba.Viderhe.Terminal.Player.Helpers
{
    public class ImageUriFactoryHelper : Support.Presentation.Helpers.Facade.IImageUriFactoryHelper
    {
        public string BuildImageUrl(Support.Presentation.Providers.Facade.ICloudServerProviderFacade cloudServerProvider, System.Guid imageId)
        {
            return string.Format("http://{0}/media/image/{1}/w/{2}/h/{3}", cloudServerProvider.CloudServerHost, imageId, (int)Xamarin.Forms.Application.Current.MainPage.Width, (int)Xamarin.Forms.Application.Current.MainPage.Height);
        }
    }
}
