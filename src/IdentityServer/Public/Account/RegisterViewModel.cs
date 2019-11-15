using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Public.Account
{
    public class RegisterViewModel
    {
        [Required]
        public string FamilyName { get; set; }

        [Required]
        public string GivenName { get; set; }
        
        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        [MinLength(8)]
        public string ConfirmPassword { get; set; }

        public bool Result { get; set; }
        public string Message { get; set; }
    }
}
