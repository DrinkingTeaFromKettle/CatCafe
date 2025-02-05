using System.ComponentModel.DataAnnotations;

namespace CatCafe.DataModels
{
    public class Address
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Country name is required")]
        [StringLength(50)]
        public string Country { get; set; }
        [StringLength(3)]
        public string CountryCode { get; set; }
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
        [StringLength(100, ErrorMessage = "{0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string StreetName { get; set; }
        [Required(ErrorMessage = "Building number is required")]
        [StringLength(10, ErrorMessage = "{0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string BuildingNumber { get; set; }
        [StringLength(10, ErrorMessage = "{0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string? ApartmentNumber { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}
