﻿using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nop.Plugin.ExternalAuth.OAuth2.Models;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.ExternalAuth.OAuth2.Controllers
{
    public class OAuth2AuthenticationController : BasePluginController
    {
        private readonly IExternalAuthenticationService _externalAuthenticationService;
        private readonly ILocalizationService _localizationService;
        private readonly IOptionsMonitorCache<OAuthOptions> _optionsCache;
        private readonly ISettingService _settingService;
        private readonly OAuth2AuthenticationSettings _oAuth2AuthenticationSettings;
        private readonly IAuthenticationPluginManager _authenticationPluginManager;

        public OAuth2AuthenticationController(IExternalAuthenticationService externalAuthenticationService,
            ILocalizationService localizationService,
            IOptionsMonitorCache<OAuthOptions> optionsCache,
            ISettingService settingService,
            OAuth2AuthenticationSettings oAuth2AuthenticationSettings,
            IAuthenticationPluginManager authenticationPluginManager)
        {
            _externalAuthenticationService = externalAuthenticationService;
            _localizationService = localizationService;
            _optionsCache = optionsCache;
            _settingService = settingService;
            _oAuth2AuthenticationSettings = oAuth2AuthenticationSettings;
            _authenticationPluginManager = authenticationPluginManager;
        }

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {
            //prepare model
            var model = new ConfigurationModel
            {
                ClientId = _oAuth2AuthenticationSettings.ClientId,
                ClientSecret = _oAuth2AuthenticationSettings.ClientSecret,
                Authority = _oAuth2AuthenticationSettings.Authority,
                Scopes = _oAuth2AuthenticationSettings.Scopes,
                AdministratorsRoles = _oAuth2AuthenticationSettings.AdministratorsRoles
            };

            return View("~/Plugins/ExternalAuth.OAuth2/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        [AdminAntiForgery]
        [Area(AreaNames.Admin)]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();

            //save settings
            _oAuth2AuthenticationSettings.ClientId = model.ClientId;
            _oAuth2AuthenticationSettings.ClientSecret = model.ClientSecret;
            _oAuth2AuthenticationSettings.Authority = model.Authority;
            _oAuth2AuthenticationSettings.Scopes = model.Scopes;
            _oAuth2AuthenticationSettings.AdministratorsRoles = model.AdministratorsRoles;
            _settingService.SaveSetting(_oAuth2AuthenticationSettings);

            _optionsCache.TryRemove(OAuth2AuthenticationDefaults.AuthenticationScheme);

            //SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        public IActionResult Logout()
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("Logout", "Customer", false)
            };

            return SignOut(authenticationProperties, OAuth2AuthenticationDefaults.AuthenticationScheme);
        }

        public IActionResult Login(string returnUrl, bool useNopLogin = false)
        {
            if (IsNotConfigured() || useNopLogin)
            {
                return RedirectToAction("Login", "Customer", false);
            }

            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("LoginCallback", "OAuth2Authentication", new { returnUrl = returnUrl })
            };

            return Challenge(authenticationProperties, OAuth2AuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> LoginCallback(string returnUrl)
        {
            var authenticateResult = await this.HttpContext.AuthenticateAsync(OAuth2AuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded || !authenticateResult.Principal.Claims.Any())
                return RedirectToRoute("Login");

            var authenticationParameters = new ExternalAuthenticationParameters
            {
                ProviderSystemName = OAuth2AuthenticationDefaults.SystemName,
                AccessToken = await this.HttpContext.GetTokenAsync(OAuth2AuthenticationDefaults.AuthenticationScheme, OAuth2AuthenticationDefaults.AccessTokenName),
                Email = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Email)?.Value,
                ExternalIdentifier = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value,
                ExternalDisplayIdentifier = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Name)?.Value,
                Claims = authenticateResult.Principal.Claims.Select(claim => new ExternalAuthenticationClaim(claim.Type, claim.Value)).ToList()
            };

            return _externalAuthenticationService.Authenticate(authenticationParameters, returnUrl);
        }

        [HttpPost]
        public IActionResult Login(object model, string returnUrl, bool captchaValid) => RedirectToActionPreserveMethod("Login", "Customer");

        bool IsNotConfigured()
        {
            return
                _authenticationPluginManager.IsPluginActive(OAuth2AuthenticationDefaults.SystemName) == false ||
                string.IsNullOrEmpty(_oAuth2AuthenticationSettings.ClientId) ||
                string.IsNullOrEmpty(_oAuth2AuthenticationSettings.ClientSecret) ||
                string.IsNullOrEmpty(_oAuth2AuthenticationSettings.Authority) ||
                string.IsNullOrEmpty(_oAuth2AuthenticationSettings.Scopes);
        }
    }
}