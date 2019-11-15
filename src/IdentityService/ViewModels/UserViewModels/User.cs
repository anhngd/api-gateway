using System;

namespace IdentityService.ViewModels.UserViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class User : UserModel
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
