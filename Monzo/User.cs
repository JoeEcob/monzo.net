using System.Diagnostics;
using Newtonsoft.Json;

namespace Monzo;

/// <summary>
/// User represents the details of an account.
/// </summary>
[DebuggerDisplay("[{Id,nq} {PreferredName}]")]
public sealed class User {
    /// <summary>
    /// The id of the user.
    /// </summary>
    [JsonProperty("user_id")]
    public string Id { get; set; }
        
    /// <summary>
    /// The preferred Name of the user.
    /// </summary>
    [JsonProperty("preferred_name")]
    public string PreferredName { get; set; }
        
    /// <summary>
    /// The preferred First Name of the user.
    /// </summary>
    [JsonProperty("preferred_first_name")]
    public string PreferredFirstName { get; set; }
}