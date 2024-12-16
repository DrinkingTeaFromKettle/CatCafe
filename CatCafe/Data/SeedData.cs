using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CatCafe.Data
{
    public class SeedData
    {
        private static RoleManager<IdentityRole>? _roleManager;

        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (_roleManager != null)
            {
                await CreateRole("Admin");
                await CreateRole("Employee");
                await CreateRole("User");
            }
        }

        private static async Task CreateRole(string roleName)
        {
            if (await _roleManager.FindByNameAsync(roleName) == null)
            {
                var results = await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
