using Newtonsoft.Json;

namespace Monzo
{
    /// <summary>
    /// The merchant's address.
    /// </summary>
    public sealed class MerchantAddress
    {
        /// <summary>
        /// The merchant's address.
        /// </summary>
        /// <value>The address.</value>
        [JsonProperty("address")]
        public string Address { get; set; }

        /// <summary>
        /// The merchant's city.
        /// </summary>
        /// <value>The city.</value>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// The merchant's country.
        /// </summary>
        /// <value>The country.</value>
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// The merchant's latitude.
        /// </summary>
        /// <value>The latitude.</value>
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        /// <summary>
        /// The merchant's longitude.
        /// </summary>
        /// <value>The longitude.</value>
        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        /// <summary>
        /// The merchant's postcode.
        /// </summary>
        /// <value>The postcode.</value>
        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        /// <summary>
        /// The merchant's region.
        /// </summary>
        /// <value>The region.</value>
        [JsonProperty("region")]
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the formatted.
        /// </summary>
        /// <value>The formatted.</value>
        public string Formatted { get; set; }

        /// <summary>
        /// Gets or sets the formatted in short form.
        /// </summary>
        /// <value>The short formatted.</value>
        [JsonProperty("short_formatted")]
        public string ShortFormatted { get; set; }

        /// <summary>
        /// Gets or sets the zoom level.
        /// </summary>
        /// <value>The zoom level.</value>
        [JsonProperty("zoom_level")]
        public string ZoomLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MerchantAddress"/> is approximate.
        /// </summary>
        /// <value><c>null</c> if [approximate] contains no value, <c>true</c> if [approximate]; otherwise, <c>false</c>.</value>
        public bool? Approximate { get; set; }
    }
}