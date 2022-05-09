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

        public FinancialStatementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FinancialStatement
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.FinancialStatements.Include(f => f.Projects).Include(f => f.Users);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: FinancialStatement/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: FinancialStatement/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr");
            return View();
        }

        // POST: FinancialStatement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FinancialStatementId,DocumentPath,IsApprovedByManagement,IsCanceled,ProjectId,UserId,ManagementUserId")] FinancialStatement financialStatement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(financialStatement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", financialStatement.ProjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr", financialStatement.UserId);
            return View(financialStatement);
        }

        // GET: FinancialStatement/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: FinancialStatement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FinancialStatementId,DocumentPath,IsApprovedByManagement,IsCanceled,ProjectId,UserId,ManagementUserId")] FinancialStatement financialStatement)
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

        // GET: FinancialStatement/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: FinancialStatement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var financialStatement = await _context.FinancialStatements.FindAsync(id);
            _context.FinancialStatements.Remove(financialStatement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FinancialStatementExists(int id)
        {
            return _context.FinancialStatements.Any(e => e.FinancialStatementId == id);
        }
    }
}
