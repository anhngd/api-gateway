using System.ComponentModel.DataAnnotations;

namespace IdentityService.ViewModels.SignUpViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class SignUp
    {
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
        [Required]
        public string FamilyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        [Required]
        public string Email { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <value></value>
        //[Required]
        //public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        [Required]
        public string Password { get; set; }
    }
}
