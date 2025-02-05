using CatCafe.DataModels;
using Grpc.Core;
using System.ComponentModel.DataAnnotations;

namespace CatCafe.ViewModels
{
    public class AdoptionInquiryEditInputViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(1000, ErrorMessage = "Maximal {0} lenght is {1}.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public InquiryStatus Status { get; set; }
    }
}
