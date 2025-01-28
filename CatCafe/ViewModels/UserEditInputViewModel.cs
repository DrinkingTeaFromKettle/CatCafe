using CatCafe.DataModels;
using System.ComponentModel.DataAnnotations;

namespace CatCafe.ViewModels
{
    public class UserEditInputViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public List<Role> Role { get; set; }
    }
}
