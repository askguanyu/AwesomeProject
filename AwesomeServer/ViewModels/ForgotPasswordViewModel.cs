using System.ComponentModel.DataAnnotations;

namespace AwesomeServer.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "EmailAddress")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}