namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Factories
{
    public static class ImageUriFactory
    {
        public static System.Uri BuildImageUri(System.Guid imageId)
        {
            return new System.Uri(Helpers.ImageUriFactoryHelper.Instance.BuildImageUrl(Xamarin.Forms.DependencyService.Get<Providers.Facade.ICloudServerProviderFacade>(), imageId));
        }

        public static System.Uri BuildImageUri(System.Guid imageId, bool original)
        {
            return new System.Uri(Helpers.ImageUriFactoryHelper.Instance.BuildImageUrl(Xamarin.Forms.DependencyService.Get<Providers.Facade.ICloudServerProviderFacade>(), imageId, original));
        }
    }
}
