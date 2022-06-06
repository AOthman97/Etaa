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

namespace Etaa.Controllers
{
    public class ClearanceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment hostingEnv;

        public ClearanceController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            hostingEnv = webHostEnvironment;
        }

        // This action and the below are for the autocomplete functionality to firstly select the Project and get the ProjectID
        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            try
            {
                var Project = (from project in this._context.Projects
                               where project.NameEn.StartsWith(prefix)
                               select new
                               {
                                   label = project.NameEn,
                                   val = project.ProjectId
                               }).ToList();

                return Json(Project);
            }
            catch (Exception ex)
            {
                return Json(default);
            }
        }

        [HttpPost]
        public decimal GetRemainAmount(int projectId)
        {
            try
            {
                decimal SumPaidAmount = (from paymentVoucherVar in _context.PaymentVouchers
                                                         where paymentVoucherVar.ProjectId == projectId
                                                         select (decimal)paymentVoucherVar.PaymentAmount).Sum();
                decimal Capital = (from project in _context.Projects
                                   where project.ProjectId == projectId
                                   select (decimal)project.Capital.GetValueOrDefault()).Single();

                return (Capital - SumPaidAmount);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        // GET: Clearance
        public async Task<IActionResult> Index()
        {
            try
            {
                // .Include(c => c.Users)
                var applicationDbContext = _context.Clearances.Include(c => c.Projects);
                return View(await applicationDbContext.ToListAsync());
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: Clearance/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null || _context.Clearances == null)
                {
                    return NotFound();
                }

                var clearance = await _context.Clearances
                    .Include(c => c.Projects)
                    .FirstOrDefaultAsync(m => m.ClearanceId == id);
                if (clearance == null)
                {
                    return NotFound();
                }

                return View(clearance);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: Clearance/Create
        public IActionResult Create()
        {
            try
            {
                ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId");
                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr");
                return View();
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: Clearance/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ClearanceId,ClearanceDocumentPath,Comments,ClearanceDate,IsApprovedByManagement,IsCanceled,ProjectId,UserId,ManagementUserId")] Clearance clearance)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _context.Add(clearance);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //        ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", clearance.ProjectId);
        //        ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr", clearance.UserId);
        //        return View(clearance);
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("Error");
        //    }
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Clearance clearance)
        {
            try
            {
                var userId = User.GetLoggedInUserId<string>();
                var filePath = HttpContext.Session.GetString("filePath");
                HttpContext.Session.Clear();
                clearance.ClearanceDocumentPath = filePath;
                clearance.UserId = userId;

                // Firstly before saving the clearance we need to check if this project is fully paid
                int ProjectId = clearance.ProjectId;
                decimal Capital = (from project in _context.Projects
                                            where project.ProjectId == clearance.ProjectId
                                            select (decimal)project.Capital.GetValueOrDefault()).Single();

                decimal SumPaidAmount = (from paymentVoucherVar in _context.PaymentVouchers
                                                         where paymentVoucherVar.ProjectId == clearance.ProjectId
                                                         select (decimal)paymentVoucherVar.PaymentAmount).Sum();

                var Project = _context.Clearances.Where(c => c.ProjectId == clearance.ProjectId).Select(c => c.ProjectId);
                if(Project.Any() == false)
                {
                    if (SumPaidAmount == Capital)
                    {
                        _context.Add(clearance);
                        await _context.SaveChangesAsync();
                        TempData["Clearance"] = clearance.ClearanceDate;
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["PaidAmount"] = clearance.ClearanceDate;
                        return View("Create", clearance);
                    }
                }
                else
                {
                    TempData["ProjectAlreadyHasAClearance"] = clearance.ClearanceDate;
                    return View("Create", clearance);
                }
            }
            catch (Exception ex)
            {
                TempData["Clearance"] = clearance.ClearanceDate;
                return View("Create", clearance);
            }
        }

        // GET: Clearance/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null || _context.Clearances == null)
                {
                    return NotFound();
                }

                var clearance = await _context.Clearances.FindAsync(id);
                if (clearance == null)
                {
                    return NotFound();
                }
                ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", clearance.ProjectId);
                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr", clearance.UserId);
                ViewData["ProjectNameAr"] = _context.Projects.Where(f => f.ProjectId == clearance.ProjectId).Select(f => f.NameAr).Single();
                return View(clearance);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: Clearance/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int clearanceId, [Bind("ClearanceId,ClearanceDocumentPath,Comments,ClearanceDate,IsApprovedByManagement,IsCanceled,ProjectId,UserId,ManagementUserId")] Clearance clearance)
        {
            try
            {
                if (clearanceId != clearance.ClearanceId)
                {
                    return NotFound();
                }

                TempData["Clearance"] = "Clearance";
                if (ModelState.IsValid)
                {
                    try
                    {
                        // If the session value exists then the user has added a file to update instead of the existing file
                        var NewFilePath = HttpContext.Session.GetString("filePath");
                        if (NewFilePath != null)
                        {
                            HttpContext.Session.Clear();
                            var OldFilePath = "";
                            OldFilePath = _context.Clearances.Where(f => f.ClearanceId == clearance.ClearanceId).Select(f => f.ClearanceDocumentPath).Single();
                            FileInfo file = new FileInfo(OldFilePath);
                            if (file.Exists)
                            {
                                file.Delete();
                            }
                            clearance.ClearanceDocumentPath = NewFilePath;
                        }

                        _context.Update(clearance);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ClearanceExists(clearance.ClearanceId))
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
                ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", clearance.ProjectId);
                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr", clearance.UserId);
                return View("Edit");
            }
            catch (Exception ex)
            {
                return View("Edit");
            }
        }

        // GET: Clearance/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null || _context.Clearances == null)
                {
                    return NotFound();
                }

                var clearance = await _context.Clearances
                    .Include(c => c.Projects)
                    .FirstOrDefaultAsync(m => m.ClearanceId == id);
                if (clearance == null)
                {
                    return NotFound();
                }

                return View(clearance);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: Clearance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                TempData["Clearance"] = "Clearance";
                if (_context.Clearances == null)
                {
                    return View("Delete");
                    //return Problem("Entity set 'ApplicationDbContext.Clearances'  is null.");
                }
                var clearance = await _context.Clearances.FindAsync(id);
                
                if (clearance != null)
                {
                    clearance.IsCanceled = true;
                    //_context.Clearances.Remove(clearance);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else 
                {
                    return View("Delete");
                }
            }
            catch (Exception ex)
            {
                return View("Delete");
            }
        }

        private bool ClearanceExists(int id)
        {
            try
            {
                return (_context.Clearances?.Any(e => e.ClearanceId == id)).GetValueOrDefault();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                var FileDic = "Temp";
                string SubFileDic = Guid.NewGuid().ToString();
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SubFolderName")))
                {
                    SubFileDic = HttpContext.Session.GetString("SubFolderName").ToString();
                }
                else
                {
                    HttpContext.Session.SetString("SubFolderName", SubFileDic);
                }

                string FilePath = Path.Combine(hostingEnv.WebRootPath, FileDic);

                if (!Directory.Exists(FilePath))
                    Directory.CreateDirectory(FilePath);
                string SubFilePath = Path.Combine(FilePath, SubFileDic);
                if (!Directory.Exists(SubFilePath))
                    Directory.CreateDirectory(SubFilePath);
                var fileName = file.FileName;

                string filePath = Path.Combine(SubFilePath, fileName);
                HttpContext.Session.SetString("filePath", filePath);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        // Get content type
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        //[HttpGet]
        public async Task<IActionResult> Download(int ClearanceId)
        {
            try
            {
                var fileSession = await (from clearance in this._context.Clearances
                                         where clearance.ClearanceId.Equals(ClearanceId)
                                         select (string?)clearance.ClearanceDocumentPath).SingleOrDefaultAsync();

                var fileName = fileSession;
                var fileExists = System.IO.File.Exists(fileName);
                if (fileExists)
                {
                    string FileExtension = GetContentType(fileName);
                    return PhysicalFile(fileName, FileExtension, fileName);
                    //return PhysicalFile(fileName, "application/pdf", fileName);
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }
}
