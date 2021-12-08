using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Monzo.Messages
{
    internal sealed class MerchantJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);

            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                JObject o = (JObject)t;
                IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();

                o.WriteTo(writer);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            switch (token.Type)
            {
                case JTokenType.Object:
                    return token.ToObject<Merchant>();

                case JTokenType.String:
                    return new Merchant {Id = token.ToString()};

                default:
                    return null;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (Merchant);
        }
    }
}