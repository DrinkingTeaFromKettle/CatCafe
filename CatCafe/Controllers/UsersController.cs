using CatCafe.Areas.Identity.Pages.Account;
using CatCafe.DataModels;
using CatCafe.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace CatCafe.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;

        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IUserStore<IdentityUser> userStore)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userStore = userStore;
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            _emailStore = (IUserEmailStore<IdentityUser>)_userStore;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserDetailsViewModel>();
            foreach (IdentityUser user in users)
            {
                var thisViewModel = new UserDetailsViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.EmailConfirmed = user.EmailConfirmed;
                thisViewModel.Roles = await _userManager.GetRolesAsync(user);
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            var thisViewModel = new UserDetailsViewModel();
            thisViewModel.UserId = user.Id;
            thisViewModel.Email = user.Email;
            thisViewModel.EmailConfirmed = user.EmailConfirmed;
            thisViewModel.Roles = await _userManager.GetRolesAsync(user);
            return View(thisViewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var model = new UserCreateViewModel
            {
                Roles = Enum.GetValues(typeof(Role)).Cast<Role>().Select(m => new SelectListItem
                {
                    Text = m.ToString(),
                    Value = m.ToString()
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] UserCreateInputViewModel user)
        {
            if (ModelState.IsValid)
            {
                var identityUser = new IdentityUser();
                await _userStore.SetUserNameAsync(identityUser, user.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(identityUser, user.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(identityUser, user.Password);

                if (result.Succeeded)
                { 
                    foreach(var userRole in user.Role)
                    { 
                        var role = _roleManager.FindByNameAsync(userRole.ToString()).Result;
                        if (role != null)
                        {
                            IdentityResult roleresult = await _userManager.AddToRoleAsync(identityUser, role.Name);
                        }
                        else
                        {
                            throw new Exception("Role "+role.ToString()+" does not exist");
                        }
                    }
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    await _userManager.ConfirmEmailAsync(identityUser, code);
                    
                }
            }
            

            return Redirect("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit([FromRoute] Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            var userRolesString = await _userManager.GetRolesAsync(user);
            var rolesList = new List<Role>();
            foreach (var roleString in userRolesString)
            {
                Enum.TryParse(roleString, out Role role);
                rolesList.Add(role);
            }
            var model = new UserEditViewModel
            {
                User = new UserEditInputViewModel
                {
                    Id = new Guid(user.Id),
                    Email = user.Email,
                    Role = rolesList
                },
                Roles = Enum.GetValues(typeof(Role)).Cast<Role>().Select(m => new SelectListItem
                {
                    Text = m.ToString(),
                    Value = m.ToString()
                }).ToList()
            };
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] Guid id, [FromForm] UserEditInputViewModel user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.FindByIdAsync(id.ToString());
                if (identityUser == null)
                {
                    return NotFound();
                }
                identityUser.Email = user.Email;
                await _userManager.UpdateAsync(identityUser);
                var roles = await _userManager.GetRolesAsync(identityUser);
                await _userManager.RemoveFromRolesAsync(identityUser, roles);
                foreach (var userRole in user.Role)
                {
                    var role = _roleManager.FindByNameAsync(userRole.ToString()).Result;
                    if (role != null)
                    {
                        IdentityResult roleresult = await _userManager.AddToRoleAsync(identityUser, role.Name);
                    }
                    else
                    {
                        throw new Exception("Role " + role.ToString() + " does not exist");
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var model = new UserEditViewModel()
            {
                User = user,
                Roles = Enum.GetValues(typeof(Role)).Cast<Role>().Select(m => new SelectListItem
                {
                    Text = m.ToString(),
                    Value = m.ToString()
                }).ToList()
            };
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            var userDetails = new UserDetailsViewModel
            {
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Roles = await _userManager.GetRolesAsync(user),
            };
            return View(userDetails);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var user = await _userStore.FindByIdAsync(id.ToString(), CancellationToken.None);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(Guid id)
        {
           return  _userStore.FindByIdAsync(id.ToString(), CancellationToken.None) == null ? false : true;
        }
    }
}
