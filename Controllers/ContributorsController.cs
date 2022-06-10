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
using Etaa.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Etaa.Controllers
{
    public class ContributorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContributorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // For the create action to load the states first, then load the cities and districts
        public IActionResult CascadeList()
        {
            try
            {
                ViewBag.States = new SelectList(_context.States, "StateId", "NameAr");
                return View();
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public JsonResult LoadCities(int StateId)
        {
            try
            {
                var City = _context.Cities.Where(City => City.StateId == StateId);
                return Json(new SelectList(City, "CityId", "NameAr"));
            }
            catch (Exception ex)
            {
                return Json(default);
            }
        }

        [HttpGet]
        public JsonResult LoadDistricts(int CityId)
        {
            try
            {
                var District = _context.Districts.Where(District => District.CityId == CityId);
                return Json(new SelectList(District, "DistrictId", "NameAr"));
            }
            catch (Exception ex)
            {
                return Json(default);
            }
        }

        [Authorize]
        // GET: Contributors
        public async Task<IActionResult> Index()
        {
            try
            {
                var applicationDbContext = _context.Contributors.Include(c => c.District);
                return View(await applicationDbContext.ToListAsync());
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: Contributors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
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
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: Contributors/Create
        public IActionResult Create()
        {
            ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "NameAr");
            var StatesList = new SelectList(_context.States.ToList(), "StateId", "NameAr");
            ViewData["States"] = StatesList;
            return View();
        }

        // POST: Contributors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContributorId,NameAr,NameEn,Mobile,WhatsappMobile,Email,Address,StartDate,EndDate,MonthlyShareAmount,NumberOfShares,IsActive,IsCanceled,DistrictId")] Contributor contributor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contributor.IsCanceled = false;
                    _context.Add(contributor);
                    await _context.SaveChangesAsync();
                    TempData["Contributor"] = "Contributor";
                    return RedirectToAction(nameof(Index));
                }
                ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "NameAr", contributor.DistrictId);
                TempData["ContributorError"] = "Contributor";
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
            catch (Exception ex)
            {
                TempData["ContributorError"] = "Contributor";
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
        }

        // GET: Contributors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
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
                var StatesList = new SelectList(_context.States.ToList(), "StateId", "NameAr");
                ViewData["States"] = StatesList;
                return View(contributor);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: Contributors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContributorId,NameAr,NameEn,Mobile,WhatsappMobile,Email,Address,StartDate,EndDate,MonthlyShareAmount,NumberOfShares,IsActive,IsCanceled,DistrictId")] Contributor contributor)
        {
            try
            {
                if (id != contributor.ContributorId)
                {
                    TempData["ContributorError"] = "Contributor";
                    var RedirectURLFirst = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                    return Json(new
                    {
                        redirectUrl = RedirectURLFirst
                    });
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(contributor);
                        await _context.SaveChangesAsync();
                        TempData["Contributor"] = "Contributor";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ContributorExists(contributor.ContributorId))
                        {
                            TempData["ContributorError"] = "Contributor";
                            var RedirectURLSecond = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                            return Json(new
                            {
                                redirectUrl = RedirectURLSecond
                            });
                        }
                        else
                        {
                            TempData["ContributorError"] = "Contributor";
                            var RedirectURLThird = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                            return Json(new
                            {
                                redirectUrl = RedirectURLThird
                            });
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "NameAr", contributor.DistrictId);
                return View("Edit");
            }
            catch (Exception ex)
            {
                TempData["ContributorError"] = "Contributor";
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
        }

        // GET: Contributors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
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
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: Contributors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                TempData["Contributor"] = "Contributor";
                var contributor = await _context.Contributors.FindAsync(id);
                contributor.IsCanceled = true;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ContributorError"] = "Contributor";
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
        }

        private bool ContributorExists(int id)
        {
            try
            {
                return _context.Contributors.Any(e => e.ContributorId == id);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
