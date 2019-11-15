namespace IdentityService.Configurations
{
    /// <summary>
	/// Strong-Typed Api Authentication Configuration
	/// </summary>
	public class ApiAuthenticationConfiguration
    {

        /// <summary>
        /// Base-address of the token issuer
        /// </summary>
        /// <value></value>
        public string Authority { get; set; }

        /// <summary>
        /// Specifies whether HTTPS is required for the discovery endpoint
        /// </summary>
        /// <value></value>
        public bool RequireHttpsMetadata { get; set; }

        /// <summary>
        /// Name of the API resource used for authentication against introspection endpoint
        /// </summary>
        /// <value></value>
        public string ApiName { get; set; }

        /// <summary>
        /// Email confirmation URL template
        /// </summary>
        /// <value></value>
        public string ConfirmEmailUrlTemplate { get; set; }
    }
}
