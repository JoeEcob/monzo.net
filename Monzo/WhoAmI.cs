using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Monzo
{
    /// <summary>
    /// Returns information about the current access token.
    /// </summary>
    [DebuggerDisplay("[{Id,nq} {Description}]")]
    public sealed class WhoAmI
    {
        /// <summary>
        /// Boolean for if the user is authenticated.
        /// </summary>
        [JsonProperty("authenticated")]
        public bool Authenticated { get; set; }

        /// <summary>
        /// The id of the client.
        /// </summary>
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        /// <summary>
        /// The id of the user.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }
}
