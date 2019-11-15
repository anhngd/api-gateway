using System.ComponentModel.DataAnnotations;

namespace IdentityService.ViewModels.AccountViewModels
{
    public class CreateAccount : Account
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
