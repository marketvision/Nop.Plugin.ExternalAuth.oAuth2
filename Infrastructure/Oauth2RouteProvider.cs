using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.ExternalAuth.OAuth2.Infrastructure
{
    public class Oauth2RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            ReplaceRoute(routeBuilder, "Login", "login/", "OAuth2Authentication", "Login", "Nop.Plugin.ExternalAuth.OAuth2.Controllers");
            ReplaceRoute(routeBuilder, "Logout", "logout/", "OAuth2Authentication", "Logout", "Nop.Plugin.ExternalAuth.OAuth2.Controllers");
        }

        private void ReplaceRoute(IRouteBuilder routeBuilder, string name, string template, string controller, string action, string assembly)
        {
            Route route = null;

            foreach (Route item in routeBuilder.Routes)
            {
                if (item.Name == name)
                {
                    route = item;
                    break;
                }
            }

            if (route != null)
                routeBuilder.Routes.Remove(route);

            routeBuilder.MapLocalizedRoute(name,
                  template,
                  new { controller = controller, action = action },
                  new { },
                  new[] { assembly }
              );
        }

        public int Priority => -1;
    }
}