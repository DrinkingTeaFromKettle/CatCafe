using System.ComponentModel.DataAnnotations;

namespace CatCafe.DataModels
{
    public class Cat : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Cat's name is required")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Cat's age is required")]
        [Range(0, 50)]
        public int Age { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Information, whether this cat is available for adoption is required")]
        public bool Adoptable { get; set; }
        [Required(ErrorMessage = "Cat's status is required")]
        [EnumDataType(typeof(CatStatus),ErrorMessage = "Invalid status value" )]
        public CatStatus Status { get; set; }
        [Required(ErrorMessage = "Date of animal's arrival on site is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfArrival { get; set; }
        [Required(ErrorMessage = "Date of acquisition is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfAcquisition { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }
        [DataType(DataType.Date)]
        public DateTime LastUpdated { get; set; }

    }
}
