using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CatCafe.DataModels
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [StringLength(50)]
        public string? Name { get; set; }
        [StringLength(50)]
        public string? SurName { get; set; }
        public Address? Address { get; set; }
    }
}
