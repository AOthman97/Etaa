#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Etaa.Data;
using Etaa.Models;

namespace Etaa.Controllers
{
    public class FamilyMembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FamilyMembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // This action and the below are for the autocomplete functionality to firstly select the family and get the FamilyID
        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            var Families = (from family in this._context.Families
                             where family.NameEn.StartsWith(prefix)
                             select new
                             {
                                 label = family.NameEn,
                                 val = family.FamilyId
                             }).ToList();

            return Json(Families);
        }

        [HttpPost]
        public ActionResult Index(string FamilyName, string FamilyId)
        {
            ViewBag.Message = "Family Name: " + FamilyName + " FamilyId: " + FamilyId;
            return View();
        }

        // GET: FamilyMembers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.FamilyMembers.Include(f => f.EducationalStatus).Include(f => f.Family).Include(f => f.Gender).Include(f => f.Job).Include(f => f.Kinship);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: FamilyMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var familyMember = await _context.FamilyMembers
                .Include(f => f.EducationalStatus)
                .Include(f => f.Family)
                .Include(f => f.Gender)
                .Include(f => f.Job)
                .Include(f => f.Kinship)
                .FirstOrDefaultAsync(m => m.FamilyMemberId == id);
            if (familyMember == null)
            {
                return NotFound();
            }

            return View(familyMember);
        }

        // GET: FamilyMembers/AddOrEditAsync
        // , int FamilyId
        public async Task<IActionResult> AddOrEditAsync(int FamilyMemberId = 0)
        {
            // Meaning it's an add operation
            if(FamilyMemberId == 0)
            {
                ViewData["EducationalStatusId"] = new SelectList(_context.EducationalStatuses, "EducationalStatusId", "NameAr");
                //ViewData["FamilyId"] = new SelectList(_context.Families, "FamilyId", "NameAr");
                ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "NameAr");
                ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "NameAr");
                ViewData["KinshipId"] = new SelectList(_context.Kinships, "KinshipId", "NameAr");
                return View();
            }
            else
            {
                var familyMember = await _context.FamilyMembers.FindAsync(FamilyMemberId);
                if (familyMember == null)
                {
                    return NotFound();
                }
                ViewData["EducationalStatusId"] = new SelectList(_context.EducationalStatuses, "EducationalStatusId", "NameAr", familyMember.EducationalStatusId);
                //ViewData["FamilyId"] = new SelectList(_context.Families, "FamilyId", "NameAr", familyMember.FamilyId);
                ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "NameAr", familyMember.GenderId);
                ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "NameAr", familyMember.JobId);
                ViewData["KinshipId"] = new SelectList(_context.Kinships, "KinshipId", "NameAr", familyMember.KinshipId);
                ViewData["FamilyId"] = familyMember.FamilyId;
                return View(familyMember);
            }
        }

        // GET: Families/Create
        public IActionResult Create()
        {
            ViewData["EducationalStatusId"] = new SelectList(_context.EducationalStatuses, "EducationalStatusId", "NameAr");
            ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "NameAr");
            ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "NameAr");
            ViewData["KinshipId"] = new SelectList(_context.Kinships, "KinshipId", "NameAr");
            return View();
        }

        // POST: FamilyMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FamilyMemberId,NameAr,NameEn,Age,Note,IsCanceled,KinshipId,GenderId,EducationalStatusId,JobId,FamilyId")] FamilyMember familyMember)
        {
            if (ModelState.IsValid && (familyMember.FamilyId != 0 && familyMember.FamilyId != null))
            {
                _context.Add(familyMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EducationalStatusId"] = new SelectList(_context.EducationalStatuses, "EducationalStatusId", "NameAr", familyMember.EducationalStatusId);
            //ViewData["FamilyId"] = new SelectList(_context.Families, "FamilyId", "NameAr", familyMember.FamilyId);
            ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "NameAr", familyMember.GenderId);
            ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "NameAr", familyMember.JobId);
            ViewData["KinshipId"] = new SelectList(_context.Kinships, "KinshipId", "NameAr", familyMember.KinshipId);
            ViewData["FamilyId"] = familyMember.FamilyId;
            return View(familyMember);
        }

        // GET: FamilyMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var familyMember = await _context.FamilyMembers.FindAsync(id);
            if (familyMember == null)
            {
                return NotFound();
            }
            ViewData["EducationalStatusId"] = new SelectList(_context.EducationalStatuses, "EducationalStatusId", "NameAr", familyMember.EducationalStatusId);
            ViewData["FamilyId"] = new SelectList(_context.Families, "FamilyId", "NameAr", familyMember.FamilyId);
            ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "NameAr", familyMember.GenderId);
            ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "NameAr", familyMember.JobId);
            ViewData["KinshipId"] = new SelectList(_context.Kinships, "KinshipId", "NameAr", familyMember.KinshipId);
            return View(familyMember);
        }

        // POST: FamilyMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FamilyMemberId,NameAr,NameEn,Age,Note,IsCanceled,KinshipId,GenderId,EducationalStatusId,JobId,FamilyId")] FamilyMember familyMember)
        {
            if (id != familyMember.FamilyMemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(familyMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FamilyMemberExists(familyMember.FamilyMemberId))
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
            ViewData["EducationalStatusId"] = new SelectList(_context.EducationalStatuses, "EducationalStatusId", "NameAr", familyMember.EducationalStatusId);
            ViewData["FamilyId"] = new SelectList(_context.Families, "FamilyId", "NameAr", familyMember.FamilyId);
            ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "NameAr", familyMember.GenderId);
            ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "NameAr", familyMember.JobId);
            ViewData["KinshipId"] = new SelectList(_context.Kinships, "KinshipId", "NameAr", familyMember.KinshipId);
            return View(familyMember);
        }

        // GET: FamilyMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var familyMember = await _context.FamilyMembers
                .Include(f => f.EducationalStatus)
                .Include(f => f.Family)
                .Include(f => f.Gender)
                .Include(f => f.Job)
                .Include(f => f.Kinship)
                .FirstOrDefaultAsync(m => m.FamilyMemberId == id);
            if (familyMember == null)
            {
                return NotFound();
            }

            return View(familyMember);
        }

        // POST: FamilyMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var familyMember = await _context.FamilyMembers.FindAsync(id);
            _context.FamilyMembers.Remove(familyMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FamilyMemberExists(int id)
        {
            return _context.FamilyMembers.Any(e => e.FamilyMemberId == id);
        }
    }
}
