using System;
using System.ComponentModel.DataAnnotations;
using AwesomeAPI.Resources;

namespace AwesomeAPI.ViewModels
{
    public class ActivityViewModel : ViewModelBase
    {
        [Required(ErrorMessage = "Required")]
        [Display(Name = "ActivityTypeId", ResourceType = typeof(DataAnnotations))]
        public int? ActivityTypeId { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "StringLengthMax")]
        [Display(Name = "Subject", ResourceType = typeof(DataAnnotations))]
        public string Subject { get; set; }

        [StringLength(255, ErrorMessage = "StringLengthMax")]
        [Display(Name = "Details", ResourceType = typeof(DataAnnotations))]
        public string Details { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Date)]
        [Display(Name = "Start", ResourceType = typeof(DataAnnotations))]
        public DateTime? Start { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "End", ResourceType = typeof(DataAnnotations))]
        public DateTime? End { get; set; }
    }
}