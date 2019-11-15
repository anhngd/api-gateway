using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Public.Account
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public int ConfirmEmail { get; set; }
    }
}
