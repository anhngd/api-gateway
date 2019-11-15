using System.ComponentModel.DataAnnotations;

namespace IdentityService.ViewModels.UserViewModels
{
    /// <summary>
	/// 
	/// </summary>
	public class UpdatePassword
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string CurrentPassword { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        [Required]
        public string NewPassword { get; set; }
    }
}
