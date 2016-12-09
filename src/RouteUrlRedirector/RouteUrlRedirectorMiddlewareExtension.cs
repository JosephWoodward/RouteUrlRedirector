﻿using System;
using Microsoft.AspNetCore.Builder;

namespace RouteUrlRedirector
{
    public static class RouteUrlRedirectorMiddlewareExtension
    {
        public static IApplicationBuilder UseRequestRedirect(this IApplicationBuilder app, Action<IRouteRedirectAction> configureRedirects)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            if (configureRedirects == null)
                throw new ArgumentNullException(nameof(configureRedirects));

            return app.UseMiddleware<UseRouteUrlRedirectMiddleware>(configureRedirects);
        }

        public static IApplicationBuilder ForceLowercaseUrl(this IApplicationBuilder app)
	    {
		    if (app == null)
			    throw new ArgumentNullException(nameof(app));

		    return app.UseMiddleware<ForceLowercaseUrlMiddleware>();
	    }
    }
}