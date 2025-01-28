using CatCafe.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CatCafe.Data
{
    public class SeedData
    {
        private static RoleManager<IdentityRole>? _roleManager;

        public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            if (_roleManager != null)
            {
                foreach(var role in Role.GetValues<Role>())
                {
                    await CreateRole(role.ToString());
                }
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
