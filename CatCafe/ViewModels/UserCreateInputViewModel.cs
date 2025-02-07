using CatCafe.DataModels;
using CatCafe.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CatCafe.ViewModels
{
    public class UserCreateInputViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        public List<Role> Role { get; set; }
    }
}
