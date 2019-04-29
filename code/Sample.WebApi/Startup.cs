using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Microsoft.Owin;
using Microsoft.Owin.Extensions;

using Owin;

using Unity.AspNet.Mvc;
using Unity.AspNet.WebApi;

[assembly: OwinStartup(typeof(Sample.Web.Startup))]

namespace Sample.Web
{
    public class Startup
    {
        ~Startup()
        {
            // Needed for use with OwinTestServer
            UseOwinHmac = true;
            SelfHost = true;
        }

        public static bool SelfHost { get; set; }

        public static bool UseOwinHmac { get; set; }

        public static bool IgnoreMvc { get; set; }

        public static HttpConfiguration Config { get; private set; }

        public static void Reset()
        {
            UnityConfig.Container = null;
            Config = null;
        }

        public void Configuration(IAppBuilder app)
        {
            // IoC
            var container = UnityConfig.GetConfiguredContainer();
            var resolver = new UnityHierarchicalDependencyResolver(container);

            // Owin config
            //app.UseDependencyResolverScope(resolver);
            //app.UseCors(CorsOptions.AllowAll);
            //if (UseOwinHmac)
            //{
            //    app.UseHmacAuthentication(resolver);
            //}

            // NB Must be before WebApiConfig.Register
            //ConfigureAuth(app);
            app.UseStageMarker(PipelineStage.Authenticate);
            // NB Needs to be after ConfigureAuth so we can enhance the authenticated principal
            //app.UseClaimsTransformation(resolver);
            app.UseStageMarker(PipelineStage.PostAuthenticate);

            // See http://stackoverflow.com/questions/33402654/web-api-with-owin-throws-objectdisposedexception-for-httpmessageinvoker
            // and http://aspnetwebstack.codeplex.com/workitem/2091

            if (SelfHost)
            {
                // WebAPI configuration
                Config = new HttpConfiguration
                {
                    DependencyResolver = resolver
                };

                //if (!UseOwinHmac)
                //{
                //    Config.Filters.Add(new HmacAuthenticationAttribute());
                //}

                WebApiConfig.Register(Config);
                app.UseWebApi(Config);
            }
            else
            {
                Config = GlobalConfiguration.Configuration;
                Config.DependencyResolver = resolver;

                //if (!UseOwinHmac)
                //{
                //    Config.Filters.Add(new HmacAuthenticationAttribute());
                //}

                // http://stackoverflow.com/questions/19907226/asp-net-webapi-2-attribute-routing-not-working
                // Needs to be before RouteConfig.RegisterRoutes(RouteTable.Routes);
                GlobalConfiguration.Configure(WebApiConfig.Register);
            }

            // MVC configuration
            // NB Have to flip sense as run before static
            if (!IgnoreMvc)
            {
                FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
                FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));
                DependencyResolver.SetResolver(new Unity.AspNet.Mvc.UnityDependencyResolver(container));

                AreaRegistration.RegisterAllAreas();
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
                RouteConfig.RegisterRoutes(RouteTable.Routes);

                MvcHandler.DisableMvcResponseHeader = true;
            }
        }
    }
}