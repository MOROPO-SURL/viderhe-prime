namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Factories
{
    public static class ImageUriFactory
    {
        public static System.Uri BuildImageUri(System.Guid imageId, bool original = false)
        {
            return new System.Uri(Xamarin.Forms.DependencyService.Get<Helpers.Facade.IImageUriFactoryHelper>().BuildImageUrl(Xamarin.Forms.DependencyService.Get<Providers.Facade.ICloudServerProviderFacade>(), imageId, original));
        }
    }
}
