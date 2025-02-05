using CatCafe.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace CatCafe.DataModels
{
    public class AdoptionInquiry : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CatId { get; set; }
        public Cat Cat { get; set; } = null!;
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public InquiryStatus Status { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public DateTime DateOfAdoption { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }
        [DataType(DataType.Date)]
        public DateTime LastUpdated { get; set; }
    }
}
