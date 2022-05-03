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
    public class ProjectTypesAssetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectTypesAssetController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectTypesAsset
        public async Task<IActionResult> Index(int ProjectTypeId)
        {
            List<ProjectTypesAssets> ProjectTypesAssets = await (from ProjectTypesAsset in _context.ProjectTypesAssets
                                                                 where ProjectTypesAsset.ProjectTypeId == ProjectTypeId
                                                                 select new ProjectTypesAssets
                                                                 {
                                                                     ProjectTypesAssetsId = ProjectTypesAsset.ProjectTypesAssetsId,
                                                                     NameAr = ProjectTypesAsset.NameAr,
                                                                     NameEn = ProjectTypesAsset.NameEn,
                                                                     ProjectTypeId = ProjectTypeId
                                                                 }).ToListAsync();

            ViewBag.ProjectTypeId = ProjectTypeId;
            // await _context.ProjectTypesAssets.ToListAsync()
            return View(ProjectTypesAssets);
        }

        // GET: ProjectTypesAsset/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTypesAssets = await _context.ProjectTypesAssets
                .FirstOrDefaultAsync(m => m.ProjectTypesAssetsId == id);
            if (projectTypesAssets == null)
            {
                return NotFound();
            }

            return View(projectTypesAssets);
        }

        // GET: ProjectTypesAsset/Create
        public IActionResult Create(int ProjectTypeId)
        {
            ProjectTypesAssets ProjectTypesAssets = new ProjectTypesAssets
            {
                ProjectTypeId = ProjectTypeId
            };

            return View(ProjectTypesAssets);
        }

        // POST: ProjectTypesAsset/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectTypesAssetsId,NameAr,NameEn,IsCanceled,ProjectTypeId")] ProjectTypesAssets projectTypesAssets)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectTypesAssets);
                await _context.SaveChangesAsync();
                // When the view just returned the Index the category items weren't shown, That's because we didn't pass-in
                // the ProjectTypeId to select from that's used in the Index action method
                return RedirectToAction(nameof(Index), new { ProjectTypeId = projectTypesAssets.ProjectTypeId });
            }
            return View(projectTypesAssets);
        }

        // GET: ProjectTypesAsset/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTypesAssets = await _context.ProjectTypesAssets.FindAsync(id);
            if (projectTypesAssets == null)
            {
                return NotFound();
            }
            return View(projectTypesAssets);
        }

        // POST: ProjectTypesAsset/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectTypesAssetsId,NameAr,NameEn,IsCanceled,ProjectTypeId")] ProjectTypesAssets projectTypesAssets)
        {
            if (id != projectTypesAssets.ProjectTypesAssetsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectTypesAssets);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectTypesAssetsExists(projectTypesAssets.ProjectTypesAssetsId))
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
            return View(projectTypesAssets);
        }

        // GET: ProjectTypesAsset/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTypesAssets = await _context.ProjectTypesAssets
                .FirstOrDefaultAsync(m => m.ProjectTypesAssetsId == id);
            if (projectTypesAssets == null)
            {
                return NotFound();
            }

            return View(projectTypesAssets);
        }

        // POST: ProjectTypesAsset/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectTypesAssets = await _context.ProjectTypesAssets.FindAsync(id);
            _context.ProjectTypesAssets.Remove(projectTypesAssets);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectTypesAssetsExists(int id)
        {
            return _context.ProjectTypesAssets.Any(e => e.ProjectTypesAssetsId == id);
        }
    }
}
