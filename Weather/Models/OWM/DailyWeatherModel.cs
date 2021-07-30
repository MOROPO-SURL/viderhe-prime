namespace SmartSolucionesCuba.Viderhe.Weather.Models.OWM
{
    public class DailyWeatherModel
    {
        [Newtonsoft.Json.JsonProperty("dt")]
        public long MeasuredAt { get; set; }

        [Newtonsoft.Json.JsonProperty("sunrise")]
        public long SunRiseAt { get; set; }

        [Newtonsoft.Json.JsonProperty("sunset")]
        public long SunSetAt { get; set; }

        [Newtonsoft.Json.JsonProperty("temp")]
        public DayTemperatureSummaryModel TemperatureSummary { get; set; }

        [Newtonsoft.Json.JsonProperty("feels_like")]
        public DayTemperatureSetModel TemperatureFeelLike { get; set; }

        [Newtonsoft.Json.JsonProperty("pressure")]
        public int Pressure { get; set; }

        [Newtonsoft.Json.JsonProperty("humidity")]
        public int Humidity { get; set; }

        [Newtonsoft.Json.JsonProperty("dew_point")]
        public float DewPoint { get; set; }

        [Newtonsoft.Json.JsonProperty("wind_speed")]
        public float WindSpeed { get; set; }

        [Newtonsoft.Json.JsonProperty("wind_deg")]
        public float WindDegrees { get; set; }

        [Newtonsoft.Json.JsonProperty("weather")]
        public WeatherConditionModel[] WeatherConditions { get; set; }

        [Newtonsoft.Json.JsonProperty("clouds")]
        public float CloudDensity { get; set; }

        [Newtonsoft.Json.JsonProperty("pop")]
        public float POP { get; set; }

        [Newtonsoft.Json.JsonProperty("uvi")]
        public float UltravioletIndex { get; set; }
    }
}
