namespace SmartSolucionesCuba.Viderhe.Extend.OHCC.RailwayMuseum.Templating.Dashboard.Presentation.Providers
{
    public class PlacardThumbnailViewProvider : Common.Templating.Dashboard.Presentation.Providers.Facade.IPlacardThumbnailViewProviderFacade
    {
        public Xamarin.Forms.ContentView GetPlacardThumnailView(Support.Presentation.Models.View.PlacardViewModel model, Support.Presentation.Providers.Facade.ICloudServerProviderFacade cloudServerProvider)
        {
            if (model.Template.Id == Descriptors.CornerTileVideoPresentingVideoTemplateDescriptor.ID)
            {
                return new Views.CornerTileVideoPresentingVideoPlacardThumbnailView(model, cloudServerProvider);
            }

            if (model.Template.Id == Descriptors.InfinityVideoTemplateDescriptor.ID)
            {
                return new Views.InfinityVideoPlacardThumbnailView(model, cloudServerProvider);
            }

            return null;
        }
    }
}
