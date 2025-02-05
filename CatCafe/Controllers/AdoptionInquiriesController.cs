using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CatCafe.Data;
using CatCafe.DataModels;
using CatCafe.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Data;
using Microsoft.AspNetCore.Authorization;


namespace CatCafe.Controllers
{
    public class AdoptionInquiriesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CatCafeDbContext _context;

        public AdoptionInquiriesController(CatCafeDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Employee, Admin")]
        // GET: AdoptionInquiries
        public async Task<IActionResult> Index()
        {
            var catCafeDbContext = _context.AdoptionInquiry.Include(a => a.User);
            return View(await catCafeDbContext.ToListAsync());
        }

        [Authorize(Roles = "Employee, Admin")]
        // GET: AdoptionInquiries/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adoptionInquiry = await _context.AdoptionInquiry
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adoptionInquiry == null)
            {
                return NotFound();
            }

            return View(adoptionInquiry);
        }

        [Authorize]
        // GET: AdoptionInquiries/Create
        public async Task<IActionResult> Create([FromRoute] Guid? id)
        {
            var cat = await _context.Cats.FindAsync(id);
            if(cat.Adoptable == false)
            {
                return NotFound();
            }
            return View();
        }

        // POST: AdoptionInquiries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] AdoptionInquiryViewModel adoptionInquiryViewModel, [FromRoute] Guid Id)
        {
            if (ModelState.IsValid)
            {
                string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if(userId == null)
                {
                    throw new Exception("No user logged in");
                }
                var applicationUser = await _userManager.FindByIdAsync(userId);
                if (applicationUser == null)
                {
                    throw new Exception("No user with id " + userId);
                }
                var address = new Address()
                {
                    Country = adoptionInquiryViewModel.Address.Country,
                    CountryCode = "aaa",
                    Region = adoptionInquiryViewModel.Address.Region,
                    City = adoptionInquiryViewModel.Address.City,
                    PostalCode = adoptionInquiryViewModel.Address.PostalCode,
                    StreetName = adoptionInquiryViewModel.Address.StreetName,
                    BuildingNumber = adoptionInquiryViewModel.Address.BuildingNumber,
                    ApartmentNumber = adoptionInquiryViewModel.Address.ApartmentNumber,
                    User = applicationUser,
                    UserId = applicationUser.Id
                };
                if (Id == null)
                {
                    NotFound();
                }
                var cat = _context.Cats.FirstOrDefault(cat => cat.Id == Id);
                if (cat == null)
                {
                    NotFound();
                }
                var adoptionInquiry = new AdoptionInquiry()
                {
                    Id = new Guid(),
                    Cat = cat,
                    CatId = Id,
                    User = applicationUser,
                    UserId = applicationUser.Id,
                    Description = adoptionInquiryViewModel.Description,
                    Status = InquiryStatus.None

                };
                _context.Add(address);
                _context.Add(adoptionInquiry);
                await _context.SaveChangesAsync();
                applicationUser.Name = adoptionInquiryViewModel.Name;
                applicationUser.SurName = adoptionInquiryViewModel.SurName;
                await _userManager.UpdateAsync(applicationUser);
                return RedirectToAction(nameof(Index));
            }
            return View(adoptionInquiryViewModel);
        }

        [Authorize(Roles = "Employee, Admin")]
        // GET: AdoptionInquiries/Edit/5
        public async Task<IActionResult> Edit([FromRoute] Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adoptionInquiry = await _context.AdoptionInquiry.FindAsync(id);
            if (adoptionInquiry == null)
            {
                return NotFound();
            }
            var inputModel = new AdoptionInquiryEditInputViewModel()
            {
                Id = (Guid)id,
                Description = adoptionInquiry.Description,
                Status = adoptionInquiry.Status,
            };
            var model = new AdoptionInquiryEditViewModel()
            {
                
                Input = inputModel,
                StatusList = Enum.GetValues(typeof(InquiryStatus)).Cast<InquiryStatus>().Select(m => new SelectListItem
                {
                    Text = m.ToString(),
                    Value = m.ToString()
                }).ToList()
            };

            return View(model);
        }

        // POST: AdoptionInquiries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Employee, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] Guid id, [FromForm] AdoptionInquiryEditInputViewModel Input)
        {
            if (id != Input.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var adoptonInquiry = await _context.AdoptionInquiry.FindAsync(id);
                adoptonInquiry.Description = Input.Description;
                adoptonInquiry.Status = Input.Status;
                if(Input.Status == InquiryStatus.Finished)
                {
                    adoptonInquiry.DateOfAdoption = DateTime.Now;
                }
                try
                {
                    _context.Update(adoptonInquiry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdoptionInquiryExists(Input.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var model = new AdoptionInquiryEditViewModel()
            {
                Input = Input,
                StatusList = Enum.GetValues(typeof(InquiryStatus)).Cast<InquiryStatus>().Select(m => new SelectListItem
                {
                    Text = m.ToString(),
                    Value = m.ToString()
                }).ToList()
            };
            return View(model);
        }

        [Authorize(Roles = "Employee, Admin")]
        // GET: AdoptionInquiries/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adoptionInquiry = await _context.AdoptionInquiry
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adoptionInquiry == null)
            {
                return NotFound();
            }

            return View(adoptionInquiry);
        }

        [Authorize(Roles = "Employee, Admin")]
        // POST: AdoptionInquiries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var adoptionInquiry = await _context.AdoptionInquiry.FindAsync(id);
            if (adoptionInquiry != null)
            {
                _context.AdoptionInquiry.Remove(adoptionInquiry);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Employee, Admin")]
        private bool AdoptionInquiryExists(Guid id)
        {
            return _context.AdoptionInquiry.Any(e => e.Id == id);
        }
    }
}
