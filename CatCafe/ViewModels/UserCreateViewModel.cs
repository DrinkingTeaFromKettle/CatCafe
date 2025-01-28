using CatCafe.Areas.Identity.Pages.Account;
using CatCafe.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CatCafe.ViewModels
{
    public class UserCreateViewModel
    {
        public UserCreateInputViewModel User {  get; set; } = default!;
        public IEnumerable<SelectListItem> Roles { get; set; } = default!;
    }
}
