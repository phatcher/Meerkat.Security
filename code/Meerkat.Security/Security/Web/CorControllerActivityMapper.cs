using System;
using System.Collections.Generic;

namespace Meerkat.Security.Web
{
    /// <summary>
    /// Implements a chain of responsibility pattern over a collection of <see cref="IControllerActivityMapper"/>s, first one wins.
    /// </summary>
    public class CorControllerActivityMapper : IControllerActivityMapper
    {
        private readonly IList<IControllerActivityMapper> mappers;

        public CorControllerActivityMapper(IControllerActivityMapper[] mappers)
        {
            this.mappers = new List<IControllerActivityMapper>(mappers);
        }

        public Tuple<string, string> Map(string controller, string action)
        {
            foreach (var mapper in mappers)
            {
                var result = mapper.Map(controller, action);
                if (result != null)
                {
                    return result;
                }
            }

            return new Tuple<string, string>(controller, action);
        }

        public string Resource(string controller, string action)
        {
            foreach (var mapper in mappers)
            {
                var result = mapper.Resource(controller, action);
                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }
            }

            return controller;
        }

        public string Action(string controller, string action)
        {
            foreach (var mapper in mappers)
            {
                var result = mapper.Action(controller, action);
                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }
            }

            return action;
        }
    }
}