using System.ComponentModel.DataAnnotations;

namespace IdentityService.ViewModels.AdminViewModels
{
    /// <summary>
	/// 
	/// </summary>
    public class CreateAdmin : Admin
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        [Required]
        public override string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        [Required]
        public virtual string Password { get; set; }
    }
}
