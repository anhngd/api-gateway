using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Public.Account
{
    public class UpdatePasswordViewModel
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmNewPassword { get; set; }

        public int IsChangePass { get; set; }
    }
}
