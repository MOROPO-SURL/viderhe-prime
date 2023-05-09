using SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Descriptors;
using SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Models.Context;
using SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Presentation.Extensions;
using SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Presentation.Views;

namespace SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Presentation.Providers
{
    public class PlacardPresentationViewProvider : Common.Templating.Presentation.Providers.Facade.IPlacardPresentationViewProviderFacade
    {
        public Common.Presentation.Views.BasePlacardPresentationView GetFor(Support.Presentation.Models.View.PlacardViewModel model, System.Func<Xamarin.Essentials.DisplayOrientation> getOrientationFunction)
        {
            if (model.Template.Id == PictureSlideSecuenceTemplateDescriptor.ID)
            {
                var context = Newtonsoft.Json.JsonConvert.DeserializeObject<PictureSlideSecuencePlacardContextModel>(model.SerializedContext);

                return new PictureSlideSecuencePlacardView(context);
            }

            if (model.Template.Id == SimpleVideoTemplateDescriptor.ID)
            {
                var context = Newtonsoft.Json.JsonConvert.DeserializeObject<SimpleVideoPlacardContextModel>(model.SerializedContext);

                return new SimpleVideoPlacardView(new Models.View.SimpleVideoPlacardViewModel
                {
                    RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                    VideoSource = XFToolKit.Presentation.Sources.Video.VideoSource.FromUri(Support.Presentation.Factories.VideoUriFactory.BuildVideoUri(context.Video.Id, context.Video.Extension).AbsoluteUri)
                });
            }

            if (model.Template.Id == SimplePictureTemplateDescriptor.ID)
            {
                var context = Newtonsoft.Json.JsonConvert.DeserializeObject<SimplePicturePlacardContextModel>(model.SerializedContext);

                if (context.LayoutMode == Enumerations.PictureLayoutMode.AspectFitWithOpaqueAspectFilledBySelf)
                {
                    return new SelfOpaqueFillSimplePicturePlacardView(new Models.View.SelfOpaqueFillSimplePicturePlacardViewModel
                    {
                        RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                        ScreenPresentationTime = context.ScreenPresentationTime,
                        Source = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(context.Image.Id)),
                        ThumnailSource = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ThumbnailUriFactory.BuildThumbnailUri(context.Image.ThumbnailId)),
                        Aspect = context.LayoutMode.ToXamarinFormsAspect(),
                        ZoomingEnabled = context.IsZoomingEnabled
                    });
                }
                else
                {
                    return new SimplePicturePlacardView(new Models.View.SimplePicturePlacardViewModel
                    {
                        RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                        ScreenPresentationTime = context.ScreenPresentationTime,
                        Source = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(context.Image.Id)),
                        Aspect = context.LayoutMode.ToXamarinFormsAspect(),
                        ZoomingEnabled = context.IsZoomingEnabled
                    });
                }
            }

            if (model.Template.Id == LandscapePicturePresentingPictureTemplateDescriptor.ID)
            {
                var context = Newtonsoft.Json.JsonConvert.DeserializeObject<LandscapePicturePresentingPicturePlacardContextModel>(model.SerializedContext);

                return new LandscapePicturePresentingPicturePlacardView(new Models.View.LandscapePicturePresentingPicturePlacardViewModel
                {
                    RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                    ScreenPresentationTime = context.ScreenPresentationTime,
                    PresenterSource = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(context.PresenterImage.Id)),
                    PresenterSide = context.PresenterSide,
                    PresenterImageWidth = context.PresenterImage.Width,
                    PresenterImageHeigth = context.PresenterImage.Height,
                    PresentedSource = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(context.PresentedImage.Id)),
                    PresentedImageCenteredAtPresentation = context.PresentedImageCenteredAtPresentation,
                    PresentedImageZoomingEnabled = context.PresentedImageZoomingEnabled
                });
            }

            if (model.Template.Id == LandscapePicturePresentingVideoTemplateDescriptor.ID)
            {
                var context = Newtonsoft.Json.JsonConvert.DeserializeObject<LandscapePicturePresentingVideoPlacardContextModel>(model.SerializedContext);

                return new LandscapePicturePresentingVideoPlacardView(new Models.View.LandscapePicturePresentingVideoPlacardViewModel
                {
                    RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                    PresenterSource = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(context.PresenterImage.Id)),
                    PresenterSide = context.PresenterSide,
                    PresenterImageWidth = context.PresenterImage.Width,
                    PresenterImageHeigth = context.PresenterImage.Height,
                    PresentedSource = XFToolKit.Presentation.Sources.Video.VideoSource.FromUri(Support.Presentation.Factories.VideoUriFactory.BuildVideoUri(context.PresentedVideo.Id, context.PresentedVideo.Extension).AbsoluteUri),
                    PresentedVideoCenteredAtPresentation = context.PresentedVideoCenteredAtPresentation
                });
            }

            if (model.Template.Id == PortraitPicturePresentingPictureTemplateDescriptor.ID)
            {
                var context = Newtonsoft.Json.JsonConvert.DeserializeObject<PortraitPicturePresentingPicturePlacardContextModel>(model.SerializedContext);

                return new PortraitPicturePresentingPicturePlacardView(new Models.View.PortraitPicturePresentingPicturePlacardViewModel
                {
                    RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                    ScreenPresentationTime = context.ScreenPresentationTime,
                    PresenterSource = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(context.PresenterImage.Id)),
                    PresenterSide = context.PresenterSide,
                    PresenterImageWidth = context.PresenterImage.Width,
                    PresenterImageHeigth = context.PresenterImage.Height,
                    PresentedSource = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(context.PresentedImage.Id)),
                    PresentedImageCenteredAtPresentation = context.PresentedImageCenteredAtPresentation,
                    PresentedImageZoomingEnabled = context.PresentedImageZoomingEnabled
                });
            }

            if (model.Template.Id == PortraitPicturePresentingVideoTemplateDescriptor.ID)
            {
                var context = Newtonsoft.Json.JsonConvert.DeserializeObject<PortraitPicturePresentingVideoPlacardContextModel>(model.SerializedContext);

                return new PortraitPicturePresentingVideoPlacardView(new Models.View.PortraitPicturePresentingVideoPlacardViewModel
                {
                    RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                    PresenterSource = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(context.PresenterImage.Id)),
                    PresenterSide = context.PresenterSide,
                    PresenterImageWidth = context.PresenterImage.Width,
                    PresenterImageHeigth = context.PresenterImage.Height,
                    PresentedSource = XFToolKit.Presentation.Sources.Video.VideoSource.FromUri(Support.Presentation.Factories.VideoUriFactory.BuildVideoUri(context.PresentedVideo.Id, context.PresentedVideo.Extension).AbsoluteUri),
                    PresentedVideoCenteredAtPresentation = context.PresentedVideoCenteredAtPresentation
                });
            }

            if (model.Template.Id == LandscapePicturePresentingPictureSlideSecuenceTemplateDescriptor.ID)
            {
                var context = Newtonsoft.Json.JsonConvert.DeserializeObject<LandscapePicturePresentingPictureSlideSecuencePlacardContextModel>(model.SerializedContext);

                return new LandscapePicturePresentingPictureSlideSecuencePlacardView(new Models.View.LandscapePicturePresentingPictureSlideSecuencePlacardViewModel
                {
                    RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                    PresenterSource = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(context.PresenterImage.Id)),
                    PresenterSide = context.PresenterSide,
                    PresenterImageWidth = context.PresenterImage.Width,
                    PresenterImageHeigth = context.PresenterImage.Height,
                    PresentedPictureSlideSecuence = context.PresentedPictureSlideSecuence,
                    PresentedPictureSlideSecuenceCenteredAtPresentation = context.PresentedPictureSlideSecuenceCenteredAtPresentation
                });
            }

            if (model.Template.Id == PortraitPicturePresentingPictureSlideSecuenceTemplateDescriptor.ID)
            {
                var context = Newtonsoft.Json.JsonConvert.DeserializeObject<PortraitPicturePresentingPictureSlideSecuencePlacardContextModel>(model.SerializedContext);

                return new PortraitPicturePresentingPictureSlideSecuencePlacardView(new Models.View.PortraitPicturePresentingPictureSlideSecuencePlacardViewModel
                {
                    RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                    PresenterSource = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(context.PresenterImage.Id)),
                    PresenterSide = context.PresenterSide,
                    PresenterImageWidth = context.PresenterImage.Width,
                    PresenterImageHeigth = context.PresenterImage.Height,
                    PresentedPictureSlideSecuence = context.PresentedPictureSlideSecuence,
                    PresentedPictureSlideSecuenceCenteredAtPresentation = context.PresentedPictureSlideSecuenceCenteredAtPresentation
                });
            }

            return null;
        }
    }
}
