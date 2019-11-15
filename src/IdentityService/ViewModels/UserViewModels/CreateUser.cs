using System.ComponentModel.DataAnnotations;

namespace IdentityService.ViewModels.UserViewModels
{
    /// <summary>
	/// 
	/// </summary>
    public class CreateUser : UserModel
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
