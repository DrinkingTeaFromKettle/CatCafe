using CatCafe.DataModels;
using Grpc.Core;
using System.ComponentModel.DataAnnotations;

namespace CatCafe.ViewModels
{
    public class AdoptionInquiryEditInputViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public InquiryStatus Status { get; set; }
    }
}
