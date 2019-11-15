using System.ComponentModel.DataAnnotations;

namespace IdentityService.ViewModels.AdminViewModels
{
    /// <summary>
	/// 
	/// </summary>
    public class UpdateAdmin
    {
        /// <summary>
		/// 
		/// </summary>
		/// <value></value>
		public string FamilyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        [Required]
        public string GivenName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Gender { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string BirthDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string ZoneInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Locale { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string PhoneNumber { get; set; }
    }
}
