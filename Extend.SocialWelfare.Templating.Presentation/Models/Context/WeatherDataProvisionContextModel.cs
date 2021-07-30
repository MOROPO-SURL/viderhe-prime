namespace SmartSolucionesCuba.Viderhe.Extend.SocialWelfare.Templating.Presentation.Models.Context
{
    public class WeatherDataProvisionContextModel : Common.Templating.Presentation.Models.Context.AbstractScreenTimedPlacardContextModel
    {
        public string WeatherCacheMicroServerHost { get; set; }

        public bool IsWeatherCacheMicroServerRequireSecureConnection { get; set; }

        public bool IsWeatherCacheMicroServerSecureConnectionStrict { get; set; }

        public Enumerations.WeatherDataProvisionService Service { get; set; }

        public Weather.Api.Common.Models.CityModel City { get; set; }
    }
}