using System;
using System.Security.Claims;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Meerkat.Security.Activities
{
    public class ClaimJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is Claim claim))
            {
                return;
            }

            writer.WriteStartObject();

            if (!string.IsNullOrEmpty(claim.Type))
            {
                writer.WritePropertyName("Type");
                writer.WriteValue(claim.Type);
            }
            if (!string.IsNullOrEmpty(claim.Value))
            {
                writer.WritePropertyName("Value");
                writer.WriteValue(claim.Value);
            }
            if (!string.IsNullOrEmpty(claim.Issuer))
            {
                writer.WritePropertyName("Issuer");
                writer.WriteValue(claim.Issuer);
            }
            if (!string.IsNullOrEmpty(claim.ValueType) && claim.ValueType != "http://www.w3.org/2001/XMLSchema#string")
            {
                writer.WritePropertyName("ValueType");
                writer.WriteValue(claim.ValueType);
            }

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var item = JObject.Load(reader);

            var type = (string)item["Type"];
            var value = (string)item["Value"];
            var issuer = (string)item["Issuer"];
            var valueType = (string)item["ValueType"];

            return new Claim(type, value, valueType, issuer);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Claim).IsAssignableFrom(objectType);
        }
    }
}
