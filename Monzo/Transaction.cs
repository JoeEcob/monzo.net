using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Monzo.Messages;
using Newtonsoft.Json;

namespace Monzo
{
    /// <summary>
    /// Transactions are movements of funds into or out of an account. Negative transactions represent debits (ie. spending money) and positive transactions represent credits (ie. receiving money).
    /// </summary>
    [DebuggerDisplay("[{Id,nq} {Amount} {Currency,nq} {Description}]")]
    public sealed class Transaction
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetName()
        {
            if (Merchant != null)
                return Merchant.Name;
            if (CounterParty != null)
                return CounterParty.Name;
            if (Metadata != null && Metadata.ContainsKey("pot_id"))
                return "Pot Transaction";

            return "Unknown name";
        }

        /// <summary>
        /// Gets the pot identifier.
        /// </summary>
        /// <value>The pot identifier.</value>
        public string PotId => Metadata.Any(x => x.Key == "pot_id") ? Metadata.FirstOrDefault(x => x.Key == "pot_id").Value : null;

        // This so far is only populated from SQL
        /// <summary>
        /// Gets or sets the pot.
        /// </summary>
        /// <value>The pot.</value>
        public Pot Pot { get; set; }

        /// <summary>
        /// The currently available balance of the account, as a 64bit integer in minor units of the currency, eg. pennies for GBP, or cents for EUR and USD.
        /// </summary>
        /// <value>The account balance.</value>
        [JsonProperty("account_balance")]
        public long AccountBalance { get; set; }

