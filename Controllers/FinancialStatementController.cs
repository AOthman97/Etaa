#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Etaa.Data;
using Etaa.Models;

namespace Etaa.Controllers
{
    public class FinancialStatementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment hostingEnv;

        public FinancialStatementController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            hostingEnv = webHostEnvironment;
        }

        // GET: FinancialStatement
        public async Task<IActionResult> Index()
        {
            try
            {
                var applicationDbContext = _context.FinancialStatements.Include(f => f.Projects).Include(f => f.Users);
                return View(await applicationDbContext.ToListAsync());
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: FinancialStatement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var financialStatement = await _context.FinancialStatements
                    .Include(f => f.Projects)
                    .Include(f => f.Users)
                    .FirstOrDefaultAsync(m => m.FinancialStatementId == id);
                if (financialStatement == null)
                {
                    return NotFound();
                }

                return View(financialStatement);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: FinancialStatement/Create
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

        // POST: FinancialStatement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("FinancialStatementId,DocumentPath,IsApprovedByManagement,IsCanceled,ProjectId,UserId,ManagementUserId")] FinancialStatement financialStatement)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(financialStatement);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", financialStatement.ProjectId);
        //    ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr", financialStatement.UserId);
        //    return View(financialStatement);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FinancialStatement financialStatement)
        {
            try
            {
                financialStatement.UserId = 1;
                var filePath = HttpContext.Session.GetString("filePath");
                HttpContext.Session.Clear();
                financialStatement.DocumentPath = filePath;
                _context.Add(financialStatement);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: FinancialStatement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var financialStatement = await _context.FinancialStatements.FindAsync(id);
                if (financialStatement == null)
                {
                    return NotFound();
                }
                ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", financialStatement.ProjectId);
                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr", financialStatement.UserId);
                return View(financialStatement);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: FinancialStatement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FinancialStatementId,DocumentPath,IsApprovedByManagement,IsCanceled,ProjectId,UserId,ManagementUserId")] FinancialStatement financialStatement)
        {
            try
            {
                if (id != financialStatement.FinancialStatementId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(financialStatement);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!FinancialStatementExists(financialStatement.FinancialStatementId))
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
                ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", financialStatement.ProjectId);
                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr", financialStatement.UserId);
                return View(financialStatement);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: FinancialStatement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var financialStatement = await _context.FinancialStatements
                    .Include(f => f.Projects)
                    .Include(f => f.Users)
                    .FirstOrDefaultAsync(m => m.FinancialStatementId == id);
                if (financialStatement == null)
                {
                    return NotFound();
                }

                return View(financialStatement);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: FinancialStatement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var financialStatement = await _context.FinancialStatements.FindAsync(id);
                _context.FinancialStatements.Remove(financialStatement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        private bool FinancialStatementExists(int id)
        {
            try
            {
                return _context.FinancialStatements.Any(e => e.FinancialStatementId == id);
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
    }
}
