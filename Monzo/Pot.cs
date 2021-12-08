using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Monzo
{
    /// <summary>
    /// Class Pot.
    /// </summary>
    public class Pot
    {
        /// <summary>
        /// The id of the pot.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The name of the pot.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The style of the pot.
        /// </summary>
        /// <value>The style.</value>
        [JsonProperty("style")]
        public string Style { get; set; }

        /// <summary>
        /// Current balance for the pot.
        /// </summary>
        /// <value>The balance.</value>
        [JsonProperty("balance")]
        public long Balance { get; set; }

        /// <summary>
        /// Currency type for the pot.
        /// </summary>
        /// <value>The currency.</value>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// A list of permissions for the pot. Usually contains a user_id and a permission_type.
        /// </summary>
        /// <value>The assigned permissions.</value>
        [JsonProperty("assigned_permissions")]
        public List<Dictionary<string, string>> AssignedPermissions { get; set; }

        /// <summary>
        /// The current account id being used when making a pot deposit / withdrawal.
        /// </summary>
        /// <value>The current account identifier.</value>
        [JsonProperty("current_account_id")]
        public string CurrentAccountId { get; set; }

        /// <summary>
        /// Date and time the pot was created.
        /// </summary>
        /// <value>The created.</value>
        [JsonProperty("created")]
        public DateTime Created { get; set; }

        /// <summary>
        /// Date and time the pot was last updated (includes withdrawals / deposits).
        /// </summary>
        /// <value>The updated.</value>
        [JsonProperty("updated")]
        public DateTime Updated { get; set; }

        /// <summary>
        /// Flag for if this pot should be used to round the change.
        /// </summary>
        /// <value><c>true</c> if [round up]; otherwise, <c>false</c>.</value>
        [JsonProperty("round_up")]
        public bool RoundUp { get; set; }

        /// <summary>
        /// Flag for if the pot is currently locked.
        /// </summary>
        /// <value><c>true</c> if locked; otherwise, <c>false</c>.</value>
        [JsonProperty("locked")]
        public bool Locked { get; set; }

        /// <summary>
        /// Flag for if the pot is deleted.
        /// </summary>
        /// <value><c>true</c> if deleted; otherwise, <c>false</c>.</value>
        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the goal amount.
        /// </summary>
        /// <value>The goal amount.</value>
        [JsonProperty("goal_amount")]
        public long? GoalAmount { get; set; }
    }
}