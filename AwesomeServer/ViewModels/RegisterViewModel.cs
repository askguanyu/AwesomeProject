using System.ComponentModel.DataAnnotations;
using AwesomeServer.Resources;

namespace AwesomeServer.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "EmailAddress")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "StringLength")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(DataAnnotations))]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Compare")]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(DataAnnotations))]
        public string ConfirmPassword { get; set; }
    }
}