namespace SmartSolucionesCuba.Viderhe.Weather.Models.OWM
{
    public class MinutelyWeatherModel
    {
        [Newtonsoft.Json.JsonProperty("dt")]
        public long MeasuredAt { get; set; }

        [Newtonsoft.Json.JsonProperty("precipitation")]
        public float Precipitation { get; set; }
    }
}
