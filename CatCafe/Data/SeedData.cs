using CatCafe.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CatCafe.Data
{
    public class SeedData
    {
        private static RoleManager<IdentityRole<Guid>>? _roleManager;
        private static UserManager<ApplicationUser>? _userManager;



        public static async Task InitializeAsync(RoleManager<IdentityRole<Guid>> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            if (_roleManager != null)
            {
                foreach(var role in Role.GetValues<Role>())
                {
                    await CreateRole(role.ToString());
                }
            }
            
            _userManager = userManager;
            var applicationUser = new ApplicationUser()
            {
                Email = "admin@example.com",
                UserName = "admin@example.com"
            };
            var result = await _userManager.CreateAsync(applicationUser, "Password!1");
            if (result.Succeeded)
            {
                var role = _roleManager.FindByNameAsync("admin").Result;
                if (role != null)
                {
                    IdentityResult roleresult = await _userManager.AddToRoleAsync(applicationUser, role.Name);
                }
                else
                {
                    throw new Exception("Role " + role.ToString() + " does not exist");
                }
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                await _userManager.ConfirmEmailAsync(applicationUser, code);
            }

        }

        private static async Task CreateRole(string roleName)
        {
            if (await _roleManager.FindByNameAsync(roleName) == null)
            {
                var results = await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            }
        }
    }
}
