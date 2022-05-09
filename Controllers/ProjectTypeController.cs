﻿#nullable disable
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
    public class ProjectTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectType
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProjectTypes.Include(p => p.ProjectDomainTypes).Include(p => p.ProjectGroup);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProjectType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTypes = await _context.ProjectTypes
                .Include(p => p.ProjectDomainTypes)
                .Include(p => p.ProjectGroup)
                .FirstOrDefaultAsync(m => m.ProjectTypeId == id);
            if (projectTypes == null)
            {
                return NotFound();
            }

            return View(projectTypes);
        }

        // GET: ProjectType/Create
        public IActionResult Create()
        {
            ViewData["ProjectDomainTypeId"] = new SelectList(_context.ProjectDomainTypes, "ProjectDomainTypeId", "NameAr");
            ViewData["ProjectGroupId"] = new SelectList(_context.ProjectGroups, "ProjectGroupId", "NameAr");
            return View();
        }

        // POST: ProjectType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectTypeId,NameAr,NameEn,IsCanceled,ProjectDomainTypeId,ProjectGroupId")] ProjectTypes projectTypes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectTypes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectDomainTypeId"] = new SelectList(_context.ProjectDomainTypes, "ProjectDomainTypeId", "NameAr", projectTypes.ProjectDomainTypeId);
            ViewData["ProjectGroupId"] = new SelectList(_context.ProjectGroups, "ProjectGroupId", "NameAr", projectTypes.ProjectGroupId);
            return View(projectTypes);
        }

        // GET: ProjectType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTypes = await _context.ProjectTypes.FindAsync(id);
            if (projectTypes == null)
            {
                return NotFound();
            }
            ViewData["ProjectDomainTypeId"] = new SelectList(_context.ProjectDomainTypes, "ProjectDomainTypeId", "NameAr", projectTypes.ProjectDomainTypeId);
            ViewData["ProjectGroupId"] = new SelectList(_context.ProjectGroups, "ProjectGroupId", "NameAr", projectTypes.ProjectGroupId);
            return View(projectTypes);
        }

        // POST: ProjectType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectTypeId,NameAr,NameEn,IsCanceled,ProjectDomainTypeId,ProjectGroupId")] ProjectTypes projectTypes)
        {
            if (id != projectTypes.ProjectTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectTypes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectTypesExists(projectTypes.ProjectTypeId))
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
            ViewData["ProjectDomainTypeId"] = new SelectList(_context.ProjectDomainTypes, "ProjectDomainTypeId", "NameAr", projectTypes.ProjectDomainTypeId);
            ViewData["ProjectGroupId"] = new SelectList(_context.ProjectGroups, "ProjectGroupId", "NameAr", projectTypes.ProjectGroupId);
            return View(projectTypes);
        }

        // GET: ProjectType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTypes = await _context.ProjectTypes
                .Include(p => p.ProjectDomainTypes)
                .Include(p => p.ProjectGroup)
                .FirstOrDefaultAsync(m => m.ProjectTypeId == id);
            if (projectTypes == null)
            {
                return NotFound();
            }

            return View(projectTypes);
        }

        // POST: ProjectType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectTypes = await _context.ProjectTypes.FindAsync(id);
            _context.ProjectTypes.Remove(projectTypes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectTypesExists(int id)
        {
            return _context.ProjectTypes.Any(e => e.ProjectTypeId == id);
        }
    }
}