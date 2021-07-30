namespace SmartSolucionesCuba.Viderhe.Weather.Api.Common.Models
{
    /// <summary>
    /// Class for the location with latitude and longitude.
    /// </summary>
    public class LocationModel : System.IEquatable<LocationModel>
    {
        /// <summary>
        /// The latitude of the <see cref="LocationModel"/>.
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// The longitude of the <see cref="LocationModel"/>.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="LocationModel"/>.
        /// </summary>
        /// <param name="latitude">The latitude of the location.</param>
        /// <param name="longitude">The longitude of the location.</param>
        public LocationModel(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(LocationModel other)
        {
            if (other == null) return false;

            return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public override bool Equals(object obj)
        {
            return obj is LocationModel other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                hash = hash * 23 + Latitude.GetHashCode();

                hash = hash * 23 + Longitude.GetHashCode();

                return hash;
            }
        }
    }
}
