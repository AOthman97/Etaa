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
using Microsoft.AspNetCore.Authorization;

namespace Etaa.Controllers
{
    public class ProjectTypesAssetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectTypesAssetController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        // GET: ProjectTypesAsset
        public async Task<IActionResult> Index(int ProjectTypeId)
        {
            try
            {
                List<ProjectTypesAssets> ProjectTypesAssets = await (from ProjectTypesAsset in _context.ProjectTypesAssets
                                                                     where ProjectTypesAsset.ProjectTypeId == ProjectTypeId
                                                                     && ProjectTypesAsset.IsCanceled == false
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
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: ProjectTypesAsset/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
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
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: ProjectTypesAsset/Create
        public IActionResult Create(int ProjectTypeId)
        {
            try
            {
                ProjectTypesAssets ProjectTypesAssets = new ProjectTypesAssets
                {
                    ProjectTypeId = ProjectTypeId
                };

                return View(ProjectTypesAssets);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: ProjectTypesAsset/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectTypesAssetsId,NameAr,NameEn,IsCanceled,ProjectTypeId")] ProjectTypesAssets projectTypesAssets)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["ProjectTypesAssets"] = "ProjectTypesAssets";
                    projectTypesAssets.IsCanceled = false;
                    _context.Add(projectTypesAssets);
                    await _context.SaveChangesAsync();
                    // When the view just returned the Index the category items weren't shown, That's because we didn't pass-in
                    // the ProjectTypeId to select from that's used in the Index action method
                    return RedirectToAction(nameof(Index), new { ProjectTypeId = projectTypesAssets.ProjectTypeId });
                }
                TempData["ProjectTypesAssetsError"] = "ProjectTypesAssets";
                return RedirectToAction(nameof(Index), new { ProjectTypeId = projectTypesAssets.ProjectTypeId });
            }
            catch (Exception ex)
            {
                TempData["ProjectTypesAssetsError"] = "ProjectTypesAssets";
                return RedirectToAction(nameof(Index), new { ProjectTypeId = projectTypesAssets.ProjectTypeId });
            }
        }

        // GET: ProjectTypesAsset/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
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
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: ProjectTypesAsset/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectTypesAssetsId,NameAr,NameEn,IsCanceled,ProjectTypeId")] ProjectTypesAssets projectTypesAssets)
        {
            try
            {
                if (id != projectTypesAssets.ProjectTypesAssetsId)
                {
                    TempData["ProjectTypesAssetsError"] = "ProjectTypesAssets";
                    return RedirectToAction(nameof(Index), new { ProjectTypeId = projectTypesAssets.ProjectTypeId });
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        TempData["ProjectTypesAssets"] = "ProjectTypesAssets";
                        _context.Update(projectTypesAssets);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProjectTypesAssetsExists(projectTypesAssets.ProjectTypesAssetsId))
                        {
                            TempData["ProjectTypesAssetsError"] = "ProjectTypesAssets";
                            return RedirectToAction(nameof(Index), new { ProjectTypeId = projectTypesAssets.ProjectTypeId });
                        }
                        else
                        {
                            TempData["ProjectTypesAssetsError"] = "ProjectTypesAssets";
                            return RedirectToAction(nameof(Index), new { ProjectTypeId = projectTypesAssets.ProjectTypeId });
                        }
                    }
                    // When the view just returned the Index the category items weren't shown, That's because we didn't pass-in
                    // the ProjectTypeId to select from that's used in the Index action method
                    return RedirectToAction(nameof(Index), new { ProjectTypeId = projectTypesAssets.ProjectTypeId });
                }
                TempData["ProjectTypesAssetsError"] = "ProjectTypesAssets";
                return RedirectToAction(nameof(Index), new { ProjectTypeId = projectTypesAssets.ProjectTypeId });
            }
            catch (Exception ex)
            {
                TempData["ProjectTypesAssetsError"] = "ProjectTypesAssets";
                return RedirectToAction(nameof(Index), new { ProjectTypeId = projectTypesAssets.ProjectTypeId });
            }
        }

        // GET: ProjectTypesAsset/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
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
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: ProjectTypesAsset/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                TempData["ProjectTypesAssets"] = "ProjectTypesAssets";
                var projectTypesAssets = await _context.ProjectTypesAssets.FindAsync(id);
                projectTypesAssets.IsCanceled = true;
                await _context.SaveChangesAsync();
                // When the view just returned the Index the category items weren't shown, That's because we didn't pass-in
                // the ProjectTypeId to select from that's used in the Index action method
                return RedirectToAction(nameof(Index), new { ProjectTypeId = projectTypesAssets.ProjectTypeId });
            }
            catch (Exception ex)
            {
                TempData["ProjectTypesAssetsError"] = "ProjectTypesAssets";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool ProjectTypesAssetsExists(int id)
        {
            try
            {
                return _context.ProjectTypesAssets.Any(e => e.ProjectTypesAssetsId == id);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
