using System;

namespace Meerkat.Security.Web
{
    /// <summary>
    /// Converts standard MVC and API verbs into permission names.
    /// <para>
    /// Current mappings include
    /// <list type="table">
    /// <item>
    ///     <term>Get, GetFromX, GetX, GetXs, GetXies</term>
    ///     <description>X, Read</description>
    /// </item>
    /// <item>
    ///     <term>Post, PostFromX</term>
    ///     <description>X, Create</description>
    /// </item>
    /// <item>
    ///     <term>Edit</term>
    ///     <description>Update</description>
    /// </item>    
    /// <item>
    ///     <term>Patch, PatchX</term>
    ///     <description>Update</description>
    /// </item>
    /// <item>
    ///     <term>Put, PutX</term>
    ///     <description>Update</description>
    /// </item>
    /// </list>
    /// </para>
    /// </summary>
    public class ControllerActivityMapper : IControllerActivityMapper
    {
        /// <copydoc cref="IControllerActivityMapper.Map" />
        public Tuple<string, string> Map(string controller, string controllerAction)
        {
            var resource = controller;
            var action = controllerAction;

            // OData handlers
            if (action != "Get" && action.StartsWith("Get"))
            {
                // TODO: Probably an issue with plurals on WebAPI controllers.
                resource = action.StartsWith("GetFrom") ? action.Substring(7) : action.Substring(3);
                if (resource.EndsWith("ies"))
                {
                    resource = resource.Substring(0, resource.Length - 3) + "y";
                }
                else if (resource.EndsWith("s"))
                {
                    resource = resource.Substring(0, resource.Length - 1);
                }

                action = "Get";
            }
            else if (action != "Post" && action.StartsWith("PostFrom"))
            {
                resource = action.Substring(8);
                action = "Post";
            }
            else if (action != "Put" && action.StartsWith("Put"))
            {
                resource = action.Substring(3);
                action = "Put";
            }
            else if (action != "Patch" && action.StartsWith("Patch"))
            {
                resource = action.Substring(5);
                action = "Patch";
            }

            // Basic transposes
            switch (action.ToLowerInvariant())
            {
                case "get":
                case "details": // Could add the identity to get row-level, but we would need more context - controller responsibility?
                    action = "Read";
                    break;

                case "post":
                    action = "Create";
                    break;

                case "put":
                case "patch":
                case "edit":
                    action = "Update";
                    break;
            }

            return new Tuple<string, string>(resource, action);
        }
    }
}
