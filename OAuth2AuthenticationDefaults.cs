namespace Nop.Plugin.ExternalAuth.OAuth2
{
    /// <summary>
    /// Represents constants of the oAuth2.0 authentication method
    /// </summary>
    public class OAuth2AuthenticationDefaults
    {
        /// <summary>
        /// System name of the oAuth2.0 authentication method
        /// </summary>
        public static string SystemName => "ExternalAuth.OAuth2";

        /// <summary>
        /// The logical name of authentication scheme
        /// </summary>
        public static string AuthenticationScheme => "IdSrv";

        /// <summary>
        /// The issuer that should be used for any claims that are created
        /// </summary>
        public static string ClaimsIssuer => "https://account.justpruvit.com";

        /// <summary>
        /// The name of the access token
        /// </summary>
        public static string AccessTokenName => "access_token";

        /// <summary>
        /// The claim type of the avatar
        /// </summary>
        public static string AvatarClaimType => "picture";

        /// <summary>
        /// Callback path
        /// </summary>
        public static string SigninCallbackPath => "/signin-oauth2";

        /// <summary>
        /// Callback path
        /// </summary>
        public static string SignoutCallbackPath => "/signout-oauth2";

        /// <summary>
        /// Name of the view component
        /// </summary>
        public const string ViewComponentName = "OAuth2Authentication";
    }
}