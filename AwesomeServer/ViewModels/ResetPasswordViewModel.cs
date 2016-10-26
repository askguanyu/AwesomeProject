using System.ComponentModel.DataAnnotations;
using AwesomeServer.Resources;

namespace AwesomeServer.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Required")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "StringLength")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(DataAnnotations))]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Compare")]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(DataAnnotations))]
        public string ConfirmPassword { get; set; }

        // UserId and Code are not shown in the View
        public string UserId { get; set; }
        public string Code { get; set; }
    }
}