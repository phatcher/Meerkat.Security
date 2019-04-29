using System.IO;

using Newtonsoft.Json;

namespace Meerkat.Security.Activities
{ 
    public static class JsonExtensions
    {
        public static JsonSerializerSettings AuthorizationSerializerSettings()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new ActivityContractResolver()
            };
            settings.Converters.Add(new ClaimJsonConverter());

            return settings;
        }

        /// <summary>
        /// Converts an <see cref="AuthorizationScope" /> to a json representation
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="formatting"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string ToJson(this AuthorizationScope entity, Formatting formatting = Formatting.Indented, JsonSerializerSettings settings = null)
        {
            return JsonConvert.SerializeObject(entity, formatting, settings ?? AuthorizationSerializerSettings());
        }

        /// <summary>
        /// Converts a json string into a <see cref="AuthorizationScope"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static AuthorizationScope ToAuthorizationScope(this string source, JsonSerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject<AuthorizationScope>(source, settings ?? AuthorizationSerializerSettings());
        }

        public static AuthorizationScope FromJsonFile(this string fileName)
        {
            var data = File.ReadAllText(fileName);

            var entity = data.ToAuthorizationScope();

            return entity;
        }
    }
}