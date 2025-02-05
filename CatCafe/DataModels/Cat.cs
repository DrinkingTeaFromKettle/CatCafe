using System.ComponentModel.DataAnnotations;

namespace CatCafe.DataModels
{
    public class Cat : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Cat's name is required")]
        [StringLength(50, ErrorMessage = "Cat's {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Cat's age is required")]
        [Range(0, 50, ErrorMessage = "Cat's {0} must be between {1} and {2} years old.")]
        public int Age { get; set; }
        [StringLength(1000, ErrorMessage ="Maximal {0} lenght is {1}.")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Information, whether this cat is available for adoption is required")]
        public bool Adoptable { get; set; }
        [Required(ErrorMessage = "Cat's {0} is required")]
        [EnumDataType(typeof(CatStatus),ErrorMessage = "Invalid {0} value" )]
        public CatStatus Status { get; set; }
        [Required(ErrorMessage = "{0} on site is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfArrival { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfAcquisition { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }
        [DataType(DataType.Date)]
        public DateTime LastUpdated { get; set; }
        public ICollection<AdoptionInquiry>? AdoptionInquiries { get; set; } = new List<AdoptionInquiry>();
    }
}
