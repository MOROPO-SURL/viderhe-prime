namespace SmartSolucionesCuba.Viderhe.Weather.Api.Common.Models
{
    public class CityModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ISOCountryCode { get; set; }

        public LocationModel Location { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is CityModel)) return false;

            var target = obj as CityModel;

            return Id.Equals(target.Id);
        }

        public override int GetHashCode()
        {
            var hashCode = -1293161159;
            hashCode = hashCode * -1521134295 + System.Collections.Generic.EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + System.Collections.Generic.EqualityComparer<string>.Default.GetHashCode(ISOCountryCode);
            return hashCode;
        }
    }
}
