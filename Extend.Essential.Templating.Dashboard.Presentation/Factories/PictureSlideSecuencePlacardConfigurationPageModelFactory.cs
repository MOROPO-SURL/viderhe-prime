using SmartSolucionesCuba.Viderhe.Extend.Common.Templating.Dashboard.Presentation.Extensions;

namespace SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Dashboard.Presentation.Factories
{
    public static class PictureSlideSecuencePlacardConfigurationPageModelFactory
    {
        public static Models.Page.PictureSlideSecuencePlacardConfigurationPageModel Build(Support.Presentation.Models.View.PlacardViewModel placardViewModel)
        {
            if (placardViewModel == null) return new Models.Page.PictureSlideSecuencePlacardConfigurationPageModel();

            var context = Newtonsoft.Json.JsonConvert.DeserializeObject<Templating.Models.Context.PictureSlideSecuencePlacardContextModel>(placardViewModel.SerializedContext);

            var result = new Models.Page.PictureSlideSecuencePlacardConfigurationPageModel
            {
                RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                DefineBeginAt = placardViewModel.BeginAt != null,
                BeginAt = placardViewModel.BeginAt ?? System.DateTime.Now.Date,
                DefineEndAt = placardViewModel.EndAt != null,
                EndAt = placardViewModel.EndAt ?? System.DateTime.Now.Date,
                PlayerOrientationSupportKind = placardViewModel.PlayerOrientationSupportKind == null ? Common.Templating.Dashboard.Presentation.Enumerations.ConfigurationPlayerOrientationSupportKind.Inherited : placardViewModel.PlayerOrientationSupportKind.Value.ToConfiguration(),
                Label = placardViewModel.Label,
                PicturePresentationSecounds = context.PicturePresentationSecounds
            };

            foreach (var item in context.Slides)
            {
                result.Slides.Add(new Models.View.PictureSlideViewModel
                {
                    Image = new Support.Presentation.Models.View.ImageViewModel { Id = item.Image.Id, ThumbnailId = item.Image.ThumbnailId },
                    LayoutMode = item.LayoutMode
                });
            }

            return result;
        }
    }
}
