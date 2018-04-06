using Newtonsoft.Json;

namespace Meerkat.Security.Activities
{
    public static class JsonExtensions
    {
        public static JsonSerializerSettings AuthorizationsSettings()
        {
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new ActivityContractResolver()
            };
            settings.Converters.Add(new ClaimJsonConverter());

            return settings;
        }

        public static string ToJson(this AuthorizationScope entity)
        {
            return JsonConvert.SerializeObject(entity, AuthorizationsSettings());
        }

        public static AuthorizationScope ToAuthorizations(this string source)
        {
            return JsonConvert.DeserializeObject<AuthorizationScope>(source, AuthorizationsSettings());
        }
    }
}
