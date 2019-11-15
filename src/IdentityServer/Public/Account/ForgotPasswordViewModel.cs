using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Public.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        public string Email { get; set; }
        
        public int IsEmailSent { get; set; }
    }
}