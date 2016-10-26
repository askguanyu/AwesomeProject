using System.ComponentModel.DataAnnotations;
using AwesomeAPI.Resources;

namespace AwesomeAPI.ViewModels
{
    public abstract class ViewModelBase
    {
        [Display(Name = "Id", ResourceType = typeof(DataAnnotations))]
        public int Id { get; set; }
    }
}