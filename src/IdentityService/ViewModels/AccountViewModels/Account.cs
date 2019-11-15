using System;

namespace IdentityService.ViewModels.AccountViewModels
{
    public class Account : UserModel
    {
        /// <summary>
        /// Role
        /// </summary>
        /// <value></value>
        public virtual string Role { get; set; }

        /// <summary>
		/// 
		/// </summary>
		/// <value></value>
		public virtual DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public virtual DateTimeOffset? CreatedAt { get; set; }
    }
}
