using Newtonsoft.Json;

namespace Monzo
{
    public class CounterParty
    {
        /// <summary>
        /// The account number of the counterparty.
        /// </summary>
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// The name of the payee.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The sort code of the payee.
        /// </summary>
        [JsonProperty("sort_code")]
        public string SortCode { get; set; }

        /// <summary>
        /// The user id of the payee, anonymous if not known.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }
}
