using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CatCafe.DataModels
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [StringLength(50, ErrorMessage = "User's {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string? Name { get; set; }
        [StringLength(50, ErrorMessage = "User's {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string? SurName { get; set; }
        public Address? Address { get; set; }
        public ICollection<AdoptionInquiry>? AdoptionInquiries { get; set; } = new List<AdoptionInquiry>();
    }
}
