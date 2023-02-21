namespace Etaa.Controllers
{
    public class ProjectTypeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProjectTypeController> _logger;

        public ProjectTypeController(ApplicationDbContext context, ILogger<ProjectTypeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize]
        // GET: ProjectType
        public async Task<IActionResult> Index()
        {
            try
            {
                var applicationDbContext = _context.ProjectTypes.Where(p => p.IsCanceled == false).Include(p => p.ProjectDomainTypes).Include(p => p.ProjectGroup).AsNoTracking();
                return View(await applicationDbContext.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // GET: ProjectType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var projectTypes = await _context.ProjectTypes
                    .Include(p => p.ProjectDomainTypes)
                    .Include(p => p.ProjectGroup)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.ProjectTypeId == id);
                if (projectTypes == null)
                {
                    return NotFound();
                }

                return View(projectTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // GET: ProjectType/Create
        public IActionResult Create()
        {
            try
            {
                ViewData["ProjectDomainTypeId"] = new SelectList(_context.ProjectDomainTypes.AsNoTracking(), "ProjectDomainTypeId", "NameAr");
                ViewData["ProjectGroupId"] = new SelectList(_context.ProjectGroups.AsNoTracking(), "ProjectGroupId", "NameAr");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // POST: ProjectType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectTypeId,NameAr,NameEn,IsCanceled,ProjectDomainTypeId,ProjectGroupId")] ProjectTypes projectTypes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    projectTypes.IsCanceled = false;
                    _context.Add(projectTypes);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("ProjectType added, ProjectType: {ProjectTypeData}, User: {User}", new { ProjectTypeId = projectTypes.ProjectTypeId, NameAr = projectTypes.NameAr, NameEn = projectTypes.NameEn, ProjectDomainTypeId = projectTypes.ProjectDomainTypeId, ProjectGroupId = projectTypes.ProjectGroupId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                    TempData["ProjectType"] = "ProjectType";
                    return RedirectToAction(nameof(Index));
                }
                ViewData["ProjectDomainTypeId"] = new SelectList(_context.ProjectDomainTypes, "ProjectDomainTypeId", "NameAr", projectTypes.ProjectDomainTypeId);
                ViewData["ProjectGroupId"] = new SelectList(_context.ProjectGroups, "ProjectGroupId", "NameAr", projectTypes.ProjectGroupId);
                TempData["ProjectTypeError"] = "ProjectType";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError("ProjectType not added, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                TempData["ProjectTypeError"] = "ProjectType";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ProjectType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // POST: ProjectType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectTypeId,NameAr,NameEn,IsCanceled,ProjectDomainTypeId,ProjectGroupId")] ProjectTypes projectTypes)
        {
            try
            {
                if (id != projectTypes.ProjectTypeId)
                {
                    TempData["ProjectTypeError"] = "ProjectType";
                    return RedirectToAction(nameof(Index));
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(projectTypes);
                        await _context.SaveChangesAsync();
                        TempData["ProjectType"] = "ProjectType";
                        _logger.LogInformation("ProjectType edited, ProjectType: {ProjectTypeData}, User: {User}", new { ProjectTypeId = projectTypes.ProjectTypeId, NameAr = projectTypes.NameAr, NameEn = projectTypes.NameEn, ProjectDomainTypeId = projectTypes.ProjectDomainTypeId, ProjectGroupId = projectTypes.ProjectGroupId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProjectTypesExists(projectTypes.ProjectTypeId))
                        {
                            _logger.LogError("DbUpdateConcurrencyException Exception, ProjectType not edited, ProjectType: {ProjectTypeData}, User: {User}", new { ProjectTypeId = projectTypes.ProjectTypeId, NameAr = projectTypes.NameAr, NameEn = projectTypes.NameEn, ProjectDomainTypeId = projectTypes.ProjectDomainTypeId, ProjectGroupId = projectTypes.ProjectGroupId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                            TempData["ProjectTypeError"] = "ProjectType";
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            _logger.LogError("DbUpdateConcurrencyException Exception, ProjectType not edited, ProjectType: {ProjectTypeData}, User: {User}", new { ProjectTypeId = projectTypes.ProjectTypeId, NameAr = projectTypes.NameAr, NameEn = projectTypes.NameEn, ProjectDomainTypeId = projectTypes.ProjectDomainTypeId, ProjectGroupId = projectTypes.ProjectGroupId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                            TempData["ProjectTypeError"] = "ProjectType";
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                TempData["ProjectType"] = "ProjectType";
                ViewData["ProjectDomainTypeId"] = new SelectList(_context.ProjectDomainTypes, "ProjectDomainTypeId", "NameAr", projectTypes.ProjectDomainTypeId);
                ViewData["ProjectGroupId"] = new SelectList(_context.ProjectGroups, "ProjectGroupId", "NameAr", projectTypes.ProjectGroupId);
                TempData["ProjectTypeError"] = "ProjectType";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError("ProjectType not edited, Message: {ErrorData}, User: {User}, ProjectType: {ProjectTypeData}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() }, new { ProjectTypeId = projectTypes.ProjectTypeId, NameAr = projectTypes.NameAr, NameEn = projectTypes.NameEn, ProjectDomainTypeId = projectTypes.ProjectDomainTypeId, ProjectGroupId = projectTypes.ProjectGroupId });
                TempData["ProjectTypeError"] = "ProjectType";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ProjectType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // POST: ProjectType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                TempData["ProjectType"] = "ProjectType";
                var projectTypes = await _context.ProjectTypes.FindAsync(id);
                projectTypes.IsCanceled = true;
                await _context.SaveChangesAsync();
                _logger.LogInformation("ProjectType canceled, ProjectType: {ProjectTypeData}, User: {User}", new { ProjectTypeId = projectTypes.ProjectTypeId, NameAr = projectTypes.NameAr, NameEn = projectTypes.NameEn, ProjectDomainTypeId = projectTypes.ProjectDomainTypeId, ProjectGroupId = projectTypes.ProjectGroupId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var projectTypes = await _context.ProjectTypes.FindAsync(id);
                _logger.LogError("ProjectType not canceled, Message: {ErrorData}, User: {User}, ProjectType: {ProjectTypeData}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() }, new { ProjectTypeId = projectTypes.ProjectTypeId, NameAr = projectTypes.NameAr, NameEn = projectTypes.NameEn, ProjectDomainTypeId = projectTypes.ProjectDomainTypeId, ProjectGroupId = projectTypes.ProjectGroupId });
                TempData["ProjectTypeError"] = "ProjectType";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool ProjectTypesExists(int id)
        {
            try
            {
                return _context.ProjectTypes.Any(e => e.ProjectTypeId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return false;
            }
        }
    }
}
