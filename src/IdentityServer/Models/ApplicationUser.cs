using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// User's family name
        /// </summary>
        /// <value></value>
        public string FamilyName { get; set; }

        /// <summary>
        /// User's given name
        /// </summary>
        /// <value></value>
        public string GivenName { get; set; }

        /// <summary>
        /// gender
        /// </summary>
        /// <value></value>
        public string Gender { get; set; }

        /// <summary>
        /// User picture
        /// </summary>
        /// <value></value>
        public string Picture { get; set; }

        /// <summary>
        /// Date of birth
        /// </summary>
        /// <value></value>
        public string BirthDate { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        /// <value></value>
        public string ZoneInfo { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        /// <value></value>
        public string Locale { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        /// <value></value>
        public string Address { get; set; }

        /// <summary>
        /// Role
        /// </summary>
        /// <value></value>
        public string RoleId { get; set; }

        public virtual IdentityRole Role { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        /// <value></value>
        public DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        /// <value></value>
        public DateTimeOffset? CreatedAt { get; set; }

        //public bool? IsEnabled { get; set; } = true;

        /// <summary>
        /// User roles
        /// </summary>
        /// <remark>
        /// User roles to query users
        /// </remark>
        /// <value></value>
        public ICollection<IdentityUserRole<string>> Roles { get; set; }
    }
}
