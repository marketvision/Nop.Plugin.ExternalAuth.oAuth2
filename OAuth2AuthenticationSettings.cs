using Nop.Core.Configuration;

namespace Nop.Plugin.ExternalAuth.OAuth2
{
    /// <summary>
    /// Represents settings of the oAuth2.0 authentication method
    /// </summary>
    public class OAuth2AuthenticationSettings : ISettings
    {
        /// <summary>
        /// Gets or sets OAuth2 client identifier
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets OAuth2 client secret
        /// </summary>
        public string ClientSecret { get; set; }

        public string Authority { get; set; }

        public string Scopes { get; set; }

        public string AdministratorsRoles { get; set; }
    }
}