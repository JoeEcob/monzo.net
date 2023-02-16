using System.Diagnostics;
using Newtonsoft.Json;

namespace Monzo;

/// <summary>
/// Information about an account’s balance.
/// </summary>
[DebuggerDisplay("[{Value} {Currency,nq}]")]
public sealed class Balance
{
    /// <summary>
    /// The currently available balance of the account, as a 64bit integer in minor units of the currency, eg. pennies for GBP, or cents for EUR and USD.
    /// </summary>
    [JsonProperty("balance")]
    public long Value { get; set; }

    /// <summary>
    /// The ISO 4217 currency code.
    /// </summary>
    [JsonProperty("currency")]
    public string Currency { get; set; }

    /// <summary>
    /// The amount spent from this account today (considered from approx 4am onwards), as a 64bit integer in minor units of the currency.
    /// </summary>
    [JsonProperty("spend_today")]
    public long SpendToday { get; set; }
}
