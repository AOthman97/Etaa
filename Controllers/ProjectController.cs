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
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment hostingEnv;

        public ProjectController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            hostingEnv = webHostEnvironment;
        }

        // GET: Project
        public async Task<IActionResult> Index()
        {
            // .Include(p => p.Users)
            var applicationDbContext = _context.Projects;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Project/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // .Include(p => p.Users)
            var projects = await _context.Projects
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (projects == null)
            {
                return NotFound();
            }

            return View(projects);
        }

        // GET: Project/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr");
            return View();
        }

        public async Task<JsonResult> GetProjectSelectionReasons()
        {
            var Result = new MultiSelectList(await _context.ProjectSelectionReasons.ToListAsync(), "ProjectSelectionReasonsId", "NameAr");
            return Json(Result);
        }

        public async Task<JsonResult> GetProjectSocialBenefits()
        {
            var Result = new MultiSelectList(await _context.ProjectSocialBenefits.ToListAsync(), "ProjectSocialBenefitsId", "NameAr");
            return Json(Result);
        }

        public async Task<JsonResult> GetProjectTypes()
        {
            var Result = new SelectList(await _context.ProjectTypes.ToListAsync(), "ProjectTypeId", "NameAr");
            return Json(Result);
        }

        public async Task<JsonResult> GetProjectTypeAssets(int ProjectTypeId)
        {
            var Result = await _context.ProjectTypesAssets.Where(ProjectTypeAsset => ProjectTypeAsset.ProjectTypeId == ProjectTypeId).ToListAsync();
            return Json(Result);
        }

        public async Task<JsonResult> GetNumberOfFunds()
        {
            var Result = new SelectList(await _context.NumberOfFunds.ToListAsync(), "NumberOfFundsId", "NameAr");
            return Json(Result);
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
        //    ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr", projects.UserId);
        //    return View(projects);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Projects project, List<ProjectsAssets> projectsAssets, List<ProjectsSelectionReasons> projectsSelectionReasons, List<ProjectsSocialBenefits> projectsSocialBenefits)
        {
            try
            {
                var filePath = HttpContext.Session.GetString("filePath");
                project.SignatureofApplicantPath = filePath;
                _context.Add(project);
                await _context.SaveChangesAsync();
                foreach (var item in projectsAssets)
                {
                    item.ProjectId = project.ProjectId;
                    _context.Add(item);
                    await _context.SaveChangesAsync();
                }

                foreach (var item in projectsSelectionReasons)
                {
                    item.ProjectId = project.ProjectId;
                    _context.Add(item);
                    await _context.SaveChangesAsync();
                }

                foreach (var item in projectsSocialBenefits)
                {
                    item.ProjectId = project.ProjectId;
                    _context.Add(item);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Project/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr", projects.UserId);
            return View(projects);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,SignatureofApplicantPath,ProjectActivity,ProjectPurpose,Capital,MonthlyInstallmentAmount,NumberOfInstallments,Date,WaiverPeriod,IsApprovedByManagement,IsCanceled,FamilyId,NumberOfFundsId,ProjectTypeId,UserId,ManagementUserId")] Projects projects)
        {
            if (id != projects.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projects);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectsExists(projects.ProjectId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr", projects.UserId);
            return View(projects);
        }

        // GET: Project/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // .Include(p => p.Users)
            var projects = await _context.Projects
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (projects == null)
            {
                return NotFound();
            }

            return View(projects);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projects = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(projects);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectsExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }

        public async Task<IActionResult> Upload(IFormFile file)
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

        // For the select reasons: List<int> SelectReasons
        public async Task<JsonResult> testresult(List<ProjectsAssets> projectsAssets, List<ProjectsSelectionReasons> projectsSelectionReasons, List<ProjectsSocialBenefits> projectsSocialBenefits, Projects project)
        {
            //var Result = new SelectList(await _context.ProjectTypes.ToListAsync(), "ProjectTypeId", "NameAr");
            //return Json(Result);
            return Json(default);
        }
    }
}
