namespace Etaa.Controllers
{
    public class ProjectTypesAssetController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProjectTypesAssetController> _logger;

        public ProjectTypesAssetController(ApplicationDbContext context, ILogger<ProjectTypesAssetController> logger)
        {
            _context = context;
            _logger = logger;
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
                                                                     }).AsNoTracking().ToListAsync();

                ViewBag.ProjectTypeId = ProjectTypeId;
                // await _context.ProjectTypesAssets.ToListAsync()
                return View(ProjectTypesAssets);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
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
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.ProjectTypesAssetsId == id);
                if (projectTypesAssets == null)
                {
                    return NotFound();
                }

                return View(projectTypesAssets);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
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
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
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
                    _logger.LogInformation("ProjectTypesAssets added, ProjectTypesAssets: {ProjectTypesAssetsData}, User: {User}", new { ProjectTypesAssetsId = projectTypesAssets.ProjectTypesAssetsId, NameAr = projectTypesAssets.NameAr, NameEn = projectTypesAssets.NameEn, ProjectTypeId = projectTypesAssets.ProjectTypeId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                    // When the view just returned the Index the category items weren't shown, That's because we didn't pass-in
                    // the ProjectTypeId to select from that's used in the Index action method
                    return RedirectToAction(nameof(Index), new { ProjectTypeId = projectTypesAssets.ProjectTypeId });
                }
                TempData["ProjectTypesAssetsError"] = "ProjectTypesAssets";
                return RedirectToAction(nameof(Index), new { ProjectTypeId = projectTypesAssets.ProjectTypeId });
            }
            catch (Exception ex)
            {
                _logger.LogError("ProjectTypesAssets not added, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
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
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
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
                        _context.Update(projectTypesAssets);
                        await _context.SaveChangesAsync();
                        TempData["ProjectTypesAssets"] = "ProjectTypesAssets";
                        _logger.LogInformation("ProjectTypesAssets added, ProjectTypesAssets: {ProjectTypesAssetsData}, User: {User}", new { ProjectTypesAssetsId = projectTypesAssets.ProjectTypesAssetsId, NameAr = projectTypesAssets.NameAr, NameEn = projectTypesAssets.NameEn, ProjectTypeId = projectTypesAssets.ProjectTypeId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProjectTypesAssetsExists(projectTypesAssets.ProjectTypesAssetsId))
                        {
                            _logger.LogError("DbUpdateConcurrencyException Exception, ProjectTypesAssets not edited, ProjectTypesAssets: {ProjectTypesAssetsData}, User: {User}", new { ProjectTypesAssetsId = projectTypesAssets.ProjectTypesAssetsId, NameAr = projectTypesAssets.NameAr, NameEn = projectTypesAssets.NameEn, ProjectTypeId = projectTypesAssets.ProjectTypeId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                            TempData["ProjectTypesAssetsError"] = "ProjectTypesAssets";
                            return RedirectToAction(nameof(Index), new { ProjectTypeId = projectTypesAssets.ProjectTypeId });
                        }
                        else
                        {
                            _logger.LogError("DbUpdateConcurrencyException Exception, ProjectTypesAssets not edited, ProjectTypesAssets: {ProjectTypesAssetsData}, User: {User}", new { ProjectTypesAssetsId = projectTypesAssets.ProjectTypesAssetsId, NameAr = projectTypesAssets.NameAr, NameEn = projectTypesAssets.NameEn, ProjectTypeId = projectTypesAssets.ProjectTypeId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
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
                _logger.LogError("ProjectTypeAssets not edited, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
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
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
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
                _logger.LogInformation("ProjectTypesAssets canceled, ProjectTypesAssets: {ProjectTypesAssetsData}, User: {User}", new { ProjectTypesAssetsId = projectTypesAssets.ProjectTypesAssetsId, NameAr = projectTypesAssets.NameAr, NameEn = projectTypesAssets.NameEn, ProjectTypeId = projectTypesAssets.ProjectTypeId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                // When the view just returned the Index the category items weren't shown, That's because we didn't pass-in
                // the ProjectTypeId to select from that's used in the Index action method
                return RedirectToAction(nameof(Index), new { ProjectTypeId = projectTypesAssets.ProjectTypeId });
            }
            catch (Exception ex)
            {
                var projectTypesAssets = await _context.ProjectTypesAssets.FindAsync(id);
                _logger.LogError("ProjectTypesAssets not canceled, Message: {ErrorData}, User: {User}, ProjectTypesAssets: {ProjectTypesAssetsData}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() }, new { ProjectTypesAssetsId = projectTypesAssets.ProjectTypesAssetsId, NameAr = projectTypesAssets.NameAr, NameEn = projectTypesAssets.NameEn, ProjectTypeId = projectTypesAssets.ProjectTypeId });
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
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return false;
            }
        }
    }
}
