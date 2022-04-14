﻿#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Etaa.Data;
using Etaa.Models;
using System.Data;

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
            ViewBag.States = new SelectList(_context.States, "StateId", "NameAr");
            return View();
        }

        [HttpGet]
        public JsonResult LoadCities(int StateId)
        {
            var City = _context.Cities.Where(City => City.StateId == StateId);
            return Json(new SelectList(City, "CityId", "NameAr"));
        }

        [HttpGet]
        public JsonResult LoadDistricts(int CityId)
        {
            var District = _context.Districts.Where(District => District.CityId == CityId);
            return Json(new SelectList(District, "DistrictId", "NameAr"));
        }

        // GET: Families
        public async Task<IActionResult> Index()
        {
            // .Include(f => f.Users)
            var applicationDbContext = _context.Families.Include(f => f.AccommodationType).Include(f => f.District).Include(f => f.EducationalStatus).Include(f => f.Gender).Include(f => f.HealthStatus).Include(f => f.InvestmentType).Include(f => f.Job).Include(f => f.MartialStatus).Include(f => f.Religion);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Families/Details/5
        public async Task<IActionResult> Details(int? id)
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
                .Include(f => f.Users)
                .FirstOrDefaultAsync(m => m.FamilyId == id);
            if (family == null)
            {
                return NotFound();
            }

            return View(family);
        }

        // GET: Families/Create
        public IActionResult Create()
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

        // POST: Families/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // ,UserId,ManagementUserId
        public async Task<IActionResult> Create([Bind("FamilyId,NameAr,NameEn,Address,HouseNumber,Alleyway,ResidentialSquare,FirstPhoneNumber,SecondPhoneNumber,NationalNumber,PassportNumber,NumberOfIndividuals,Age,MonthlyIncome,IsCurrentInvestmentProject,IsApprovedByManagement,IsCanceled,DateOfBirth,DistrictId,GenderId,ReligionId,MartialStatusId,JobId,HealthStatusId,EducationalStatusId,AccommodationTypeId,InvestmentTypeId")] Family family)
        {
            if (ModelState.IsValid)
            {
                _context.Add(family);
                await _context.SaveChangesAsync();
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
            //ViewData["FamilyId"] = family.FamilyId;
            return View(family);
        }

        // GET: Families/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr", family.UserId);
            return View(family);
        }

        // POST: Families/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FamilyId,NameAr,NameEn,Address,HouseNumber,Alleyway,ResidentialSquare,FirstPhoneNumber,SecondPhoneNumber,NationalNumber,PassportNumber,NumberOfIndividuals,Age,MonthlyIncome,IsCurrentInvestmentProject,IsApprovedByManagement,IsCanceled,DateOfBirth,DistrictId,GenderId,ReligionId,MartialStatusId,JobId,HealthStatusId,EducationalStatusId,AccommodationTypeId,InvestmentTypeId,UserId,ManagementUserId")] Family family)
        {
            if (id != family.FamilyId)
            {
                return NotFound();
            }

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
                        return NotFound();
                    }
                    else
                    {
                        throw;
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr", family.UserId);
            return View(family);
        }

        // GET: Families/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
                .Include(f => f.Users)
                .FirstOrDefaultAsync(m => m.FamilyId == id);
            if (family == null)
            {
                return NotFound();
            }

            return View(family);
        }

        // POST: Families/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var family = await _context.Families.FindAsync(id);
            _context.Families.Remove(family);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FamilyExists(int id)
        {
            return _context.Families.Any(e => e.FamilyId == id);
        }
    }
}
