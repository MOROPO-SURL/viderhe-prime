namespace SmartSolucionesCuba.Viderhe.Weather.Models.OWM
{
    public class CityModel
    {
        [Newtonsoft.Json.JsonProperty("id")]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("name")]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("country")]
        public string Country { get; set; }

        [Newtonsoft.Json.JsonProperty("coord")]
        public CoordenateModel Coord { get; set; }
    }
}
