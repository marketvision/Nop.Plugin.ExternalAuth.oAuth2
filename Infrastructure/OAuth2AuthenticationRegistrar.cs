using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
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
            builder.AddOpenIdConnect(OAuth2AuthenticationDefaults.AuthenticationScheme, o =>
            {
                //configure the OAuth2 Client ID and Client Secret
                var settings = EngineContext.Current.Resolve<OAuth2AuthenticationSettings>();
                string[] scopes = settings.Scopes?.Split(' ') ?? new string[] { };
                foreach (var scope in scopes)
                {
                    o.Scope.Add(scope);
                }
                o.ClientId = settings.ClientId;
                o.ClientSecret = settings.ClientSecret;
                o.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                o.CallbackPath = new PathString(OAuth2AuthenticationDefaults.SigninCallbackPath);
                o.ClaimsIssuer = OAuth2AuthenticationDefaults.ClaimsIssuer;
                o.SaveTokens = true;
                o.Authority = settings.Authority;
                o.SignedOutCallbackPath = new PathString(OAuth2AuthenticationDefaults.SignoutCallbackPath);
                o.GetClaimsFromUserInfoEndpoint = true;
                o.ClaimActions.MapJsonKey(ClaimTypes.Email, "display_email");
                o.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
                o.ClaimActions.MapJsonKey(ClaimTypes.Name, "display_name");
                o.ClaimActions.MapJsonKey(ClaimTypes.Uri, "picture");
                o.Events = new OpenIdConnectEvents
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
                    },
                    OnUserInformationReceived = n =>
                    {
                        return Task.FromResult(0);
                    }
                };
            });
        }
    }
}