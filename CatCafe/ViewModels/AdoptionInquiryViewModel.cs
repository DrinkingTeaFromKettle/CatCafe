using CatCafe.DataModels;
using System.ComponentModel.DataAnnotations;

namespace CatCafe.ViewModels
{
    public class AdoptionInquiryViewModel
    {
        /*public Guid CatId { get; set; }*/
        /* public Guid UserId { get; set; }*/
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string SurName { get; set; }
        public AddressViewModel Address { get; set; }
    }
}
