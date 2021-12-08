using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Monzo
{
    /// <summary>
    /// The merchant a transaction wasd made at.
    /// </summary>
    [DebuggerDisplay("[{Id,nq} {Name}]")]
    public sealed class Merchant
    {
        /// <summary>
        /// The address of the merchant.
        /// </summary>
        /// <value>The address.</value>
        [JsonProperty("address")]
        public MerchantAddress Address { get; set; }

        /// <summary>
        /// The time the merchant was created at.
        /// </summary>
        /// <value>The created.</value>
        [JsonProperty("created")]
        public DateTime? Created { get; set; }

        /// <summary>
        /// Used to group individual merchants who are part of a chain.
        /// </summary>
        /// <value>The group identifier.</value>
        [JsonProperty("group_id")]
        public string GroupId { get; set; }

        /// <summary>
        /// The merchant's Id.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The merchant's logo image URL.
        /// </summary>
        /// <value>The logo.</value>
        [JsonProperty("logo")]
        public string Logo { get; set; }

        /// <summary>
        /// The emoji displayed for the merchant.
        /// </summary>
        /// <value>The emoji.</value>
        [JsonProperty("emoji")]
        public string Emoji { get; set; }

        /// <summary>
        /// The name of the merchant.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The category of the merchant.
        /// </summary>
        /// <value>The category.</value>
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Merchant"/> was an atm transaction.
        /// </summary>
        /// <value><c>null</c> if [atm] contains no value, <c>true</c> if [atm]; otherwise, <c>false</c>.</value>
        public bool? Atm { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether feedback was disabled.
        /// </summary>
        /// <value><c>true</c> if [disable feedback]; otherwise, <c>false</c>.</value>
        [JsonProperty("disable_feedback")]
        public bool DisableFeedback { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Merchant"/> is online.
        /// </summary>
        /// <value><c>null</c> if [online] contains no value, <c>true</c> if [online]; otherwise, <c>false</c>.</value>
        public bool? Online { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        /// <value>The metadata.</value>
        public MonzoWebhookTransactionMerchantMetadata Metadata { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty =>
            Id != null && Address == null && Created == null && GroupId == null && Logo == null &&
            Emoji == null && Name == null && Category == null && Atm == null &&
            Online == null;
    }
}