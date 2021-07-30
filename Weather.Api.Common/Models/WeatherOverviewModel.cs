namespace SmartSolucionesCuba.Viderhe.Weather.Api.Common.Models
{
    public class WeatherOverviewModel
    {
        public System.DateTime MeasuredAt { get; set; }

        public int ConditionId { get; set; }

        public string ConditionDetail { get; set; }

        public System.DateTime SunRiseAt { get; set; }

        public System.DateTime SunSetAt { get; set; }

        public float Temperature { get; set; }

        public float TemperatureFeelLike { get; set; }

        public float MinTemperature { get; set; }

        public float MaxTemperature { get; set; }

        public int Pressure { get; set; }

        public int Humidity { get; set; }

        public float DewPoint { get; set; }

        public float UltravioletIndex { get; set; }

        public float CloudDensity { get; set; }

        public int Visibility { get; set; }

        public float WindSpeed { get; set; }

        public float WindDegrees { get; set; }
    }
}
