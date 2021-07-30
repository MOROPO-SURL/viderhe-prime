namespace SmartSolucionesCuba.Viderhe.Weather.Models.OWM
{
    public class AlertWeatherModel
    {
        [Newtonsoft.Json.JsonProperty("sender_name")]
        public string Author { get; set; }

        [Newtonsoft.Json.JsonProperty("event")]
        public string Title { get; set; }

        [Newtonsoft.Json.JsonProperty("start")]
        public long StartAt { get; set; }

        [Newtonsoft.Json.JsonProperty("end")]
        public long EndAt { get; set; }

        [Newtonsoft.Json.JsonProperty("description")]
        public string Description { get; set; }
    }
}
