namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Factories
{
    public static class ImageUriFactory
    {
        public static System.Uri BuildImageUri(Providers.Facade.ICloudServerProviderFacade cloudServerProvider, System.Guid imageId)
        {
            return new System.Uri(Xamarin.Forms.DependencyService.Get<Helpers.Facade.IImageUriFactoryHelper>().BuildImageUrl(cloudServerProvider, imageId));
        }
    }
}
