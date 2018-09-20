using System.ComponentModel.DataAnnotations;

namespace SmartMoveWebApp.Dtos
{
    public class AccountLoginDto
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string AccountType { get; set; }
    }
}