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

        /// <summary>
        /// Converts an <see cref="AuthorizationScope" /> to a json representation
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string ToJson(this AuthorizationScope entity)
        {
            return JsonConvert.SerializeObject(entity, AuthorizationsSettings());
        }

        /// <summary>
        /// Converts a json string into a <see cref="AuthorizationScope"/>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static AuthorizationScope ToAuthorizationScope(this string source)
        {
            return JsonConvert.DeserializeObject<AuthorizationScope>(source, AuthorizationsSettings());
        }
    }
}
