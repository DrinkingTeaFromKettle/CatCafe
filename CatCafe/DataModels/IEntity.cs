using System.ComponentModel.DataAnnotations;

namespace CatCafe.DataModels
{
    public interface IEntity
    {
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
