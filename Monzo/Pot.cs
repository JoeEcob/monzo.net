using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Monzo
{
    public class Pot
    {
        /// <summary>
        /// The id of the pot.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The name of the pot.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The style of the pot.
        /// </summary>
        [JsonProperty("style")]
        public string Style { get; set; }

        /// <summary>
        /// Current balance for the pot.
        /// </summary>
        [JsonProperty("balance")]
        public long Balance { get; set; }

        /// <summary>
        /// Currency type for the pot.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// A list of permissions for the pot. Usually contains a user_id and a permission_type.
        /// </summary>
        [JsonProperty("assigned_permissions")]
        public List<Dictionary<string, string>> AssignedPermissions { get; set; }

        /// <summary>
        /// The current account id being used when making a pot deposit / withdrawal.
        /// </summary>
        [JsonProperty("current_account_id")]
        public string CurrentAccountId { get; set; }

        /// <summary>
        /// Date and time the pot was created.
        /// </summary>
        [JsonProperty("created")]
        public DateTime Created { get; set; }

        /// <summary>
        /// Date and time the pot was last updated (includes withdrawals / deposits).
        /// </summary>
        [JsonProperty("updated")]
        public DateTime Updated { get; set; }

        /// <summary>
        /// Flag for if this pot should be used to round the change.
        /// </summary>
        [JsonProperty("round_up")]
        public bool RoundUp { get; set; }

        /// <summary>
        /// Flag for if the pot is currently locked.
        /// </summary>
        [JsonProperty("locked")]
        public bool Locked { get; set; }

        /// <summary>
        /// Flag for if the pot is deleted.
        /// </summary>
        [JsonProperty("deleted")]
        public bool Deleted { get; set; }
    }
}