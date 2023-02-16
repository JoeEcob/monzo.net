using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Monzo;

/// <summary>
/// Accounts represent a store of funds, and have a list of transactions.
/// </summary>
[DebuggerDisplay("[{Id,nq} {Description}]")]
public sealed class Account
{
    /// <summary>
    /// The id of the account.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// The account description.
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; }

    /// <summary>
    /// When the account was created.
    /// </summary>
    [JsonProperty("created")]
    public DateTime Created { get; set; }

    /// <summary>
    /// If the account is prepaid or a current account.
    /// </summary>
    [JsonProperty("type")]
    public AccountType Type { get; set; }

    /// <summary>
    /// Sort code for the account.
    /// </summary>
    [JsonProperty("sort_code")]
    public string SortCode { get; set; }

    /// <summary>
    /// Unique account number.
    /// </summary>
    [JsonProperty("account_number")]
    public string AccountNumber { get; set; }

    /// <summary>
    /// Flag for if the account has been closed.
    /// </summary>
    [JsonProperty("closed")]
    public bool Closed { get; set; }

    /// <summary>
    /// Currency of the account.
    /// </summary>
    [JsonProperty("currency")]
    public string Currency { get; set; }

    /// <summary>
    /// ISO Alpha-2 Country Code of the account.
    /// </summary>
    [JsonProperty("country_code")]
    public string CountryCode { get; set; }

    /// <summary>
    /// Owners of the account.
    /// </summary>
    [JsonProperty("owners")]
    public User[] Owners { get; set; }
}