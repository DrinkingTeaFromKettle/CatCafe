﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CatCafe.Data;
using CatCafe.DataModels;
using CatCafe.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CatCafe.Controllers
{
    public class CatsController : Controller
    {
        private readonly CatCafeDbContext _context;


        public CatsController(CatCafeDbContext context)
        {
            _context = context;
        }


        // GET: Cats
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cats.ToListAsync());
        }

        // GET: Cats/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat = await _context.Cats
                .Include(a => a.AdoptionInquiries)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cat == null)
            {
                return NotFound();
            }

            return View(cat);
        }

        // GET: Cats/Create
        [Authorize(Roles = "Employee, Admin")]
        public IActionResult Create()
        {
            var model = new CatViewModel
            {
                StatusList = Enum.GetValues(typeof(CatStatus)).Cast<CatStatus>().Select(m => new SelectListItem
                {
                    Text = m.ToString(),
                    Value = m.ToString()
                }).ToList()
            };
            return View(model);
        }

        // POST: Cats/Create
        [HttpPost]
        [Authorize(Roles = "Employee, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Cat cat)
        {
            if (ModelState.IsValid)
            {
                cat.Id = Guid.NewGuid();
                _context.Add(cat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cat);
        }

        // GET: Cats/Edit/5
        [Authorize(Roles = "Employee, Admin")]
        public async Task<IActionResult> Edit([FromRoute] Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat = await _context.Cats.FindAsync(id);
            if (cat == null)
            {
                return NotFound();
            }
            var model = new CatViewModel
            {
                Cat = cat,
                StatusList = Enum.GetValues(typeof(CatStatus)).Cast<CatStatus>().Select(m => new SelectListItem
                {
                    Text = m.ToString(),
                    Value = m.ToString(),
                    Selected = m == cat.Status
                })
            };
            return View(model);
        }

        // POST: Cats/Edit/5
        [HttpPost]
        [Authorize(Roles = "Employee, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] Guid id, [FromForm] Cat cat)
        {
            if (id != cat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatExists(cat.Id))
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
            return View(cat);
        }

        // GET: Cats/Delete/5
        [Authorize(Roles = "Employee, Admin")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat = await _context.Cats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cat == null)
            {
                return NotFound();
            }

            return View(cat);
        }

        // POST: Cats/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Employee, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cat = await _context.Cats.FindAsync(id);
            if (cat != null)
            {
                _context.Cats.Remove(cat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatExists(Guid id)
        {
            return _context.Cats.Any(e => e.Id == id);
        }
    }
}
