﻿#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Etaa.Data;
using Etaa.Models;
using System.Data;
using Etaa.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Etaa.Controllers
{
    public class FamiliesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FamiliesController(ApplicationDbContext context)
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
        // GET: Families
        public async Task<IActionResult> Index()
        {
            try
            {
                // .Include(f => f.IdentityUser)
                // .Include(f => f.AccommodationType).Include(f => f.District).Include(f => f.EducationalStatus).Include(f => f.Gender).Include(f => f.HealthStatus).Include(f => f.InvestmentType).Include(f => f.Job).Include(f => f.MartialStatus).Include(f => f.Religion)
                var applicationDbContext = _context.Families;
                return View(await applicationDbContext.ToListAsync());
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: Families/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var family = await _context.Families
                    .Include(f => f.AccommodationType)
                    .Include(f => f.District)
                    .Include(f => f.EducationalStatus)
                    .Include(f => f.Gender)
                    .Include(f => f.HealthStatus)
                    .Include(f => f.InvestmentType)
                    .Include(f => f.Job)
                    .Include(f => f.MartialStatus)
                    .Include(f => f.Religion)
                    .FirstOrDefaultAsync(m => m.FamilyId == id);
                if (family == null)
                {
                    return NotFound();
                }

                return View(family);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: Families/Create
        public IActionResult Create()
        {
            try
            {
                ViewData["AccommodationTypeId"] = new SelectList(_context.AccommodationTypes, "AccommodationTypeId", "NameAr");
                ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "NameAr");
                ViewData["EducationalStatusId"] = new SelectList(_context.EducationalStatuses, "EducationalStatusId", "NameAr");
                ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "NameAr");
                ViewData["HealthStatusId"] = new SelectList(_context.HealthStatuses, "HealthStatusId", "NameAr");
                ViewData["InvestmentTypeId"] = new SelectList(_context.InvestmentTypes, "InvestmentTypeId", "NameAr");
                ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "NameAr");
                ViewData["MartialStatusId"] = new SelectList(_context.MartialStatuses, "MartialStatusId", "NameAr");
                ViewData["ReligionId"] = new SelectList(_context.Religions, "ReligionId", "NameAr");

                var StatesList = new SelectList(_context.States.ToList(), "StateId", "NameAr");
                ViewData["States"] = StatesList;
                return View();
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: Families/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // ,UserId,ManagementUserId
        public async Task<IActionResult> Create([Bind("FamilyId,NameAr,NameEn,Address,HouseNumber,Alleyway,ResidentialSquare,FirstPhoneNumber,SecondPhoneNumber,NationalNumber,PassportNumber,NumberOfIndividuals,Age,MonthlyIncome,IsCurrentInvestmentProject,IsApprovedByManagement,IsCanceled,DateOfBirth,DistrictId,GenderId,ReligionId,MartialStatusId,JobId,HealthStatusId,EducationalStatusId,AccommodationTypeId,InvestmentTypeId")] Family family)
        {
            try
            {
                var userId = User.GetLoggedInUserId<string>();
                TempData["Family"] = family.NameAr;
                if (ModelState.IsValid)
                {
                    family.IsCanceled = false;
                    family.IsApprovedByManagement = false;
                    family.UserId = userId;
                    _context.Add(family);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["AccommodationTypeId"] = new SelectList(_context.AccommodationTypes, "AccommodationTypeId", "NameAr", family.AccommodationTypeId);
                    ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "NameAr", family.DistrictId);
                    ViewData["EducationalStatusId"] = new SelectList(_context.EducationalStatuses, "EducationalStatusId", "NameAr", family.EducationalStatusId);
                    ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "NameAr", family.GenderId);
                    ViewData["HealthStatusId"] = new SelectList(_context.HealthStatuses, "HealthStatusId", "NameAr", family.HealthStatusId);
                    ViewData["InvestmentTypeId"] = new SelectList(_context.InvestmentTypes, "InvestmentTypeId", "NameAr", family.InvestmentTypeId);
                    ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "NameAr", family.JobId);
                    ViewData["MartialStatusId"] = new SelectList(_context.MartialStatuses, "MartialStatusId", "NameAr", family.MartialStatusId);
                    ViewData["ReligionId"] = new SelectList(_context.Religions, "ReligionId", "NameAr", family.ReligionId);
                    var RedirectURL = Url.Action(nameof(Create), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                    return Json(new
                    {
                        redirectUrl = RedirectURL
                    });
                }
            }
            catch (Exception ex)
            {
                TempData["FamilyError"] = family.NameAr;
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
        }

        // GET: Families/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var family = await _context.Families.FindAsync(id);
                ViewData["FamilyMembers"] = await _context.FamilyMembers.Where(f => f.FamilyId == id).ToListAsync();
                if (family == null)
                {
                    return NotFound();
                }
                ViewData["AccommodationTypeId"] = new SelectList(_context.AccommodationTypes, "AccommodationTypeId", "NameAr", family.AccommodationTypeId);
                ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "NameAr", family.DistrictId);
                ViewData["EducationalStatusId"] = new SelectList(_context.EducationalStatuses, "EducationalStatusId", "NameAr", family.EducationalStatusId);
                ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "NameAr", family.GenderId);
                ViewData["HealthStatusId"] = new SelectList(_context.HealthStatuses, "HealthStatusId", "NameAr", family.HealthStatusId);
                ViewData["InvestmentTypeId"] = new SelectList(_context.InvestmentTypes, "InvestmentTypeId", "NameAr", family.InvestmentTypeId);
                ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "NameAr", family.JobId);
                ViewData["MartialStatusId"] = new SelectList(_context.MartialStatuses, "MartialStatusId", "NameAr", family.MartialStatusId);
                ViewData["ReligionId"] = new SelectList(_context.Religions, "ReligionId", "NameAr", family.ReligionId);
                //ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", family.UserId);

                var StatesList = new SelectList(_context.States.ToList(), "StateId", "NameAr");
                ViewData["States"] = StatesList;
                return View(family);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: Families/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FamilyId,NameAr,NameEn,Address,HouseNumber,Alleyway,ResidentialSquare,FirstPhoneNumber,SecondPhoneNumber,NationalNumber,PassportNumber,NumberOfIndividuals,Age,MonthlyIncome,IsCurrentInvestmentProject,IsApprovedByManagement,IsCanceled,DateOfBirth,DistrictId,GenderId,ReligionId,MartialStatusId,JobId,HealthStatusId,EducationalStatusId,AccommodationTypeId,InvestmentTypeId,UserId,ManagementUserId")] Family family)
        {
            try
            {
                if (id != family.FamilyId)
                {
                    return NotFound();
                }

                TempData["Family"] = family.NameAr;
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(family);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!FamilyExists(family.FamilyId))
                        {
                            return View("Edit");
                        }
                        else
                        {
                            return View("Edit");
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                
                ViewData["AccommodationTypeId"] = new SelectList(_context.AccommodationTypes, "AccommodationTypeId", "NameAr", family.AccommodationTypeId);
                ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "NameAr", family.DistrictId);
                ViewData["EducationalStatusId"] = new SelectList(_context.EducationalStatuses, "EducationalStatusId", "NameAr", family.EducationalStatusId);
                ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "NameAr", family.GenderId);
                ViewData["HealthStatusId"] = new SelectList(_context.HealthStatuses, "HealthStatusId", "NameAr", family.HealthStatusId);
                ViewData["InvestmentTypeId"] = new SelectList(_context.InvestmentTypes, "InvestmentTypeId", "NameAr", family.InvestmentTypeId);
                ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "NameAr", family.JobId);
                ViewData["MartialStatusId"] = new SelectList(_context.MartialStatuses, "MartialStatusId", "NameAr", family.MartialStatusId);
                ViewData["ReligionId"] = new SelectList(_context.Religions, "ReligionId", "NameAr", family.ReligionId);
                ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", family.UserId);
                TempData["FamilyError"] = "FamilyError";
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
            catch (Exception ex)
            {
                TempData["FamilyError"] = "FamilyError";
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
        }

        // GET: Families/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var family = await _context.Families
                    .Include(f => f.AccommodationType)
                    .Include(f => f.District)
                    .Include(f => f.EducationalStatus)
                    .Include(f => f.Gender)
                    .Include(f => f.HealthStatus)
                    .Include(f => f.InvestmentType)
                    .Include(f => f.Job)
                    .Include(f => f.MartialStatus)
                    .Include(f => f.Religion)
                    .FirstOrDefaultAsync(m => m.FamilyId == id);
                if (family == null)
                {
                    return NotFound();
                }

                return View(family);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: Families/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var family = await _context.Families.FindAsync(id);
                //_context.Families.Remove(family);
                family.IsCanceled = true;
                //_context.Families.Remove(family);
                await _context.SaveChangesAsync();
                TempData["Family"] = family.NameAr;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["FamilyError"] = "FamilyError";
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
        }

        private bool FamilyExists(int id)
        {
            try
            {
                return _context.Families.Any(e => e.FamilyId == id);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
