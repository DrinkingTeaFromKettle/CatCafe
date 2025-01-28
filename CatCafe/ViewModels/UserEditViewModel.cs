using Microsoft.AspNetCore.Mvc.Rendering;

namespace CatCafe.ViewModels
{
    public class UserEditViewModel
    {
        public UserEditInputViewModel User { get; set; } = default!;
        public IEnumerable<SelectListItem> Roles { get; set; } = default!;
    }
}
