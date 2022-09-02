namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Helpers
{
    public sealed class ImageUriFactoryHelper
    {
        private static ImageUriFactoryHelper instance;

        public static ImageUriFactoryHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ImageUriFactoryHelper();
                }

                return instance;
            }
        }

        private int? mainDisplayInvariantWidth;
        private int? mainDisplayInvariantHeight;

        private ImageUriFactoryHelper()
        {
        }

        public void Initialize()
        {
            var currentWith = (int)Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width;
            var currentHeight = (int)Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Height;

            mainDisplayInvariantWidth = System.Math.Max(currentWith, currentHeight);
            mainDisplayInvariantHeight = System.Math.Min(currentWith, currentHeight);
        }

        public string BuildImageUrl(Providers.Facade.ICloudServerProviderFacade cloudServerProvider, System.Guid imageId, bool original = false)
        {
            if (original) return string.Format("{0}://{1}/media/image/{2}", cloudServerProvider.UseSSL ? "https" : "http", cloudServerProvider.Host, imageId);

            if (mainDisplayInvariantWidth == null || mainDisplayInvariantHeight == null) Initialize();

            return string.Format("{0}://{1}/media/image/{2}/w/{3}/h/{4}", cloudServerProvider.UseSSL ? "https" : "http", cloudServerProvider.Host, imageId, mainDisplayInvariantWidth.Value, mainDisplayInvariantHeight.Value);
        }
    }
}
