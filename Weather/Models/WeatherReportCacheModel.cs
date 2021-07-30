namespace SmartSolucionesCuba.Viderhe.Weather.Models
{
    public class WeatherReportCacheModel
    {
        public Enumerations.PorvisionService PorvisionService { get; set; }

        public Api.Common.Models.WeatherReportModel Report { get; set; }
    }
}