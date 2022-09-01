namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Helpers
{
    public class DefaultImageUriFactoryHelper : Facade.IImageUriFactoryHelper
    {
        public string BuildImageUrl(Providers.Facade.ICloudServerProviderFacade cloudServerProvider, System.Guid imageId, bool original = false)
        {
            if (original) return string.Format("{0}://{1}/media/image/{2}", cloudServerProvider.UseSSL ? "https" : "http", cloudServerProvider.Host, imageId);

            var currentWith = (int)Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width;
            var currentHeight = (int)Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Height;

            var designedWidth = System.Math.Max(currentWith, currentHeight);
            var designedHeight = System.Math.Min(currentWith, currentHeight);

            return string.Format("{0}://{1}/media/image/{2}/w/{3}/h/{4}", cloudServerProvider.UseSSL ? "https" : "http", cloudServerProvider.Host, imageId, designedWidth, designedHeight);
        }
    }
}
