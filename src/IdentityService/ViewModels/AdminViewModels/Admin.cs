using System;

namespace IdentityService.ViewModels.AdminViewModels
{
    public class Admin : UserModel
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
