using System;

namespace IdentityService.ViewModels.AdminViewModels
{
    /// <summary>
	/// 
	/// </summary>
    public class LockRequestAdmin
    {
        /// <summary>
		/// The date and time, in ISO 8601 format, when any user lockout ends.
		/// </summary>
		/// <remarks>
		/// A value in the past means the user is not locked out.
		/// </remarks>
		/// <value></value>
		public DateTime? End { get; set; }
    }
}
