using Nop.Core;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;

namespace Nop.Plugin.ExternalAuth.OAuth2
{
    /// <summary>
    /// Represents method for the authentication with OAuth2
    /// </summary>
    public class OAuth2AuthenticationMethod : BasePlugin, IExternalAuthenticationMethod
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public OAuth2AuthenticationMethod(ILocalizationService localizationService,
            ISettingService settingService,
            IWebHelper webHelper)
        {
            _localizationService = localizationService;
            _settingService = settingService;
            _webHelper = webHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/OAuth2Authentication/Configure";
        }

        /// <summary>
        /// Gets a name of a view component for displaying plugin in public store
        /// </summary>
        /// <returns>View component name</returns>
        public string GetPublicViewComponentName()
        {
            return OAuth2AuthenticationDefaults.ViewComponentName;
        }

        /// <summary>
        /// Install the plugin
        /// </summary>
        public override void Install()
        {
            //settings
            _settingService.SaveSetting(new OAuth2AuthenticationSettings());

            //locales
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.ClientId", "Client ID");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.ClientId.Hint", "Enter the OAuth2 client ID here.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.ClientSecret", "Client secret");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.ClientSecret.Hint", "Enter the OAuth2 client secret here.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.Authority", "Authority");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.Authority.Hint", "Enter the OAuth2 server.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.Scopes", "Scopes");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.Scopes.Hint", "Enter the OAuth2 client scopes.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.AdministratorsRoles", "Administrators roles");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.AdministratorsRoles.Hint", "Enter the roles which allows the logged in user to administrate the platform.");

            base.Install();
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<OAuth2AuthenticationSettings>();

            //locales
            _localizationService.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.ClientId");
            _localizationService.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.ClientId.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.ClientSecret");
            _localizationService.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.ClientSecret.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.Authority");
            _localizationService.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.Authority.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.Scopes");
            _localizationService.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.Scopes.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.AdministratorsRoles");
            _localizationService.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.AdministratorsRoles.Hint");

            base.Uninstall();
        }

        #endregion
    }
}