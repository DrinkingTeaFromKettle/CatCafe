using CatCafe.DataModels;
using System.ComponentModel.DataAnnotations;

namespace CatCafe.ViewModels
{
    public class AdoptionInquiryViewModel
    {
        [StringLength(1000, ErrorMessage = "Maximal {0} lenght is {1}.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(50, ErrorMessage = "User's {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(50, ErrorMessage = "User's {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string SurName { get; set; }
        public AddressViewModel Address { get; set; }
    }
}
