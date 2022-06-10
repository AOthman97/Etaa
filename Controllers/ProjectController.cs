#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Etaa.Data;
using Etaa.Models;
using System.Collections;
using Etaa.Extensions;
using Serilog.Context;

namespace Etaa.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _hostingEnv;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<ProjectController> logger)
        {
            _context = context;
            _hostingEnv = webHostEnvironment;
            _logger = logger;
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

        // GET: Project
        public async Task<IActionResult> Index()
        {
            try
            {
                // .Include(p => p.IdentityUser)
                var applicationDbContext = _context.Projects;
                return View(await applicationDbContext.ToListAsync());
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: Project/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // .Include(p => p.IdentityUser)
                var projects = await _context.Projects
                    .FirstOrDefaultAsync(m => m.ProjectId == id);
                if (projects == null)
                {
                    return NotFound();
                }

                //HttpContext.Session.SetString("filePath", projects.SignatureofApplicantPath);
                return View(projects);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: Project/Create
        public IActionResult Create()
        {
            try
            {
                ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr");
                return View();
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public async Task<JsonResult> GetProjectSelectionReasons()
        {
            try
            {
                var Result = new MultiSelectList(await _context.ProjectSelectionReasons.ToListAsync(), "ProjectSelectionReasonsId", "NameAr");
                return Json(Result);
            }
            catch (Exception ex)
            {
                return Json(default);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetProjectSelectionReasons(int ProjectId)
        {
            try
            {
                IEnumerable SelectedValues = _context.ProjectsSelectionReasons.Where(p => p.ProjectId == ProjectId).Select(p => p.ProjectSelectionReasonsId);
                var Result = new MultiSelectList(await _context.ProjectSelectionReasons.ToListAsync(), "ProjectSelectionReasonsId", "NameAr", SelectedValues);
                return Json(Result);
            }
            catch (Exception ex)
            {
                return Json(default);
            }
        }

        public async Task<JsonResult> GetProjectSocialBenefits()
        {
            try
            {
                var Result = new MultiSelectList(await _context.ProjectSocialBenefits.ToListAsync(), "ProjectSocialBenefitsId", "NameAr");
                return Json(Result);
            }
            catch (Exception ex)
            {
                return Json(default);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetProjectSocialBenefits(int ProjectId)
        {
            try
            {
                IEnumerable SelectedValues = _context.ProjectsSocialBenefits.Where(p => p.ProjectId == ProjectId).Select(p => p.ProjectSocialBenefitsId);
                var Result = new MultiSelectList(await _context.ProjectSocialBenefits.ToListAsync(), "ProjectSocialBenefitsId", "NameAr", SelectedValues);
                return Json(Result);
            }
            catch (Exception ex)
            {
                return Json(default);
            }
        }

        public async Task<JsonResult> GetProjectTypes()
        {
            try
            {
                var Result = new SelectList(await _context.ProjectTypes.ToListAsync(), "ProjectTypeId", "NameAr");
                return Json(Result);
            }
            catch (Exception ex)
            {
                return Json(default);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetProjectTypes(int ProjectId)
        {
            try
            {
                ProjectTypes projectTypes = new ProjectTypes();
                projectTypes.ProjectTypeId = _context.Projects.Where(p => p.ProjectId == ProjectId).Select(p => p.ProjectTypeId).SingleOrDefault();
                var Result = new SelectList(await _context.ProjectTypes.ToListAsync(), "ProjectTypeId", "NameAr", projectTypes.ProjectTypeId);
                return Json(Result);
            }
            catch (Exception ex)
            {
                return Json(default);
            }
        }

        public async Task<JsonResult> GetProjectTypeAssets(int ProjectTypeId)
        {
            try
            {
                var Result = await _context.ProjectTypesAssets.Where(ProjectTypeAsset => ProjectTypeAsset.ProjectTypeId == ProjectTypeId).ToListAsync();
                return Json(Result);
            }
            catch (Exception ex)
            {
                return Json(default);
            }
        }

        public async Task<JsonResult> GetProjectTypeAssetsForEdit(int ProjectId)
        {
            try
            {
                //var Result = await _context.ProjectTypesAssets.Where(ProjectTypeAsset => ProjectTypeAsset.ProjectTypeId == ProjectTypeId).ToListAsync();
                var Result = await _context.ProjectAssetesProjectTypeAssets.Where(ProjectsAssets => ProjectsAssets.ProjectId == ProjectId).ToListAsync();
                return Json(Result);
            }
            catch (Exception ex)
            {
                return Json(default);
            }
        }

        public async Task<JsonResult> GetNumberOfFunds()
        {
            try
            {
                var Result = new SelectList(await _context.NumberOfFunds.ToListAsync(), "NumberOfFundsId", "NameAr");
                return Json(Result);
            }
            catch (Exception ex)
            {
                return Json(default);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetNumberOfFunds(int ProjectId)
        {
            try
            {
                NumberOfFunds numberOfFunds = new NumberOfFunds();
                numberOfFunds.NumberOfFundsId = _context.Projects.Where(p => p.ProjectId == ProjectId).Select(p => p.NumberOfFundsId).SingleOrDefault();
                var Result = new SelectList(await _context.NumberOfFunds.ToListAsync(), "NumberOfFundsId", "NameAr", numberOfFunds.NumberOfFundsId);
                return Json(Result);
            }
            catch (Exception ex)
            {
                return Json(default);
            }
        }

        // POST: Project/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ProjectId,SignatureofApplicantPath,ProjectActivity,ProjectPurpose,Capital,MonthlyInstallmentAmount,NumberOfInstallments,Date,WaiverPeriod,IsApprovedByManagement,IsCanceled,FamilyId,NumberOfFundsId,ProjectTypeId,UserId,ManagementUserId")] Projects projects)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(projects);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", projects.UserId);
        //    return View(projects);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Projects project, List<ProjectsAssets> projectsAssets, List<ProjectsSelectionReasons> projectsSelectionReasons, List<ProjectsSocialBenefits> projectsSocialBenefits)
        {
            try
            {
                var userId = User.GetLoggedInUserId<string>();
                //var userName = User.GetLoggedInUserName();
                //var userEmail = User.GetLoggedInUserEmail();

                var filePath = HttpContext.Session.GetString("filePath");
                HttpContext.Session.Clear();
                project.SignatureofApplicantPath = filePath;
                project.UserId = userId;
                var ProjectTypeNameEn = (from projectType in _context.ProjectTypes
                                         where projectType.ProjectTypeId == project.ProjectTypeId
                                         select projectType.NameEn).SingleOrDefaultAsync();
                var ProjectTypeNameAr = (from projectType in _context.ProjectTypes
                                         where projectType.ProjectTypeId == project.ProjectTypeId
                                         select projectType.NameAr).SingleOrDefaultAsync();
                var FamilyNameEn = (from family in _context.Families
                                    where family.FamilyId == project.FamilyId
                                    select family.NameEn).SingleOrDefaultAsync();
                var FamilyNameAr = (from family in _context.Families
                                    where family.FamilyId == project.FamilyId
                                    select family.NameAr).SingleOrDefaultAsync();
                project.NameEn = String.Concat(ProjectTypeNameEn.Result, " ", FamilyNameEn.Result);
                project.NameAr = String.Concat(ProjectTypeNameAr.Result, " ", FamilyNameAr.Result);

                decimal Capital = (decimal)project.Capital;
                decimal MonthlyInstallmentAmount = (decimal)project.MonthlyInstallmentAmount;
                int NumberOfInstallments = (int)project.NumberOfInstallments;
                int MaxInstallmentsNo = _context.Installments.Select(i => i.InstallmentsId).Max();

                if((NumberOfInstallments > MaxInstallmentsNo))
                {
                    TempData["NumberOfInstallmentsGreaterThanMaxInstallmentNo"] = "Project";
                    return View("Create");
                }
                else if((Capital / MonthlyInstallmentAmount != NumberOfInstallments))
                {
                    TempData["CapitalDividedByMonthlyInstallmentAmountNotEqualToNumberOfInstallments"] = "Project";
                    return View("Create");
                }
                else
                {
                    _context.Add(project);
                    foreach (var item in projectsAssets)
                    {
                        item.ProjectId = project.ProjectId;
                        _context.Add(item);
                    }

                    foreach (var item in projectsSelectionReasons)
                    {
                        item.ProjectId = project.ProjectId;
                        _context.Add(item);
                    }

                    foreach (var item in projectsSocialBenefits)
                    {
                        item.ProjectId = project.ProjectId;
                        _context.Add(item);
                    }

                    await _context.SaveChangesAsync();

                    TempData["Project"] = "Project";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["Project"] = "Project";
                return View("Create");
            }
        }

        // GET: Project/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var projects = await _context.Projects.FindAsync(id);
                if (projects == null)
                {
                    return NotFound();
                }
                ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", projects.UserId);
                ViewData["FamilyNameAr"] = _context.Families.Where(f => f.FamilyId == projects.FamilyId).Select(f => f.NameAr).Single();
                //HttpContext.Session.SetString("filePath", projects.SignatureofApplicantPath);
                return View(projects);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int projectId, Projects projects, List<ProjectsAssets> projectsAssets, List<ProjectsSelectionReasons> projectsSelectionReasons, List<ProjectsSocialBenefits> projectsSocialBenefits)
        {
            try
            {
                // In each of these models create actions you should check first if there's no not canceled rows that have
                // the same ProjectId in them
                var FinancialStatemntId = _context.FinancialStatements.Where(f => f.ProjectId == projectId && f.IsCanceled == false).Select(f => f.FinancialStatementId);
                var ClearanceId = _context.Clearances.Where(c => c.ProjectId == projectId && c.IsCanceled == false).Select(c => c.ClearanceId);
                var PaymentVoucherId = _context.PaymentVouchers.Where(p => p.ProjectId == projectId && p.IsCanceled == false).Select(p => p.PaymentVoucherId);
                // This is a business validation
                if (projectId != projects.ProjectId || FinancialStatemntId.Any())
                {
                    TempData["FinancialStatemntId"] = "FinancialStatemntId";
                    var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", projects.UserId));
                    return Json(new
                    {
                        redirectUrl = RedirectURL
                    });
                }
                else if (ClearanceId.Any())
                {
                    TempData["ClearanceId"] = "ClearanceId";
                    return View("Edit");
                }
                else if (PaymentVoucherId.Any())
                {
                    TempData["PaymentVoucherId"] = "PaymentVoucherId";
                    return View("Edit");
                }
                else
                {
                    try
                    {
                        // If the session value exists then the user has added a file to update instead of the existing file
                        var NewFilePath = HttpContext.Session.GetString("filePath");
                        if (NewFilePath != null)
                        {
                            HttpContext.Session.Clear();
                            var OldFilePath = "";
                            OldFilePath = _context.Projects.Where(f => f.ProjectId == projects.ProjectId).Select(f => f.SignatureofApplicantPath).Single();
                            FileInfo file = new FileInfo(OldFilePath);
                            if (file.Exists)
                            {
                                file.Delete();
                            }
                            projects.SignatureofApplicantPath = NewFilePath;
                        }

                        string ProjectTypeNameEn = (from projectType in _context.ProjectTypes
                                                    where projectType.ProjectTypeId == projects.ProjectTypeId
                                                    select projectType.NameEn).Single();
                        string ProjectTypeNameAr = (from projectType in _context.ProjectTypes
                                                    where projectType.ProjectTypeId == projects.ProjectTypeId
                                                    select (string)projectType.NameAr).Single();
                        string FamilyNameEn = (from family in _context.Families
                                               where family.FamilyId == projects.FamilyId
                                               select (string)family.NameEn).Single();
                        string FamilyNameAr = (from family in _context.Families
                                               where family.FamilyId == projects.FamilyId
                                               select (string)family.NameAr).Single();
                        projects.NameEn = String.Concat(ProjectTypeNameEn, " ", FamilyNameEn);
                        projects.NameAr = String.Concat(ProjectTypeNameAr, " ", FamilyNameAr);

                        _context.Update(projects);
                        //await _context.SaveChangesAsync();

                        // For this model and the other two models instead of directly updating their rows and the hassle of it,
                        // Just delete the rows and add new rows with the new values
                        List<ProjectsAssets> projectsAssetsRemoved = await _context.ProjectsAssets.Where(p => p.ProjectId == projectId).ToListAsync();

                        foreach (var item in projectsAssetsRemoved)
                        {
                            _context.Remove(item);
                        }

                        List<ProjectsSelectionReasons> projectsSelectionReasonsRemoved = await _context.ProjectsSelectionReasons.Where(p => p.ProjectId == projectId).ToListAsync();

                        foreach (var item in projectsSelectionReasonsRemoved)
                        {
                            _context.Remove(item);
                        }

                        List<ProjectsSocialBenefits> projectsSocialBenefitsRemoved = await _context.ProjectsSocialBenefits.Where(p => p.ProjectId == projectId).ToListAsync();

                        foreach (var item in projectsSocialBenefitsRemoved)
                        {
                            _context.Remove(item);
                        }

                        foreach (var item in projectsAssets)
                        {
                            item.ProjectId = projects.ProjectId;
                            _context.Add(item);
                        }

                        foreach (var item in projectsSelectionReasons)
                        {
                            item.ProjectId = projects.ProjectId;
                            _context.Add(item);
                        }

                        foreach (var item in projectsSocialBenefits)
                        {
                            item.ProjectId = projects.ProjectId;
                            _context.Add(item);
                        }

                        await _context.SaveChangesAsync();

                        TempData["Project"] = "Project";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProjectsExists(projects.ProjectId))
                        {
                            return View("Edit");
                        }
                        else
                        {
                            return View("Edit");
                        }
                    }
                    ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", projects.UserId);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: Project/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // .Include(p => p.IdentityUser)
                var projects = await _context.Projects
                    .FirstOrDefaultAsync(m => m.ProjectId == id);
                if (projects == null)
                {
                    return NotFound();
                }

                return View(projects);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);
                // In each of these models create actions you should check first if there's no not canceled rows that have
                // the same ProjectId in them
                var FinancialStatemntId = _context.FinancialStatements.Where(f => f.ProjectId == id && f.IsCanceled == false).Select(f => f.FinancialStatementId);
                var ClearanceId = _context.Clearances.Where(c => c.ProjectId == id && c.IsCanceled == false).Select(c => c.ClearanceId);
                var PaymentVoucherId = _context.PaymentVouchers.Where(p => p.ProjectId == id && p.IsCanceled == false).Select(p => p.PaymentVoucherId);
                // This is a business validation
                if (FinancialStatemntId != null)
                {
                    TempData["FinancialStatemntId"] = "FinancialStatemntId";
                    return View("Delete");
                }
                else if (ClearanceId != null)
                {
                    TempData["ClearanceId"] = "ClearanceId";
                    return View("Delete");
                }
                else if (PaymentVoucherId != null)
                {
                    TempData["PaymentVoucherId"] = "PaymentVoucherId";
                    return View("Delete");
                }
                else
                {
                    project.IsCanceled = true;
                    await _context.SaveChangesAsync();
                    TempData["Project"] = "Project";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["Project"] = "Project";
                return View("Delete");
            }
        }

        private bool ProjectsExists(int id)
        {
            try
            {
                return _context.Projects.Any(e => e.ProjectId == id);
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

                string FilePath = Path.Combine(_hostingEnv.WebRootPath, FileDic);

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

                //delete Folder
                //string path = @"D:\Workarea\Test\Code";
                //Directory.Delete(path);


                //Move Folder from source to destination
                //string source = @"D:\Workarea\Test\Code";
                //string destination = @"D:\Workarea\Test\NewDirectory";

                //try
                //{
                //    // First, you should ensure that the
                //    // source directory exists
                //    if (Directory.Exists(source))
                //    {
                //        // You should eEnsure the destination
                //        // directory doesn't already exist
                //        if (!Directory.Exists(destination))
                //        {
                //            // Move the source directory
                //            // to the new location
                //            Directory.Move(source, destination);
                //        }
                //        else
                //        {
                //            Console.WriteLine("Destination directory" +
                //                        " already exists...");
                //        }
                //    }
                //    else
                //    {
                //        Console.WriteLine("Source directory " +
                //                "does not exist...");
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //}
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
        public async Task<IActionResult> Download(int ProjectId)
        {
            try
            {
                //var fileSession = HttpContext.Session.GetString("filePath");
                //HttpContext.Session.Remove("filePath");
                
                var fileSession = await (from project in this._context.Projects
                                         where project.ProjectId.Equals(ProjectId)
                                         select (string)project.SignatureofApplicantPath).SingleOrDefaultAsync();

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

                //var fileStream = System.IO.File.OpenRead(fileSession);
                //string FileExtension = GetContentType(fileSession);
                //return File(fileStream, FileExtension, fileSession);
                //return View("Error");

            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }
}
