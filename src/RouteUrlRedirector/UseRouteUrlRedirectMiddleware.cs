﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RouteUrlRedirector.Configuration;

namespace RouteUrlRedirector
{
    internal class UseRouteUrlRedirectMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Action<RouteRedirectionConfiguration> _routeConfigurations;
        private IDictionary<string, RouteItem> _builtRouteItems;

        public UseRouteUrlRedirectMiddleware(RequestDelegate next, Action<RouteRedirectionConfiguration> routeConfigurations)
        {
            _next = next;
            _routeConfigurations = routeConfigurations;
        }

        public async Task Invoke(HttpContext context)
        {
            if (_builtRouteItems == null)
            {
                var config = new RouteRedirectionConfiguration();
                _routeConfigurations(config);

                _builtRouteItems = config.BuildOptions();
            }

            if (!context.Request.Path.HasValue || !_builtRouteItems.ContainsKey(context.Request.Path.Value))
            {
                await _next(context);
                return;
            }

            RouteItem newPath = _builtRouteItems[context.Request.Path.Value];
            context.Response.Redirect(newPath.Result, newPath.PermanencyType == RoutePermanencyType.Permanently);
        }
    }
}