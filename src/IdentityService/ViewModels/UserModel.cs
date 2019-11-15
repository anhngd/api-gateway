using System;
using Newtonsoft.Json;

namespace IdentityService.ViewModels
{
    /// <summary>
	/// 
	/// </summary>
	public class UserModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public virtual string FamilyName { get; set; }

        /// <summary>
		/// 
		/// </summary>
		/// <value></value>
        public virtual string GivenName { get; set; }

        /// <summary>
		/// 
		/// </summary>
		/// <value></value>
        public virtual string Gender { get; set; }

        /// <summary>
		/// 
		/// </summary>
		/// <value></value>
        public virtual string Picture { get; set; }

        /// <summary>
		/// 
		/// </summary>
		/// <value></value>
        public virtual string BirthDate { get; set; }

        /// <summary>
		/// 
		/// </summary>
		/// <value></value>
        public virtual string ZoneInfo { get; set; }

        /// <summary>
		/// 
		/// </summary>
		/// <value></value>
        public virtual string Locale { get; set; }

        /// <summary>
		/// 
		/// </summary>
		/// <value></value>
        public virtual string Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public virtual bool PhoneNumberConfirmed { get; set; }

        /// <summary>
		/// 
		/// </summary>
		/// <value></value>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
		/// 
		/// </summary>
		/// <value></value>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
		/// 
		/// </summary>
		/// <value></value>
        public virtual string Email { get; set; }

        /// <summary>
		/// 
		/// </summary>
		/// <value></value>
        public virtual string UserName { get; set; }

        /// <summary>
		/// 
		/// </summary>
		/// <value></value>
        public virtual string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public virtual bool Lockedout { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
