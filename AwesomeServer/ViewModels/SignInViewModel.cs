using System.ComponentModel.DataAnnotations;
using AwesomeServer.Resources;

namespace AwesomeServer.ViewModels
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "EmailAddress")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(DataAnnotations))]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(DataAnnotations))]
        public bool RememberMe { get; set; }
    }
}