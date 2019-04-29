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

        /// <summary>
        /// Create a new instance of the <see cref="CorControllerActivityMapper"/>
        /// </summary>
        /// <param name="mappers"></param>
        public CorControllerActivityMapper(IControllerActivityMapper[] mappers)
        {
            this.mappers = new List<IControllerActivityMapper>(mappers);
        }

        /// <copydoc cref="IControllerActivityMapper.Map" />
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

        /// <summary>
        /// Apply the mappers in turn returning the first successful one
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Apply the mappers in turn returning the first successful one
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
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