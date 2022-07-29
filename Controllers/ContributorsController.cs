namespace Etaa.Controllers
{
    public class ContributorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ContributorsController> _logger;

        public ContributorsController(ApplicationDbContext context, ILogger<ContributorsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // For the create action to load the states first, then load the cities and districts
        public IActionResult CascadeList()
        {
            try
            {
                ViewBag.States = new SelectList(_context.States, "StateId", "NameAr");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        [HttpGet]
        public JsonResult LoadCities(int StateId)
        {
            try
            {
                var City = _context.Cities.Where(City => City.StateId == StateId);
                return Json(new SelectList(City, "CityId", "NameAr"));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return Json(default);
            }
        }

        [HttpGet]
        public JsonResult LoadDistricts(int CityId)
        {
            try
            {
                var District = _context.Districts.Where(District => District.CityId == CityId);
                return Json(new SelectList(District, "DistrictId", "NameAr"));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return Json(default);
            }
        }

        [Authorize]
        // GET: Contributors
        public async Task<IActionResult> Index()
        {
            try
            {
                var applicationDbContext = _context.Contributors.Include(c => c.District).AsNoTracking();
                return View(await applicationDbContext.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // GET: Contributors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var contributor = await _context.Contributors
                    .Include(c => c.District)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.ContributorId == id);
                if (contributor == null)
                {
                    return NotFound();
                }

                return View(contributor);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // GET: Contributors/Create
        public IActionResult Create()
        {
            ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "NameAr");
            var StatesList = new SelectList(_context.States.ToList(), "StateId", "NameAr");
            ViewData["States"] = StatesList;
            return View();
        }

        // POST: Contributors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContributorId,NameAr,NameEn,Mobile,WhatsappMobile,Email,Address,StartDate,EndDate,MonthlyShareAmount,NumberOfShares,IsActive,IsCanceled,DistrictId")] Contributor contributor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contributor.IsCanceled = false;
                    _context.Add(contributor);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Contributor added, Contributor: {ContributorData}, User: {User}", new { ContributorId = contributor.ContributorId, NameAr = contributor.NameAr, NameEn = contributor.NameEn }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                    TempData["Contributor"] = "Contributor";
                    return RedirectToAction(nameof(Index));
                }
                ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "NameAr", contributor.DistrictId);
                TempData["ContributorError"] = "Contributor";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError("Contributor not added, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                TempData["ContributorError"] = "Contributor";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Contributors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var contributor = await _context.Contributors.FindAsync(id);
                if (contributor == null)
                {
                    return NotFound();
                }
                ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "NameAr", contributor.DistrictId);
                var StatesList = new SelectList(_context.States.ToList(), "StateId", "NameAr");
                ViewData["States"] = StatesList;
                return View(contributor);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // POST: Contributors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContributorId,NameAr,NameEn,Mobile,WhatsappMobile,Email,Address,StartDate,EndDate,MonthlyShareAmount,NumberOfShares,IsActive,IsCanceled,DistrictId")] Contributor contributor)
        {
            try
            {
                if (id != contributor.ContributorId)
                {
                    TempData["ContributorError"] = "Contributor";
                    return RedirectToAction(nameof(Index));
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(contributor);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation("Contributor edited, Contributor: {ContributorData}, User: {User}", new { ContributorId = contributor.ContributorId, NameAr = contributor.NameAr, NameEn = contributor.NameEn }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                        TempData["Contributor"] = "Contributor";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ContributorExists(contributor.ContributorId))
                        {
                            _logger.LogInformation("DbUpdateConcurrencyException Exception, Contributor not edited, Contributor: {ContributorData}, User: {User}", new { ContributorId = contributor.ContributorId, NameAr = contributor.NameAr, NameEn = contributor.NameEn }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                            TempData["ContributorError"] = "Contributor";
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            _logger.LogInformation("DbUpdateConcurrencyException Exception, Contributor not edited, Contributor: {ContributorData}, User: {User}", new { ContributorId = contributor.ContributorId, NameAr = contributor.NameAr, NameEn = contributor.NameEn }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                            TempData["ContributorError"] = "Contributor";
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "NameAr", contributor.DistrictId);
                return View("Edit");
            }
            catch (Exception ex)
            {
                _logger.LogError("Contributor not edited, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                TempData["ContributorError"] = "Contributor";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Contributors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var contributor = await _context.Contributors
                    .Include(c => c.District)
                    .FirstOrDefaultAsync(m => m.ContributorId == id);
                if (contributor == null)
                {
                    return NotFound();
                }

                return View(contributor);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // POST: Contributors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                TempData["Contributor"] = "Contributor";
                var contributor = await _context.Contributors.FindAsync(id);
                contributor.IsCanceled = true;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Contributor canceled, Contributor: {ContributorData}, User: {User}", new { ContributorId = contributor.ContributorId, NameAr = contributor.NameAr, NameEn = contributor.NameEn }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError("Contributor not canceled, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                TempData["ContributorError"] = "Contributor";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool ContributorExists(int id)
        {
            try
            {
                return _context.Contributors.Any(e => e.ContributorId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return false;
            }
        }
    }
}
