using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;

namespace Nop.Plugin.ExternalAuth.OAuth2.Infrastructure
{
    public class OAuthStartup : INopStartup
    {
        public int Order => 101112;

        public void Configure(IApplicationBuilder application)
        {
            application.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

        }
    }
}