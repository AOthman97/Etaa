#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Etaa.Data;
using Etaa.Models;
using Etaa.Extensions;
using Microsoft.AspNetCore.Authorization;

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
            try
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
            catch (Exception ex)
            {
                return Json(default);
            }
        }

        [HttpPost]
        public ActionResult Index(string FamilyName, string FamilyId)
        {
            try
            {
                ViewBag.Message = "Family Name: " + FamilyName + " FamilyId: " + FamilyId;
                return View();
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [Authorize]
        // GET: FamilyMembers
        public async Task<IActionResult> Index()
        {
            try
            {
                var applicationDbContext = _context.FamilyMembers.Include(f => f.EducationalStatus).Include(f => f.Family).Include(f => f.Gender).Include(f => f.Job).Include(f => f.Kinship);
                return View(await applicationDbContext.OrderBy(family => family.Family.NameEn).ToListAsync());
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: FamilyMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
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
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: FamilyMembers/AddOrEditAsync
        // , int FamilyId
        public async Task<IActionResult> AddOrEdit(int FamilyMemberId = 0)
        {
            try
            {
                // Meaning it's an add operation
                if (FamilyMemberId == 0)
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
                ViewData["EducationalStatusId"] = new SelectList(_context.EducationalStatuses, "EducationalStatusId", "NameAr");
                ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "NameAr");
                ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "NameAr");
                ViewData["KinshipId"] = new SelectList(_context.Kinships, "KinshipId", "NameAr");
                return View();
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: FamilyMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NameAr,NameEn,Age,Note,IsCanceled,KinshipId,GenderId,EducationalStatusId,JobId,FamilyId")] FamilyMember familyMember)
        {
            try
            {
                //foreach (var item in ViewData.ModelState.Values)
                //{
                //    if(item.Errors != null)
                //    {

                //    }
                //}

                if (ModelState.IsValid && (familyMember.FamilyId != 0 && familyMember.FamilyId != null))
                {
                    _context.Add(familyMember);
                    await _context.SaveChangesAsync();
                    TempData["FamilyMember"] = "FamilyMember";
                    return RedirectToAction(nameof(Index));
                }
                ViewData["EducationalStatusId"] = new SelectList(_context.EducationalStatuses, "EducationalStatusId", "NameAr", familyMember.EducationalStatusId);
                //ViewData["FamilyId"] = new SelectList(_context.Families, "FamilyId", "NameAr", familyMember.FamilyId);
                ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "NameAr", familyMember.GenderId);
                ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "NameAr", familyMember.JobId);
                ViewData["KinshipId"] = new SelectList(_context.Kinships, "KinshipId", "NameAr", familyMember.KinshipId);
                ViewData["FamilyId"] = familyMember.FamilyId;
                TempData["FamilyMemberError"] = "FamilyMemberError";
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
            catch (Exception ex)
            {
                TempData["FamilyMemberError"] = "FamilyMemberError";
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
        }

        // GET: FamilyMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
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
                ViewData["FamilyNameAr"] = _context.Families.Where(f => f.FamilyId == familyMember.FamilyId).Select(f => f.NameAr).Single();
                return View(familyMember);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: FamilyMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FamilyMemberId,NameAr,NameEn,Age,Note,IsCanceled,KinshipId,GenderId,EducationalStatusId,JobId,FamilyId")] FamilyMember familyMember)
        {
            try
            {
                if (id != familyMember.FamilyMemberId)
                {
                    TempData["FamilyMemberError"] = "FamilyMemberError";
                    var RedirectURLFirst = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                    return Json(new
                    {
                        redirectUrl = RedirectURLFirst
                    });
                }

                TempData["FamilyMember"] = "FamilyMember";
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
                            return View("Edit");
                        }
                        else
                        {
                            return View("Edit");
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                ViewData["EducationalStatusId"] = new SelectList(_context.EducationalStatuses, "EducationalStatusId", "NameAr", familyMember.EducationalStatusId);
                ViewData["FamilyId"] = new SelectList(_context.Families, "FamilyId", "NameAr", familyMember.FamilyId);
                ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "NameAr", familyMember.GenderId);
                ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "NameAr", familyMember.JobId);
                ViewData["KinshipId"] = new SelectList(_context.Kinships, "KinshipId", "NameAr", familyMember.KinshipId);
                TempData["FamilyMemberError"] = "FamilyMemberError";
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
            catch (Exception ex)
            {
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
        }

        // GET: FamilyMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
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
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: FamilyMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                TempData["FamilyMember"] = "FamilyMember";
                var familyMember = await _context.FamilyMembers.FindAsync(id);
                familyMember.IsCanceled = true;
                //_context.FamilyMembers.Remove(familyMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["FamilyMemberError"] = "FamilyMemberError";
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
        }

        private bool FamilyMemberExists(int id)
        {
            try
            {
                return _context.FamilyMembers.Any(e => e.FamilyMemberId == id);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
