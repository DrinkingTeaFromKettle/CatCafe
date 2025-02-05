using CatCafe.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace CatCafe.DataModels
{
    public class AdoptionInquiry : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid CatId { get; set; }
        public Cat Cat { get; set; } = null!;
        [StringLength(1000, ErrorMessage = "Maximal {0} lenght is {1}.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Inquiry {0} is required.")]
        public InquiryStatus Status { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public DateTime DateOfAdoption { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }
        [DataType(DataType.Date)]
        public DateTime LastUpdated { get; set; }
    }
}
