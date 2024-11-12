using Microsoft.AspNetCore.Mvc.Rendering;
using CatCafe.DataModels;

namespace CatCafe.ViewModels
{
    public class CatViewModel
    {
        public Cat Cat { get; set; } = default!;
        public IEnumerable<SelectListItem> StatusList { get; set; } = default!;
    }
}
