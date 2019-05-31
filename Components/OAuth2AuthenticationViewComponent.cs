using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.ExternalAuth.OAuth2.Components
{
    /// <summary>
    /// Represents a view component to render 'Sign In with oAuth2.0' button on the login page
    /// </summary>
    [ViewComponent(Name = OAuth2AuthenticationDefaults.ViewComponentName)]
    public class OAuth2AuthenticationViewComponent : NopViewComponent
    {
        /// <summary>
        /// Invoke the external authentication view component
        /// </summary>
        /// <returns>View component result</returns>
        public IViewComponentResult Invoke()
        {
            return View("~/Plugins/ExternalAuth.OAuth2/Views/PublicInfo.cshtml");
        }
    }
}