        /// <summary>
        /// The amount of the transaction in minor units of currency. For example pennies in the case of GBP. A negative amount indicates a debit (most card transactions will have a negative amount)
        /// </summary>
        /// <value>The amount.</value>
        [JsonProperty("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// This is only present on declined transactions! Valid values are INSUFFICIENT_FUNDS, CARD_INACTIVE, CARD_BLOCKED or OTHER.
        /// </summary>
        /// <value>The decline reason.</value>
        [JsonProperty("decline_reason")]
        public string DeclineReason { get; set; }

        /// <summary>
        /// The category can be set for each transaction by the user. Over time we learn which merchant goes in which category and auto-assign the category of a transaction. If the user hasn’t set a category, we’ll return the default category of the merchant on this transactions. Top-ups have category “monzo”. Valid values are general, eating_out, expenses, transport, cash, bills, entertainment, shopping, holidays, groceries
        /// </summary>
        /// <value>The category.</value>
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>
        /// Details of the payee if the transaction was a bank transfer. Null otherwise.
        /// </summary>
        /// <value>The counter party.</value>
        [JsonProperty("counterparty")]
        public CounterParty CounterParty { get; set; }

        /// <summary>
        /// Time the transaction was created.
        /// </summary>
        /// <value>The created.</value>
        [JsonProperty("created")]
        public DateTime? Created { get; set; }

        /// <summary>
        /// The ISO 4217 currency code.
        /// </summary>
        /// <value>The currency.</value>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Transaction description.
        /// </summary>
        /// <value>The description.</value>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// The transaction's Id.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Flag for if the transaction should be included in your spending summary.
        /// </summary>
        /// <value><c>true</c> if [include in spending]; otherwise, <c>false</c>.</value>
        [JsonProperty("include_in_spending")]
        public bool IncludeInSpending { get; set; }

        /// <summary>
        /// Top-ups to an account are represented as transactions with a positive amount and is_load = true. Other transactions such as refunds, reversals or chargebacks may have a positive amount but is_load = false
        /// </summary>
        /// <value><c>true</c> if this instance is load; otherwise, <c>false</c>.</value>
        [JsonProperty("is_load")]
        public bool IsLoad { get; set; }

        /// <summary>
        /// This contains the merchant_id of the merchant that this transaction was made at. If you pass ?expand[]=merchant in your request URL, it will contain lots of information about the merchant.
        /// </summary>
        /// <value>The merchant.</value>
        [JsonProperty("merchant")]
        [JsonConverter(typeof(MerchantJsonConverter))]
        public Merchant Merchant { get; set; }

        /// <summary>
        /// You may store your own key-value annotations against a transaction in its metadata. Metadata is private to your application.
        /// </summary>
        /// <value>The metadata.</value>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Notes entered by the user against the transaction.
        /// </summary>
        /// <value>The notes.</value>
        [JsonProperty("notes")]
        public string Notes { get; set; }

        /// <summary>
        /// You probably don’t need to worry about this. Card transactions only settle 24-48 hours (sometimes even more!) after the purchase; until then they are just “authorised” and settled = false on them.
        /// </summary>
        /// <value>The settled.</value>
        [JsonProperty("settled")]
        public DateTime? Settled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [amount is pending].
        /// </summary>
        /// <value><c>true</c> if [amount is pending]; otherwise, <c>false</c>.</value>
        [JsonProperty("amount_is_pending")]
        public bool AmountIsPending { get; set; }

        /// <summary>
        /// Gets or sets the updated.
        /// </summary>
        /// <value>The updated.</value>
        public DateTime? Updated { get; set; }
        /// <summary>
        /// Gets the amount pounds.
        /// </summary>
        /// <value>The amount pounds.</value>
        public decimal AmountPounds => (decimal)Amount / 100;
        /// <summary>
        /// Gets or sets the local amount.
        /// </summary>
        /// <value>The local amount.</value>
        [JsonProperty("local_amount")]
        public long LocalAmount { get; set; }
        /// <summary>
        /// Gets or sets the local currency.
        /// </summary>
        /// <value>The local currency.</value>
        [JsonProperty("local_currency")]
        public string LocalCurrency { get; set; }
        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        /// <value>The account identifier.</value>
        [JsonProperty("account_id")]
        public string AccountId { get; set; }
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the scheme.
        /// </summary>
        /// <value>The scheme.</value>
        public string Scheme { get; set; }
        /// <summary>
        /// Gets or sets the dedupe identifier.
        /// </summary>
        /// <value>The dedupe identifier.</value>
        [JsonProperty("dedupe_id")]
        public string DedupeId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Transaction"/> is originator.
        /// </summary>
        /// <value><c>true</c> if originator; otherwise, <c>false</c>.</value>
        public bool Originator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can be excluded from breakdown.
        /// </summary>
        /// <value><c>true</c> if this instance can be excluded from breakdown; otherwise, <c>false</c>.</value>
        [JsonProperty("can_be_Excluded_from_breakdown")]
        public bool CanBeExcludedFromBreakdown { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance can be made subscription.
        /// </summary>
        /// <value><c>true</c> if this instance can be made subscription; otherwise, <c>false</c>.</value>
        [JsonProperty("can_be_made_subscription")]
        public bool CanBeMadeSubscription { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance can split the bill.
        /// </summary>
        /// <value><c>true</c> if this instance can split the bill; otherwise, <c>false</c>.</value>
        [JsonProperty("can_split_the_bill")]
        public bool CanSplitTheBill { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance can add to tab.
        /// </summary>
        /// <value><c>true</c> if this instance can add to tab; otherwise, <c>false</c>.</value>
        [JsonProperty("can_add_to_tab")]
        public bool CanAddToTab { get; set; }
        /// <summary>
        /// Gets or sets the atm fees detailed.
        /// </summary>
        /// <value>The atm fees detailed.</value>
        [JsonProperty("atm_fees_detailed")]
        public object AtmFeesDetailed { get; set; }
        /// <summary>
        /// Gets or sets the labels.
        /// </summary>
        /// <value>The labels.</value>
        public string[] Labels { get; set; }

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        /// <value>The attachments.</value>
        public object Attachments { get; set; }
        /// <summary>
        /// Gets or sets the international.
        /// </summary>
        /// <value>The international.</value>
        public object International { get; set; }
    }
}