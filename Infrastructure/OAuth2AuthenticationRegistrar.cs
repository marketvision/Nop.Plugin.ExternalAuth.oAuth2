using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Services.Authentication.External;

namespace Nop.Plugin.ExternalAuth.OAuth2.Infrastructure
{
    /// <summary>
    /// Represents registrar of OAuth2 external authentication
    /// </summary>
    public class OAuth2AuthenticationRegistrar : IExternalAuthenticationRegistrar
    {
        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="builder">Authentication builder</param>
        public void Configure(AuthenticationBuilder builder)
        {
            builder.AddOpenIdConnect(OAuth2AuthenticationDefaults.AuthenticationScheme, options =>
            {
                //configure the OAuth2 Client ID and Client Secret
                var settings = EngineContext.Current.Resolve<OAuth2AuthenticationSettings>();
                string[] scopes = settings.Scopes?.Split(' ') ?? new string[] { };
                foreach (var scope in scopes)
                {
                    options.Scope.Add(scope);
                }
                options.ClientId = settings.ClientId;
                options.ClientSecret = settings.ClientSecret;
                options.ResponseType = "code";
                options.CallbackPath = new PathString(OAuth2AuthenticationDefaults.SigninCallbackPath);
                options.ClaimsIssuer = OAuth2AuthenticationDefaults.ClaimsIssuer;
                options.SaveTokens = true;
                options.Authority = settings.Authority;
                options.SignedOutCallbackPath = new PathString(OAuth2AuthenticationDefaults.SignoutCallbackPath);
                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProviderForSignOut = n =>
                    {
                        var idTokenHint = n.Request.Cookies["stored_id_token"];
                        if (idTokenHint != null)
                            n.ProtocolMessage.IdTokenHint = idTokenHint;

                        return Task.FromResult(0);
                    },
                    OnTokenResponseReceived = n =>
                    {
                        n.Response.Cookies.Append("stored_id_token", n.TokenEndpointResponse.IdToken);
                        return Task.FromResult(0);
                    }
                };
            });
        }
    }
}