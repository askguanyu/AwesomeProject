using System.ComponentModel.DataAnnotations;
using AwesomeAPI.Resources;

namespace AwesomeAPI.ViewModels
{
    public class ActivityTypeViewModel : ViewModelBase
    {
        [Required(ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "StringLengthMax")]
        [Display(Name = "Description", ResourceType = typeof(DataAnnotations))]
        public string Description { get; set; }
    }
}