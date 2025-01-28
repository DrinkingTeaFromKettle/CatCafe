using CatCafe.DataModels;

namespace CatCafe.ViewModels
{
    public class UserDetailsViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
