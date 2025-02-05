using Microsoft.AspNetCore.Mvc.Rendering;

namespace CatCafe.ViewModels
{
    public class AdoptionInquiryEditViewModel
    {
        public AdoptionInquiryEditInputViewModel Input { get; set; } = default!;
        public IEnumerable<SelectListItem> StatusList { get; set; } = default!;
    }
}
