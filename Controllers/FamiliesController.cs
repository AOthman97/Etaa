namespace Etaa.Controllers
{
    public class FamiliesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FamiliesController> _logger;

        public FamiliesController(ApplicationDbContext context, ILogger<FamiliesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // For the create action to load the states first, then load the cities and districts
        public IActionResult CascadeList()
        {
            try
            {
                ViewBag.States = new SelectList(_context.States.AsNoTracking(), "StateId", "NameAr");
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
                var City = _context.Cities.Where(City => City.StateId == StateId).AsNoTracking();
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
                var District = _context.Districts.Where(District => District.CityId == CityId).AsNoTracking();
                return Json(new SelectList(District, "DistrictId", "NameAr"));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return Json(default);
            }
        }

        [Authorize]
        // GET: Families
        public async Task<IActionResult> Index()
        {
            try
            {
                var applicationDbContext = _context.Families.Where(f => f.IsCanceled == false).AsNoTracking();
                return View(await applicationDbContext.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // GET: Families/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var family = await _context.Families
                    .Include(f => f.AccommodationType)
                    .Include(f => f.District)
                    .Include(f => f.EducationalStatus)
                    .Include(f => f.Gender)
                    .Include(f => f.HealthStatus)
                    .Include(f => f.InvestmentType)
                    .Include(f => f.Job)
                    .Include(f => f.MartialStatus)
                    .Include(f => f.Religion)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.FamilyId == id);
                if (family == null)
                {
                    return NotFound();
                }

                return View(family);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // GET: Families/Create
        public IActionResult Create()
        {
            try
            {
                ViewData["AccommodationTypeId"] = new SelectList(_context.AccommodationTypes, "AccommodationTypeId", "NameAr");
                ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "NameAr");
                ViewData["EducationalStatusId"] = new SelectList(_context.EducationalStatuses, "EducationalStatusId", "NameAr");
                ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "NameAr");
                ViewData["HealthStatusId"] = new SelectList(_context.HealthStatuses, "HealthStatusId", "NameAr");
                ViewData["InvestmentTypeId"] = new SelectList(_context.InvestmentTypes, "InvestmentTypeId", "NameAr");
                ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "NameAr");
                ViewData["MartialStatusId"] = new SelectList(_context.MartialStatuses, "MartialStatusId", "NameAr");
                ViewData["ReligionId"] = new SelectList(_context.Religions, "ReligionId", "NameAr");

                var StatesList = new SelectList(_context.States.AsNoTracking().ToList(), "StateId", "NameAr");
                ViewData["States"] = StatesList;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // POST: Families/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // ,UserId,ManagementUserId
        public async Task<IActionResult> Create([Bind("FamilyId,NameAr,NameEn,Address,HouseNumber,Alleyway,ResidentialSquare,FirstPhoneNumber,SecondPhoneNumber,NationalNumber,PassportNumber,NumberOfIndividuals,Age,MonthlyIncome,IsCurrentInvestmentProject,IsApprovedByManagement,IsCanceled,DateOfBirth,DistrictId,GenderId,ReligionId,MartialStatusId,JobId,HealthStatusId,EducationalStatusId,AccommodationTypeId,InvestmentTypeId,DistrictName,HealthCondition")] Family family)
        {
            try
            {
                var userId = User.GetLoggedInUserId<string>();
                
                if (ModelState.IsValid)
                {
                    TempData["Family"] = family.NameAr;
                    family.IsCanceled = false;
                    family.IsApprovedByManagement = false;
                    family.UserId = userId;
                    _context.Add(family);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Family added, Family: {FamilyData}, User: {User}", new { FamilyId = family.FamilyId, NameAr = family.NameAr, NameEn = family.NameEn }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["AccommodationTypeId"] = new SelectList(_context.AccommodationTypes, "AccommodationTypeId", "NameAr", family.AccommodationTypeId);
                    ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "NameAr", family.DistrictId);
                    ViewData["EducationalStatusId"] = new SelectList(_context.EducationalStatuses, "EducationalStatusId", "NameAr", family.EducationalStatusId);
                    ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "NameAr", family.GenderId);
                    ViewData["HealthStatusId"] = new SelectList(_context.HealthStatuses, "HealthStatusId", "NameAr", family.HealthStatusId);
                    ViewData["InvestmentTypeId"] = new SelectList(_context.InvestmentTypes, "InvestmentTypeId", "NameAr", family.InvestmentTypeId);
                    ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "NameAr", family.JobId);
                    ViewData["MartialStatusId"] = new SelectList(_context.MartialStatuses, "MartialStatusId", "NameAr", family.MartialStatusId);
                    ViewData["ReligionId"] = new SelectList(_context.Religions, "ReligionId", "NameAr", family.ReligionId);
                    TempData["FamilyError"] = family.NameAr;
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Family not added, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                TempData["FamilyError"] = family.NameAr;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Families/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var family = await _context.Families.FindAsync(id);
                ViewData["FamilyMembers"] = await _context.FamilyMembers.Where(f => f.FamilyId == id).ToListAsync();
                if (family == null)
                {
                    return NotFound();
                }
                ViewData["AccommodationTypeId"] = new SelectList(_context.AccommodationTypes, "AccommodationTypeId", "NameAr", family.AccommodationTypeId);
                ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "NameAr", family.DistrictId);
                ViewData["EducationalStatusId"] = new SelectList(_context.EducationalStatuses, "EducationalStatusId", "NameAr", family.EducationalStatusId);
                ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "NameAr", family.GenderId);
                ViewData["HealthStatusId"] = new SelectList(_context.HealthStatuses, "HealthStatusId", "NameAr", family.HealthStatusId);
                ViewData["InvestmentTypeId"] = new SelectList(_context.InvestmentTypes, "InvestmentTypeId", "NameAr", family.InvestmentTypeId);
                ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "NameAr", family.JobId);
                ViewData["MartialStatusId"] = new SelectList(_context.MartialStatuses, "MartialStatusId", "NameAr", family.MartialStatusId);
                ViewData["ReligionId"] = new SelectList(_context.Religions, "ReligionId", "NameAr", family.ReligionId);
                //ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", family.UserId);

                var StatesList = new SelectList(_context.States.ToList(), "StateId", "NameAr");
                ViewData["States"] = StatesList;
                return View(family);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // POST: Families/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FamilyId,NameAr,NameEn,Address,HouseNumber,Alleyway,ResidentialSquare,FirstPhoneNumber,SecondPhoneNumber,NationalNumber,PassportNumber,NumberOfIndividuals,Age,MonthlyIncome,IsCurrentInvestmentProject,IsApprovedByManagement,IsCanceled,DateOfBirth,DistrictId,GenderId,ReligionId,MartialStatusId,JobId,HealthStatusId,EducationalStatusId,AccommodationTypeId,InvestmentTypeId,UserId,ManagementUserId,DistrictName,HealthCondition")] Family family)
        {
            try
            {
                if (id != family.FamilyId)
                {
                    return NotFound();
                }

                TempData["Family"] = family.NameAr;
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(family);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation("Family edited, Family: {FamilyData}, User: {User}", new { FamilyId = family.FamilyId, NameAr = family.NameAr, NameEn = family.NameEn }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!FamilyExists(family.FamilyId))
                        {
                            _logger.LogError("DbUpdateConcurrencyException Exception, Family not edited, Family: {FamilyData}, User: {User}", new { FamilyId = family.FamilyId, NameAr = family.NameAr, NameEn = family.NameEn }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                            return View("Edit");
                        }
                        else
                        {
                            _logger.LogError("DbUpdateConcurrencyException Exception, Family not edited, Family: {FamilyData}, User: {User}", new { FamilyId = family.FamilyId, NameAr = family.NameAr, NameEn = family.NameEn }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                            return View("Edit");
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                
                ViewData["AccommodationTypeId"] = new SelectList(_context.AccommodationTypes, "AccommodationTypeId", "NameAr", family.AccommodationTypeId);
                ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "NameAr", family.DistrictId);
                ViewData["EducationalStatusId"] = new SelectList(_context.EducationalStatuses, "EducationalStatusId", "NameAr", family.EducationalStatusId);
                ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "NameAr", family.GenderId);
                ViewData["HealthStatusId"] = new SelectList(_context.HealthStatuses, "HealthStatusId", "NameAr", family.HealthStatusId);
                ViewData["InvestmentTypeId"] = new SelectList(_context.InvestmentTypes, "InvestmentTypeId", "NameAr", family.InvestmentTypeId);
                ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "NameAr", family.JobId);
                ViewData["MartialStatusId"] = new SelectList(_context.MartialStatuses, "MartialStatusId", "NameAr", family.MartialStatusId);
                ViewData["ReligionId"] = new SelectList(_context.Religions, "ReligionId", "NameAr", family.ReligionId);
                ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", family.UserId);
                TempData["FamilyError"] = "FamilyError";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError("Family not edited, Message: {ErrorData}, User: {User}, Family: {FamilyData}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() }, new { FamilyId = family.FamilyId, NameAr = family.NameAr, NameEn = family.NameEn });
                TempData["FamilyError"] = "FamilyError";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Families/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var family = await _context.Families
                    .Include(f => f.AccommodationType)
                    .Include(f => f.District)
                    .Include(f => f.EducationalStatus)
                    .Include(f => f.Gender)
                    .Include(f => f.HealthStatus)
                    .Include(f => f.InvestmentType)
                    .Include(f => f.Job)
                    .Include(f => f.MartialStatus)
                    .Include(f => f.Religion)
                    .FirstOrDefaultAsync(m => m.FamilyId == id);
                if (family == null)
                {
                    return NotFound();
                }

                return View(family);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // POST: Families/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromForm] int FamilyId)
        {
            try
            {
                var family = await _context.Families.FindAsync(FamilyId);

                if(family != null)
                {
                    family.IsCanceled = true;
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Family canceled, Family: {FamilyData}, User: {User}", new { FamilyId = family.FamilyId, NameAr = family.NameAr, NameEn = family.NameEn }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                    TempData["Family"] = family.NameAr;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["FamilyError"] = "FamilyError";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                var family = await _context.Families.FindAsync(FamilyId);
                _logger.LogError("Family not canceled, Message: {ErrorData}, User: {User}, Family: {FamilyData}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() }, new { FamilyId = family.FamilyId, NameAr = family.NameAr, NameEn = family.NameEn });
                TempData["FamilyError"] = "FamilyError";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool FamilyExists(int id)
        {
            try
            {
                return _context.Families.Any(e => e.FamilyId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return false;
            }
        }
    }
}
