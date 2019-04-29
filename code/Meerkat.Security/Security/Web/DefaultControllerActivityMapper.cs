﻿using System;

namespace Meerkat.Security.Web
{
    /// <summary>
    /// Basic implementation that just returns the original values
    /// </summary>
    public class DefaultControllerActivityMapper : IControllerActivityMapper
    {
        /// <copydoc cref="IControllerActivityMapper.Map" />
        public Tuple<string, string> Map(string controller, string action)
        {
            return new Tuple<string, string>(controller, action);
        }
    }
}