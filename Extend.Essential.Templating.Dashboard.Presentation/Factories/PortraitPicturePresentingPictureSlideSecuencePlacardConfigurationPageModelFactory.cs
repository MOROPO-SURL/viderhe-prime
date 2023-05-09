using SmartSolucionesCuba.Viderhe.Extend.Common.Templating.Presentation.Extensions;

namespace SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Dashboard.Presentation.Factories
{
    public class PortraitPicturePresentingPictureSlideSecuencePlacardConfigurationPageModelFactory
    {
        public static Models.Page.PortraitPicturePresentingPictureSlideSecuencePlacardConfigurationPageModel Build(Support.Presentation.Models.View.PlacardViewModel placardViewModel)
        {
            if (placardViewModel == null) return new Models.Page.PortraitPicturePresentingPictureSlideSecuencePlacardConfigurationPageModel();

            var context = Newtonsoft.Json.JsonConvert.DeserializeObject<Templating.Models.Context.PortraitPicturePresentingPictureSlideSecuencePlacardContextModel>(placardViewModel.SerializedContext);

            var result = new Models.Page.PortraitPicturePresentingPictureSlideSecuencePlacardConfigurationPageModel
            {
                RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                DefineBeginAt = placardViewModel.BeginAt != null,
                BeginAt = placardViewModel.BeginAt ?? System.DateTime.Now.Date,
                DefineEndAt = placardViewModel.EndAt != null,
                EndAt = placardViewModel.EndAt ?? System.DateTime.Now.Date,
                Label = placardViewModel.Label,
                PresenterImage = context.PresenterImage.ToViewModel(),
                PresenterSide = context.PresenterSide,
                PictureSlidePresentationSecounds = System.Convert.ToInt32(context.PresentedPictureSlideSecuence.PicturePresentationSecounds),
                PresentedPictureSlideSecuenceCenteredAtPresentation = context.PresentedPictureSlideSecuenceCenteredAtPresentation
            };

            foreach (var item in context.PresentedPictureSlideSecuence.Slides)
            {
                result.PresentedSlides.Add(new Models.View.PictureSlideViewModel
                {
                    Image = new Support.Presentation.Models.View.ImageViewModel { Id = item.Image.Id, ThumbnailId = item.Image.ThumbnailId },
                    LayoutMode = item.LayoutMode
                });
            }

            return result;
        }
    }
}
