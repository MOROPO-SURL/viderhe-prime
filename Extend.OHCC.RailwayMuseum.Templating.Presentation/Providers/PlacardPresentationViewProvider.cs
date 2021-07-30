using SmartSolucionesCuba.Viderhe.Extend.OHCC.RailwayMuseum.Templating.Descriptors;
using SmartSolucionesCuba.Viderhe.Extend.OHCC.RailwayMuseum.Templating.Presentation.Models.Context;
using SmartSolucionesCuba.Viderhe.Extend.OHCC.RailwayMuseum.Templating.Presentation.Views;

namespace SmartSolucionesCuba.Viderhe.Extend.OHCC.RailwayMuseum.Templating.Presentation.Providers
{
    public class PlacardPresentationViewProvider : Common.Templating.Presentation.Providers.Facade.IPlacardPresentationViewProviderFacade
    {
        public Common.Presentation.Views.BasePlacardPresentationView GetFor(Support.Presentation.Models.View.PlacardViewModel model, Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider, Support.Presentation.Providers.Facade.ICloudServerProviderFacade cloudServerProvider)
        {
            if (model.Template.Id == InfinityVideoTemplateDescriptor.ID)
            {
                var context = Newtonsoft.Json.JsonConvert.DeserializeObject<InfinityVideoPlacardContextModel>(model.SerializedContext);

                return new InfinityVideoPlacardView(new Models.View.InfinityVideoPlacardViewModel
                {
                    RequireUnobstructedPresetation = false,
                    VideoSource = XFToolKit.Presentation.Sources.Video.VideoSource.FromUri(Support.Presentation.Factories.VideoUriFactory.BuildVideoUri(cloudServerProvider, context.VideoId, context.VideoExtension).AbsoluteUri),
                    VideoThumbnailImageSource = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ThumbnailUriFactory.BuildThumbnailUri(cloudServerProvider, context.ThumbnailId))
                });
            }

            if (model.Template.Id == CornerTileVideoPresentingVideoTemplateDescriptor.ID)
            {
                var context = Newtonsoft.Json.JsonConvert.DeserializeObject<CornerTileVideoPresentingVideoPlacardContextModel>(model.SerializedContext);

                return new CornerTileVideoPresentingVideoPlacardView(new Models.View.CornerTileVideoPresentingVideoPlacardViewModel
                {
                    RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                    PresenterTileVideoSource = XFToolKit.Presentation.Sources.Video.VideoSource.FromUri(Support.Presentation.Factories.VideoUriFactory.BuildVideoUri(cloudServerProvider, context.PresenterTileVideo.Id, context.PresenterTileVideo.Extension).AbsoluteUri),
                    PresenterTileVideoPlayDelay = context.PresenterTileVideoPlayDelay,
                    PresenterTileVideoScreenCorner = context.PresenterTileVideoScreenCorner,
                    PresentedVideoSource = XFToolKit.Presentation.Sources.Video.VideoSource.FromUri(Support.Presentation.Factories.VideoUriFactory.BuildVideoUri(cloudServerProvider, context.PresentedVideo.Id, context.PresentedVideo.Extension).AbsoluteUri)
                });
            }

            return null;
        }
    }
}
