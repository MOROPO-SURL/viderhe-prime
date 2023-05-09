using SmartSolucionesCuba.Viderhe.Extend.Common.Templating.Dashboard.Presentation.Extensions;
using SmartSolucionesCuba.Viderhe.Extend.Common.Templating.Presentation.Extensions;

namespace SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Dashboard.Presentation.Factories
{
    public static class SimplePicturePlacardConfigurationPageModelFactory
    {
        public static Models.Page.SimplePicturePlacardConfigurationPageModel Build(Support.Presentation.Models.View.PlacardViewModel placardViewModel)
        {
            if (placardViewModel == null) return new Models.Page.SimplePicturePlacardConfigurationPageModel();

            var context = Newtonsoft.Json.JsonConvert.DeserializeObject<Templating.Models.Context.SimplePicturePlacardContextModel>(placardViewModel.SerializedContext);

            return new Models.Page.SimplePicturePlacardConfigurationPageModel
            {
                RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                DefineBeginAt = placardViewModel.BeginAt != null,
                BeginAt = placardViewModel.BeginAt ?? System.DateTime.Now.Date,
                DefineEndAt = placardViewModel.EndAt != null,
                EndAt = placardViewModel.EndAt ?? System.DateTime.Now.Date,
                PlayerOrientationSupportKind = placardViewModel.PlayerOrientationSupportKind == null ? Common.Templating.Dashboard.Presentation.Enumerations.ConfigurationPlayerOrientationSupportKind.Inherited : placardViewModel.PlayerOrientationSupportKind.Value.ToConfiguration(),
                Label = placardViewModel.Label,
                ScreenPresentationTimeSecounds = System.Convert.ToInt32(context.ScreenPresentationTime.TotalSeconds),
                LayoutMode = context.LayoutMode,
                IsZoomingEnabled = context.IsZoomingEnabled,
                Image = context.Image.ToViewModel()
            };
        }
    }
}
