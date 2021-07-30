namespace SmartSolucionesCuba.Viderhe.Weather.Models.OWM
{
    public class WeatherConditionModel
    {
        [Newtonsoft.Json.JsonProperty("id")]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("main")]
        public string Header { get; set; }

        [Newtonsoft.Json.JsonProperty("description")]
        public string Detail { get; set; }

        [Newtonsoft.Json.JsonProperty("icon")]
        public string Icon { get; set; }
    }
}
