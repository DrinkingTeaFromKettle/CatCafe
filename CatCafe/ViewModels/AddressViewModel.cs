using System.ComponentModel.DataAnnotations;

namespace CatCafe.ViewModels
{
    public class AddressViewModel
    {
        public string Country { get; set; }
        [Required(ErrorMessage = "City name is required")]
        [StringLength(100)]
        public string City { get; set; }
        [Required(ErrorMessage = "Region is required")]
        [StringLength(50)]
        public string Region { get; set; }
        [Required(ErrorMessage = "Postal code is required")]
        [StringLength(10)]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "Street name is required")]
        [StringLength(100)]
        public string StreetName { get; set; }
        [Required(ErrorMessage = "Building number is required")]
        [StringLength(10)]
        public string BuildingNumber { get; set; }
        [StringLength(10)]
        public string? ApartmentNumber { get; set; }
    }
}
