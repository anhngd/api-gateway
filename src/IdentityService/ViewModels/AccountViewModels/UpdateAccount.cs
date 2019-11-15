using System.ComponentModel.DataAnnotations;

namespace IdentityService.ViewModels.AccountViewModels
{
    /// <summary>
	/// 
	/// </summary>
    public class UpdateAccount
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
        public virtual string PhoneNumber { get; set; }
    }
}
