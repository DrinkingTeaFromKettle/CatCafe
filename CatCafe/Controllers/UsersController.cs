using CatCafe.Areas.Identity.Pages.Account;
using CatCafe.Data;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly CatCafeDbContext _context;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IUserStore<ApplicationUser> userStore, CatCafeDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userStore = userStore;
            _context = context;
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            _emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserDetailsViewModel>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserDetailsViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.EmailConfirmed = user.EmailConfirmed;
                thisViewModel.Roles = await _userManager.GetRolesAsync(user);
                thisViewModel.Inquiries = _context.AdoptionInquiry.Where(a => a.UserId == user.Id).ToList();
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
            thisViewModel.Inquiries = _context.AdoptionInquiry.Where(a => a.UserId == id).ToList();
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
                var ApplicationUser = new ApplicationUser();
                await _userStore.SetUserNameAsync(ApplicationUser, user.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(ApplicationUser, user.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(ApplicationUser, user.Password);

                if (result.Succeeded)
                { 
                    foreach(var userRole in user.Role)
                    { 
                        var role = _roleManager.FindByNameAsync(userRole.ToString()).Result;
                        if (role != null)
                        {
                            IdentityResult roleresult = await _userManager.AddToRoleAsync(ApplicationUser, role.Name);
                        }
                        else
                        {
                            throw new Exception("Role "+role.ToString()+" does not exist");
                        }
                    }
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(ApplicationUser);
                    await _userManager.ConfirmEmailAsync(ApplicationUser, code);
                    
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
                    Id = user.Id,
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
                var ApplicationUser = await _userManager.FindByIdAsync(id.ToString());
                if (ApplicationUser == null)
                {
                    return NotFound();
                }
                ApplicationUser.Email = user.Email;
                await _userManager.UpdateAsync(ApplicationUser);
                var roles = await _userManager.GetRolesAsync(ApplicationUser);
                await _userManager.RemoveFromRolesAsync(ApplicationUser, roles);
                foreach (var userRole in user.Role)
                {
                    var role = _roleManager.FindByNameAsync(userRole.ToString()).Result;
                    if (role != null)
                    {
                        IdentityResult roleresult = await _userManager.AddToRoleAsync(ApplicationUser, role.Name);
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
                Inquiries = _context.AdoptionInquiry.Where(a => a.UserId == id).ToList()
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
