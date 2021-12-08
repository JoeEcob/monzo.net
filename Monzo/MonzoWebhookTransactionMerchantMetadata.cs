using Newtonsoft.Json;

namespace Monzo
{
    /// <summary>
    /// Class MonzoWebhookTransactionMerchantMetadata.
    /// </summary>
    public class MonzoWebhookTransactionMerchantMetadata
    {
        /// <summary>
        /// Gets or sets the created for transaction.
        /// </summary>
        /// <value>The created for transaction.</value>
        [JsonProperty("created_for_transaction")]
        public string CreatedForTransaction { get; set; }

        /// <summary>
        /// Gets or sets the enriched from settlement.
        /// </summary>
        /// <value>The enriched from settlement.</value>
        [JsonProperty("enriched_from_settlement")]
        public string EnrichedFromSettlement { get; set; }

        /// <summary>
        /// Gets or sets the foursquare category.
        /// </summary>
        /// <value>The foursquare category.</value>
        [JsonProperty("foursquare_category")]
        public string FoursquareCategory { get; set; }

        /// <summary>
        /// Gets or sets the foursquare category token.
        /// </summary>
        /// <value>The foursquare category token.</value>
        [JsonProperty("foursquare_category_icon")]
        public string FoursquareCategoryToken { get; set; }

        /// <summary>
        /// Gets or sets the foursquare identifier.
        /// </summary>
        /// <value>The foursquare identifier.</value>
        [JsonProperty("foursquare_id")]
        public string FoursquareId { get; set; }

        /// <summary>
        /// Gets or sets the foursquare website.
        /// </summary>
        /// <value>The foursquare website.</value>
        [JsonProperty("foursquare_website")]
        public string FoursquareWebsite { get; set; }

        /// <summary>
        /// Gets or sets the google places icon.
        /// </summary>
        /// <value>The google places icon.</value>
        [JsonProperty("google_places_icon")]
        public string GooglePlacesIcon { get; set; }

        /// <summary>
        /// Gets or sets the google places identifier.
        /// </summary>
        /// <value>The google places identifier.</value>
        [JsonProperty("google_places_id")]
        public string GooglePlacesId { get; set; }

        /// <summary>
        /// Gets or sets the name of the google places.
        /// </summary>
        /// <value>The name of the google places.</value>
        [JsonProperty("google_places_name")]
        public string GooglePlacesName { get; set; }

        /// <summary>
        /// Gets or sets the name of the suggested.
        /// </summary>
        /// <value>The name of the suggested.</value>
        [JsonProperty("suggested_name")]
        public string SuggestedName { get; set; }

        /// <summary>
        /// Gets or sets the suggested tags.
        /// </summary>
        /// <value>The suggested tags.</value>
        [JsonProperty("suggested_tags")]
        public string SuggestedTags { get; set; }

        /// <summary>
        /// Gets or sets the twitter identifier.
        /// </summary>
        /// <value>The twitter identifier.</value>
        [JsonProperty("twitter_id")]
        public string TwitterId { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        /// <value>The website.</value>
        public string Website { get; set; }
    }
}