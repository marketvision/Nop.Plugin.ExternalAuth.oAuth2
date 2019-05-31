using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Nop.Plugin.ExternalAuth.OAuth2.Models
{
    /// <summary>
    /// Represents a configuration model
    /// </summary>
    public class ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.ExternalAuth.OAuth2.ClientId")]
        public string ClientId { get; set; }

        [NopResourceDisplayName("Plugins.ExternalAuth.OAuth2.ClientSecret")]
        [DataType(DataType.Password)]
        [NoTrim]
        public string ClientSecret { get; set; }

        [NopResourceDisplayName("Plugins.ExternalAuth.OAuth2.Authority")]
        public string Authority { get; set; }

        [NopResourceDisplayName("Plugins.ExternalAuth.OAuth2.Scopes")]
        public string Scopes { get; set; }

        [NopResourceDisplayName("Plugins.ExternalAuth.OAuth2.AdministratorsRoles")]
        public string AdministratorsRoles { get; set; }
    }
}