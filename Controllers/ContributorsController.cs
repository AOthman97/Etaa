#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Etaa.Data;
using Etaa.Models;

namespace Etaa.Controllers
{
    public class ContributorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContributorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contributors
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Contributors.Include(c => c.District);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Contributors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contributor = await _context.Contributors
                .Include(c => c.District)
                .FirstOrDefaultAsync(m => m.ContributorId == id);
            if (contributor == null)
            {
                return NotFound();
            }

            return View(contributor);
        }

        // GET: Contributors/Create
        public IActionResult Create()
        {
            ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "NameAr");
            return View();
        }

        // POST: Contributors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContributorId,NameAr,NameEn,Mobile,WhatsappMobile,Email,Address,StartDate,EndDate,MonthlyShareAmount,NumberOfShares,IsActive,IsCanceled,DistrictId")] Contributor contributor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contributor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "NameAr", contributor.DistrictId);
            return View(contributor);
        }

        // GET: Contributors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contributor = await _context.Contributors.FindAsync(id);
            if (contributor == null)
            {
                return NotFound();
            }
            ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "NameAr", contributor.DistrictId);
            return View(contributor);
        }

        // POST: Contributors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContributorId,NameAr,NameEn,Mobile,WhatsappMobile,Email,Address,StartDate,EndDate,MonthlyShareAmount,NumberOfShares,IsActive,IsCanceled,DistrictId")] Contributor contributor)
        {
            if (id != contributor.ContributorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contributor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContributorExists(contributor.ContributorId))
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
            ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "NameAr", contributor.DistrictId);
            return View(contributor);
        }

        // GET: Contributors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contributor = await _context.Contributors
                .Include(c => c.District)
                .FirstOrDefaultAsync(m => m.ContributorId == id);
            if (contributor == null)
            {
                return NotFound();
            }

            return View(contributor);
        }

        // POST: Contributors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contributor = await _context.Contributors.FindAsync(id);
            _context.Contributors.Remove(contributor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContributorExists(int id)
        {
            return _context.Contributors.Any(e => e.ContributorId == id);
        }
    }
}
