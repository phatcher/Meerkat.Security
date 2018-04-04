using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Meerkat.Security.Activities
{
    public class ActivityContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (property.DeclaringType == typeof(Authorizations))
            {
                //switch (property.PropertyName)
                //{
                //    case "Scope":
                //    case "DefaultActivity":
                //    case "DefaultAllowUnauthenticated":
                //    case "DefaultAuthorization":
                //    case "Activities":
                //        property.ShouldSerialize = x => x != null;
                //        break;
                //}
            }

            if (property.DeclaringType == typeof(Activity))
            {
                if (property.PropertyName == "Allow")
                {
                    property.ShouldSerialize = x => x is Permission p && !p.IsEmpty();
                }
                if (property.PropertyName == "Deny")
                {
                    property.ShouldSerialize = x => x is Permission p && !p.IsEmpty();
                }
            }

            return property;
        }
    }
}
