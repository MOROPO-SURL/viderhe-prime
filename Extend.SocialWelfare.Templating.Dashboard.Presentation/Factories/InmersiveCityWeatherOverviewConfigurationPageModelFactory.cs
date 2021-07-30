using SmartSolucionesCuba.Viderhe.Extend.Common.Templating.Presentation.Extensions;

namespace SmartSolucionesCuba.Viderhe.Extend.SocialWelfare.Templating.Dashboard.Presentation.Factories
{
    public static class InmersiveCityWeatherOverviewConfigurationPageModelFactory
    {
        public static Models.Page.InmersiveCityWeatherOverviewConfigurationPageModel Build(Support.Presentation.Models.View.PlacardViewModel placardViewModel)
        {
            if (placardViewModel == null) return new Models.Page.InmersiveCityWeatherOverviewConfigurationPageModel();

            var context = Newtonsoft.Json.JsonConvert.DeserializeObject<Templating.Presentation.Models.Context.InmersiveCityWeatherOverviewContextModel>(placardViewModel.SerializedContext);

            return new Models.Page.InmersiveCityWeatherOverviewConfigurationPageModel
            {
                RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                Label = placardViewModel.Label,
                ScreenPresentationTimeSecounds = System.Convert.ToInt32(context.ScreenPresentationTime.TotalSeconds),
                BackgroundCityImage = context.BackgroundCityImage.ToViewModel(),
                WeatherCacheMicroServerHost = context.WeatherCacheMicroServerHost,
                IsWeatherCacheMicroServerRequireSecureConnection = context.IsWeatherCacheMicroServerRequireSecureConnection,
                IsWeatherCacheMicroServerSecureConnectionStrict = context.IsWeatherCacheMicroServerSecureConnectionStrict,
                Service = context.Service,
                City = context.City
            };
        }
    }


}
