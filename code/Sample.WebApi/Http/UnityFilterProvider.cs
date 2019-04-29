using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using Unity;

namespace Sample.Web.Http
{
    public class UnityFilterProvider : IFilterProvider
    {
        private readonly IUnityContainer container;
        private readonly ActionDescriptorFilterProvider defaultProvider;

        public UnityFilterProvider(IUnityContainer container)
        {
            this.container = container;
            defaultProvider = new ActionDescriptorFilterProvider();
        }

        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            var attributes = defaultProvider.GetFilters(configuration, actionDescriptor);

            foreach (var attr in attributes)
            {
                container.BuildUp(attr.Instance.GetType(), attr.Instance);
            }

            return attributes;
        }
    }
}