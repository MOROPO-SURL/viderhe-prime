using SmartSolucionesCuba.Viderhe.Extend.Common.Templating.Presentation.Extensions;

namespace SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Dashboard.Presentation.Factories
{
    public static class LandscapePicturePresentingPicturePlacardConfigurationPageModelFactory
    {
        public static Models.Page.LandscapePicturePresentingPicturePlacardConfigurationPageModel Build(Support.Presentation.Models.View.PlacardViewModel placardViewModel)
        {
            if (placardViewModel == null) return new Models.Page.LandscapePicturePresentingPicturePlacardConfigurationPageModel();

            var context = Newtonsoft.Json.JsonConvert.DeserializeObject<Templating.Models.Context.LandscapePicturePresentingPicturePlacardContextModel>(placardViewModel.SerializedContext);

            return new Models.Page.LandscapePicturePresentingPicturePlacardConfigurationPageModel
            {
                RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                DefineBeginAt = placardViewModel.BeginAt != null,
                BeginAt = placardViewModel.BeginAt ?? System.DateTime.Now.Date,
                DefineEndAt = placardViewModel.EndAt != null,
                EndAt = placardViewModel.EndAt ?? System.DateTime.Now.Date,
                Label = placardViewModel.Label,
                ScreenPresentationTimeSecounds = System.Convert.ToInt32(context.ScreenPresentationTime.TotalSeconds),
                PresenterImage = context.PresenterImage.ToViewModel(),
                PresenterSide = context.PresenterSide,
                PresentedImage = context.PresentedImage.ToViewModel(),
                PresentedImageCenteredAtPresentation = context.PresentedImageCenteredAtPresentation,
                PresentedImageZoomingEnabled = context.PresentedImageZoomingEnabled
            };
        }
    }
}
