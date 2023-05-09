using SmartSolucionesCuba.Viderhe.Extend.Common.Templating.Presentation.Extensions;

namespace SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Dashboard.Presentation.Factories
{
    public class LandscapePicturePresentingVideoPlacardConfigurationPageModelFactory
    {
        public static Models.Page.LandscapePicturePresentingVideoPlacardConfigurationPageModel Build(Support.Presentation.Models.View.PlacardViewModel placardViewModel)
        {
            if (placardViewModel == null) return new Models.Page.LandscapePicturePresentingVideoPlacardConfigurationPageModel();

            var context = Newtonsoft.Json.JsonConvert.DeserializeObject<Templating.Models.Context.LandscapePicturePresentingVideoPlacardContextModel>(placardViewModel.SerializedContext);

            return new Models.Page.LandscapePicturePresentingVideoPlacardConfigurationPageModel
            {
                RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                DefineBeginAt = placardViewModel.BeginAt != null,
                BeginAt = placardViewModel.BeginAt ?? System.DateTime.Now.Date,
                DefineEndAt = placardViewModel.EndAt != null,
                EndAt = placardViewModel.EndAt ?? System.DateTime.Now.Date,
                Label = placardViewModel.Label,
                PresenterImage = context.PresenterImage.ToViewModel(),
                PresenterSide = context.PresenterSide,
                PresentedVideo = context.PresentedVideo.ToViewModel(),
                PresentedVideoCenteredAtPresentation = context.PresentedVideoCenteredAtPresentation
            };
        }
    }
}
