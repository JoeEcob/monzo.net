using System.Collections.Generic;
using Newtonsoft.Json;

namespace Monzo.Messages;

internal sealed class ListPotsResponse
{
    [JsonProperty("pots")]
    public IList<Pot> Pots { get; set; }
}