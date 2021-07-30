namespace SmartSolucionesCuba.Viderhe.Weather.Models.OWM
{
    public class DayTemperatureSetModel
    {
        [Newtonsoft.Json.JsonProperty("day")]
        public float Day { get; set; }

        [Newtonsoft.Json.JsonProperty("night")]
        public float Night { get; set; }

        [Newtonsoft.Json.JsonProperty("eve")]
        public float Evening { get; set; }

        [Newtonsoft.Json.JsonProperty("morn")]
        public float Morning { get; set; }
    }
}
