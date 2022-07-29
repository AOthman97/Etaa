namespace Etaa.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _hostingEnv;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<ProjectController> logger)
        {
            _context = context;
            _hostingEnv = webHostEnvironment;
            _logger = logger;
        }

        // This action and the below are for the autocomplete functionality to firstly select the Project and get the ProjectID
        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            try
            {
                var Project = (from project in this._context.Projects
                               where project.NameEn.StartsWith(prefix) ||
                               project.NameAr.StartsWith(prefix)
                               select new
                               {
                                   label = project.NameAr,
                                   val = project.ProjectId
                               }).AsNoTracking().ToList();

                return Json(Project);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return Json(default);
            }
        }

        [Authorize]
        // GET: Project
        public async Task<IActionResult> Index()
        {
            try
            {
                // .Include(p => p.IdentityUser)
                var applicationDbContext = _context.Projects.AsNoTracking();
                return View(await applicationDbContext.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // GET: Project/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // .Include(p => p.IdentityUser)
                var projects = await _context.Projects
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.ProjectId == id);
                if (projects == null)
                {
                    return NotFound();
                }

                //HttpContext.Session.SetString("filePath", projects.SignatureofApplicantPath);
                return View(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // GET: Project/Create
        public IActionResult Create()
        {
            try
            {
                ViewData["UserId"] = new SelectList(_context.IdentityUser.AsNoTracking(), "UserId", "NameAr");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        public async Task<JsonResult> GetProjectSelectionReasons()
        {
            try
            {
                var Result = new MultiSelectList(await _context.ProjectSelectionReasons.AsNoTracking().ToListAsync(), "ProjectSelectionReasonsId", "NameAr");
                return Json(Result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return Json(default);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetProjectSelectionReasons(int ProjectId)
        {
            try
            {
                IEnumerable SelectedValues = _context.ProjectsSelectionReasons.Where(p => p.ProjectId == ProjectId).AsNoTracking().Select(p => p.ProjectSelectionReasonsId);
                var Result = new MultiSelectList(await _context.ProjectSelectionReasons.AsNoTracking().ToListAsync(), "ProjectSelectionReasonsId", "NameAr", SelectedValues);
                return Json(Result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return Json(default);
            }
        }

        public async Task<JsonResult> GetProjectSocialBenefits()
        {
            try
            {
                var Result = new MultiSelectList(await _context.ProjectSocialBenefits.AsNoTracking().ToListAsync(), "ProjectSocialBenefitsId", "NameAr");
                return Json(Result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return Json(default);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetProjectSocialBenefits(int ProjectId)
        {
            try
            {
                IEnumerable SelectedValues = _context.ProjectsSocialBenefits.Where(p => p.ProjectId == ProjectId).AsNoTracking().Select(p => p.ProjectSocialBenefitsId);
                var Result = new MultiSelectList(await _context.ProjectSocialBenefits.AsNoTracking().ToListAsync(), "ProjectSocialBenefitsId", "NameAr", SelectedValues);
                return Json(Result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return Json(default);
            }
        }

        public async Task<JsonResult> GetProjectTypes()
        {
            try
            {
                var Result = new SelectList(await _context.ProjectTypes.AsNoTracking().ToListAsync(), "ProjectTypeId", "NameAr");
                return Json(Result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return Json(default);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetProjectTypes(int ProjectId)
        {
            try
            {
                ProjectTypes projectTypes = new ProjectTypes();
                projectTypes.ProjectTypeId = _context.Projects.Where(p => p.ProjectId == ProjectId).AsNoTracking().Select(p => p.ProjectTypeId).SingleOrDefault();
                var Result = new SelectList(await _context.ProjectTypes.AsNoTracking().ToListAsync(), "ProjectTypeId", "NameAr", projectTypes.ProjectTypeId);
                return Json(Result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return Json(default);
            }
        }

        public async Task<JsonResult> GetProjectTypeAssets(int ProjectTypeId)
        {
            try
            {
                var Result = await _context.ProjectTypesAssets.Where(ProjectTypeAsset => ProjectTypeAsset.ProjectTypeId == ProjectTypeId).AsNoTracking().ToListAsync();
                return Json(Result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return Json(default);
            }
        }

        public async Task<JsonResult> GetProjectTypeAssetsForEdit(int ProjectId)
        {
            try
            {
                //var Result = await _context.ProjectTypesAssets.Where(ProjectTypeAsset => ProjectTypeAsset.ProjectTypeId == ProjectTypeId).ToListAsync();
                var Result = await _context.ProjectAssetesProjectTypeAssets.Where(ProjectsAssets => ProjectsAssets.ProjectId == ProjectId).AsNoTracking().ToListAsync();
                return Json(Result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return Json(default);
            }
        }

        public async Task<JsonResult> GetNumberOfFunds()
        {
            try
            {
                var Result = new SelectList(await _context.NumberOfFunds.AsNoTracking().ToListAsync(), "NumberOfFundsId", "NameAr");
                return Json(Result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return Json(default);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetNumberOfFunds(int ProjectId)
        {
            try
            {
                NumberOfFunds numberOfFunds = new NumberOfFunds();
                numberOfFunds.NumberOfFundsId = _context.Projects.Where(p => p.ProjectId == ProjectId).AsNoTracking().Select(p => p.NumberOfFundsId).SingleOrDefault();
                var Result = new SelectList(await _context.NumberOfFunds.AsNoTracking().ToListAsync(), "NumberOfFundsId", "NameAr", numberOfFunds.NumberOfFundsId);
                return Json(Result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return Json(default);
            }
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
        //    ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", projects.UserId);
        //    return View(projects);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Projects project, List<ProjectsAssets> projectsAssets, List<ProjectsSelectionReasons> projectsSelectionReasons, List<ProjectsSocialBenefits> projectsSocialBenefits)
        {
            try
            {
                var filePath = HttpContext.Session.GetString("filePath");
                HttpContext.Session.Clear();

                decimal? Capital = (decimal?)project.Capital;
                decimal? MonthlyInstallmentAmount = (decimal?)project.MonthlyInstallmentAmount;
                int? NumberOfInstallments = (int?)project.NumberOfInstallments;
                int MaxInstallmentsNo = _context.Installments.Select(i => i.InstallmentsId).Max();

                if(NumberOfInstallments > MaxInstallmentsNo)
                {
                    TempData["NumberOfInstallmentsGreaterThanMaxInstallmentNo"] = "Project";
                    var RedirectURL = Url.Action(nameof(Create), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", project.UserId));
                    return Json(new
                    {
                        redirectUrl = RedirectURL
                    });
                }
                else if(Capital / MonthlyInstallmentAmount != NumberOfInstallments)
                {
                    TempData["CapitalDividedByMonthlyInstallmentAmountNotEqualToNumberOfInstallments"] = "Project";
                    var RedirectURL = Url.Action(nameof(Create), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", project.UserId));
                    return Json(new
                    {
                        redirectUrl = RedirectURL
                    });
                }
                else if (project.FamilyId.Equals(null) || project.FamilyId.Equals(0))
                {
                    TempData["ChooseFamily"] = "Project";
                    var RedirectURL = Url.Action(nameof(Create), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", project.UserId));
                    return Json(new
                    {
                        redirectUrl = RedirectURL
                    });
                }
                else if (project.NumberOfFundsId.Equals(null) || project.NumberOfFundsId.Equals(0))
                {
                    TempData["ChooseFundsNumber"] = "Project";
                    var RedirectURL = Url.Action(nameof(Create), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", project.UserId));
                    return Json(new
                    {
                        redirectUrl = RedirectURL
                    });
                }
                else if (project.FirstInstallmentDueDate.Equals(null))
                {
                    TempData["FirstInstallmentDueDate"] = "Project";
                    var RedirectURL = Url.Action(nameof(Create), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", project.UserId));
                    return Json(new
                    {
                        redirectUrl = RedirectURL
                    });
                }
                else
                {
                    if(!string.IsNullOrEmpty(filePath))
                        project.SignatureofApplicantPath = filePath;
                    var userId = User.GetLoggedInUserId<string>();
                    project.UserId = userId;
                    var ProjectTypeNameEn = (from projectType in _context.ProjectTypes
                                             where projectType.ProjectTypeId == project.ProjectTypeId
                                             select projectType.NameEn).Single();
                    var ProjectTypeNameAr = (from projectType in _context.ProjectTypes
                                             where projectType.ProjectTypeId == project.ProjectTypeId
                                             select projectType.NameAr).Single();
                    var FamilyNameEn = (from family in _context.Families
                                        where family.FamilyId == project.FamilyId
                                        select family.NameEn).Single();
                    var FamilyNameAr = (from family in _context.Families
                                        where family.FamilyId == project.FamilyId
                                        select family.NameAr).Single();
                    project.NameEn = String.Concat(ProjectTypeNameEn, " ", FamilyNameEn);
                    project.NameAr = String.Concat(ProjectTypeNameAr, " ", FamilyNameAr);

                    using var transaction = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        await _context.Projects.AddAsync(project);
                        await _context.SaveChangesAsync();
                        List<ProjectsAssets> projectsAssetsList = new List<ProjectsAssets>();
                        foreach (var item in projectsAssets)
                        {
                            if (item.Quantity > 0 && item.Amount > 0)
                            {
                                item.ProjectId = project.ProjectId;
                                projectsAssetsList.Add(new ProjectsAssets { Amount = item.Amount, ProjectId = item.ProjectId, ProjectsAssetsId = item.ProjectsAssetsId, ProjectTypesAssetsId = item.ProjectTypesAssetsId, Quantity = item.Quantity });
                                await _context.ProjectsAssets.AddAsync(item);
                            }
                        }

                        List<ProjectsSelectionReasons> projectsSelectionReasonsList = new List<ProjectsSelectionReasons>();
                        foreach (var item in projectsSelectionReasons)
                        {
                            item.ProjectId = project.ProjectId;
                            projectsSelectionReasonsList.Add(new ProjectsSelectionReasons { ProjectId = item.ProjectId, ProjectSelectionReasonsId = item.ProjectSelectionReasonsId, ProjectsSelectionReasonsId = item.ProjectsSelectionReasonsId });
                            await _context.ProjectsSelectionReasons.AddAsync(item);
                        }

                        List<ProjectsSocialBenefits> projectsSocialBenefitsList = new List<ProjectsSocialBenefits>();
                        foreach (var item in projectsSocialBenefits)
                        {
                            item.ProjectId = project.ProjectId;
                            projectsSocialBenefitsList.Add(new ProjectsSocialBenefits { ProjectId = item.ProjectId, ProjectSocialBenefitsId = item.ProjectSocialBenefitsId, ProjectsSocialBenefitsId = item.ProjectsSocialBenefitsId });
                            await _context.ProjectsSocialBenefits.AddAsync(item);
                        }

                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        _logger.LogInformation("Project added, Project: {ProjectData}, User: {User}", new { ProjectId = project.ProjectId, NameAr = project.NameAr, NameEn = project.NameEn, ProjectTypeId = project.ProjectTypeId, Capital = project.Capital, NumberOfInstallments = project.NumberOfInstallments, MonthlyInstallmentAmount = project.MonthlyInstallmentAmount, NumberOfFunds = project.NumberOfFundsId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                        projectsAssetsList.ForEach(x => _logger.LogInformation("Project assets added for project, Project assets: {ProjectAssetsData}, User: {User}", new { ProjectsAssetsId = x.ProjectsAssetsId, ProjectId = project.ProjectId, Quantity = x.Quantity, Amount = x.Amount, ProjectTypesAssetsId = x.ProjectTypesAssetsId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() }));
                        projectsSelectionReasonsList.ForEach(x => _logger.LogInformation("Project selection reasons added for project, Project selection reasons: {ProjectSelectionReasonsData}, User: {User}", new { ProjectId = project.ProjectId, ProjectSelectionReasonsId = x.ProjectSelectionReasonsId, ProjectsSelectionReasonsId = x.ProjectsSelectionReasonsId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() }));
                        projectsSocialBenefitsList.ForEach(x => _logger.LogInformation("Project social benfits added for project, Project social benefits: {ProjectSocialBenefitsData}, User: {User}", new { ProjectId = project.ProjectId, ProjectSocialBenefitsId = x.ProjectSocialBenefitsId, ProjectsSocialBenefitsId = x.ProjectsSocialBenefitsId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() }));

                        TempData["Project"] = "Project";
                        var RedirectURLThird = Url.Action(nameof(Index));
                        return Json(new
                        {
                            redirectUrl = RedirectURLThird
                        });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.LogError("Project not added, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                        TempData["ProjectError"] = "Project";
                        var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", project.UserId));
                        return Json(new
                        {
                            redirectUrl = RedirectURL
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Project not added, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                TempData["ProjectError"] = "Project";
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", project.UserId));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
        }

        // GET: Project/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
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
                ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", projects.UserId);
                ViewData["FamilyNameAr"] = _context.Families.Where(f => f.FamilyId == projects.FamilyId).Select(f => f.NameAr).Single();
                //HttpContext.Session.SetString("filePath", projects.SignatureofApplicantPath);
                return View(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int projectId, Projects projects, List<ProjectsAssets> projectsAssets, List<ProjectsSelectionReasons> projectsSelectionReasons, List<ProjectsSocialBenefits> projectsSocialBenefits)
        {
            try
            {
                // In each of these models create actions you should check first if there's no not canceled rows that have
                // the same ProjectId in them
                var FinancialStatemntId = _context.FinancialStatements.Where(f => f.ProjectId == projectId && f.IsCanceled == false).Select(f => f.FinancialStatementId);
                var ClearanceId = _context.Clearances.Where(c => c.ProjectId == projectId && c.IsCanceled == false).Select(c => c.ClearanceId);
                var PaymentVoucherId = _context.PaymentVouchers.Where(p => p.ProjectId == projectId && p.IsCanceled == false).Select(p => p.PaymentVoucherId);
                // This is a business validation
                if (projectId != projects.ProjectId || FinancialStatemntId.Any())
                {
                    TempData["FinancialStatemntId"] = "FinancialStatemntId";
                    var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", projects.UserId));
                    return Json(new
                    {
                        redirectUrl = RedirectURL
                    });
                }
                else if (ClearanceId.Any())
                {
                    TempData["ClearanceId"] = "ClearanceId";
                    var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", projects.UserId));
                    return Json(new
                    {
                        redirectUrl = RedirectURL
                    });
                }
                else if (PaymentVoucherId.Any())
                {
                    TempData["PaymentVoucherId"] = "PaymentVoucherId";
                    var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", projects.UserId));
                    return Json(new
                    {
                        redirectUrl = RedirectURL
                    });
                }
                else
                {
                    try
                    {
                        decimal? Capital = (decimal?)projects.Capital;
                        decimal? MonthlyInstallmentAmount = (decimal?)projects.MonthlyInstallmentAmount;
                        int? NumberOfInstallments = (int?)projects.NumberOfInstallments;
                        int MaxInstallmentsNo = _context.Installments.Select(i => i.InstallmentsId).Max();

                        if (NumberOfInstallments > MaxInstallmentsNo)
                        {
                            TempData["NumberOfInstallmentsGreaterThanMaxInstallmentNo"] = "Project";
                            var RedirectURL = Url.Action(nameof(Create), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", projects.UserId));
                            return Json(new
                            {
                                redirectUrl = RedirectURL
                            });
                        }
                        else if (Capital / MonthlyInstallmentAmount != NumberOfInstallments)
                        {
                            TempData["CapitalDividedByMonthlyInstallmentAmountNotEqualToNumberOfInstallments"] = "Project";
                            var RedirectURL = Url.Action(nameof(Create), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", projects.UserId));
                            return Json(new
                            {
                                redirectUrl = RedirectURL
                            });
                        }
                        else if (projects.FamilyId.Equals(null) || projects.FamilyId.Equals(0))
                        {
                            TempData["ChooseFamily"] = "Project";
                            var RedirectURL = Url.Action(nameof(Create), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", projects.UserId));
                            return Json(new
                            {
                                redirectUrl = RedirectURL
                            });
                        }
                        else if (projects.NumberOfFundsId.Equals(null) || projects.NumberOfFundsId.Equals(0))
                        {
                            TempData["ChooseFundsNumber"] = "Project";
                            var RedirectURL = Url.Action(nameof(Create), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", projects.UserId));
                            return Json(new
                            {
                                redirectUrl = RedirectURL
                            });
                        }

                        // If the session value exists then the user has added a file to update instead of the existing file
                        var NewFilePath = HttpContext.Session.GetString("filePath");
                        var Check = ViewData["filePath"];
                        ViewData["filePath"] = null;
                        if (NewFilePath != null)
                        {
                            HttpContext.Session.Clear();
                            var OldFilePath = "";
                            OldFilePath = _context.Projects.Where(f => f.ProjectId == projects.ProjectId).Select(f => f.SignatureofApplicantPath).Single();
                            if(OldFilePath != null && !string.IsNullOrEmpty(OldFilePath))
                            {
                                FileInfo file = new FileInfo(OldFilePath);
                                if (file.Exists)
                                {
                                    file.Delete();
                                }
                            }
                            projects.SignatureofApplicantPath = NewFilePath;
                        }

                        string ProjectTypeNameEn = (from projectType in _context.ProjectTypes
                                                    where projectType.ProjectTypeId == projects.ProjectTypeId
                                                    select projectType.NameEn).Single();
                        string ProjectTypeNameAr = (from projectType in _context.ProjectTypes
                                                    where projectType.ProjectTypeId == projects.ProjectTypeId
                                                    select (string)projectType.NameAr).Single();
                        string FamilyNameEn = (from family in _context.Families
                                               where family.FamilyId == projects.FamilyId
                                               select (string)family.NameEn).Single();
                        string FamilyNameAr = (from family in _context.Families
                                               where family.FamilyId == projects.FamilyId
                                               select (string)family.NameAr).Single();
                        projects.NameEn = String.Concat(ProjectTypeNameEn, " ", FamilyNameEn);
                        projects.NameAr = String.Concat(ProjectTypeNameAr, " ", FamilyNameAr);

                        using var transaction = await _context.Database.BeginTransactionAsync();
                        try
                        {
                            _context.Update(projects);
                            await _context.SaveChangesAsync();

                            // For this model and the other two models instead of directly updating their rows and the hassle of it,
                            // Just delete the rows and add new rows with the new values
                            List<ProjectsAssets> projectsAssetsRemoved = await _context.ProjectsAssets.Where(p => p.ProjectId == projectId).ToListAsync();

                            foreach (var item in projectsAssetsRemoved)
                            {
                                _context.Remove(item);
                                //await _context.SaveChangesAsync();
                            }

                            List<ProjectsSelectionReasons> projectsSelectionReasonsRemoved = await _context.ProjectsSelectionReasons.Where(p => p.ProjectId == projectId).ToListAsync();

                            foreach (var item in projectsSelectionReasonsRemoved)
                            {
                                _context.Remove(item);
                                //await _context.SaveChangesAsync();
                            }

                            List<ProjectsSocialBenefits> projectsSocialBenefitsRemoved = await _context.ProjectsSocialBenefits.Where(p => p.ProjectId == projectId).ToListAsync();

                            foreach (var item in projectsSocialBenefitsRemoved)
                            {
                                _context.Remove(item);
                                //await _context.SaveChangesAsync();
                            }

                            List<ProjectsAssets> projectsAssetsList = new List<ProjectsAssets>();
                            foreach (var item in projectsAssets)
                            {
                                if (item.Quantity > 0 && item.Amount > 0)
                                {
                                    item.ProjectId = projects.ProjectId;
                                    _context.Add(item);
                                    //await _context.SaveChangesAsync();
                                    projectsAssetsList.Add(new ProjectsAssets { Amount = item.Amount, ProjectId = item.ProjectId, ProjectsAssetsId = item.ProjectsAssetsId, ProjectTypesAssetsId = item.ProjectTypesAssetsId, Quantity = item.Quantity });
                                }
                            }

                            List<ProjectsSelectionReasons> projectsSelectionReasonsList = new List<ProjectsSelectionReasons>();
                            foreach (var item in projectsSelectionReasons)
                            {
                                item.ProjectId = projects.ProjectId;
                                _context.Add(item);
                                //await _context.SaveChangesAsync();
                                projectsSelectionReasonsList.Add(new ProjectsSelectionReasons { ProjectId = item.ProjectId, ProjectSelectionReasonsId = item.ProjectSelectionReasonsId, ProjectsSelectionReasonsId = item.ProjectsSelectionReasonsId });
                            }

                            List<ProjectsSocialBenefits> projectsSocialBenefitsList = new List<ProjectsSocialBenefits>();
                            foreach (var item in projectsSocialBenefits)
                            {
                                item.ProjectId = projects.ProjectId;
                                _context.Add(item);
                                //await _context.SaveChangesAsync();
                                projectsSocialBenefitsList.Add(new ProjectsSocialBenefits { ProjectId = item.ProjectId, ProjectSocialBenefitsId = item.ProjectSocialBenefitsId, ProjectsSocialBenefitsId = item.ProjectsSocialBenefitsId });
                            }

                            await _context.SaveChangesAsync();

                            await transaction.CommitAsync();

                            _logger.LogInformation("Project edited, Project: {ProjectData}, User: {User}", new { ProjectId = projects.ProjectId, NameAr = projects.NameAr, NameEn = projects.NameEn, ProjectTypeId = projects.ProjectTypeId, Capital = projects.Capital, NumberOfInstallments = projects.NumberOfInstallments, MonthlyInstallmentAmount = projects.MonthlyInstallmentAmount, NumberOfFunds = projects.NumberOfFundsId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                            projectsAssetsList.ForEach(x => _logger.LogInformation("Project assets edited for project, Project assets: {ProjectAssetsData}, User: {User}", new { ProjectsAssetsId = x.ProjectsAssetsId, ProjectId = projects.ProjectId, Quantity = x.Quantity, Amount = x.Amount, ProjectTypesAssetsId = x.ProjectTypesAssetsId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() }));
                            projectsSelectionReasonsList.ForEach(x => _logger.LogInformation("Project selection reasons edited for project, Project selection reasons: {ProjectSelectionReasonsData}, User: {User}", new { ProjectId = projects.ProjectId, ProjectSelectionReasonsId = x.ProjectSelectionReasonsId, ProjectsSelectionReasonsId = x.ProjectsSelectionReasonsId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() }));
                            projectsSocialBenefitsList.ForEach(x => _logger.LogInformation("Project social benfits edited for project, Project social benefits: {ProjectSocialBenefitsData}, User: {User}", new { ProjectId = projects.ProjectId, ProjectSocialBenefitsId = x.ProjectSocialBenefitsId, ProjectsSocialBenefitsId = x.ProjectsSocialBenefitsId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() }));
                            TempData["Project"] = "Project";
                            //return RedirectToAction(nameof(Index));
                            var RedirectURLThird = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                            return Json(new
                            {
                                redirectUrl = RedirectURLThird
                            });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            _logger.LogError("Project not added, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                            TempData["ProjectError"] = "Project";
                            var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", projects.UserId));
                            return Json(new
                            {
                                redirectUrl = RedirectURL
                            });
                        }
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProjectsExists(projects.ProjectId))
                        {
                            _logger.LogError("DbUpdateConcurrencyException Exception, Project not edited, Project: {ProjectData}, User: {User}", new { ProjectId = projects.ProjectId, NameAr = projects.NameAr, NameEn = projects.NameEn, ProjectTypeId = projects.ProjectTypeId, Capital = projects.Capital, NumberOfInstallments = projects.NumberOfInstallments, MonthlyInstallmentAmount = projects.MonthlyInstallmentAmount, NumberOfFunds = projects.NumberOfFundsId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                            TempData["ProjectError"] = "ProjectError";
                            var RedirectURLSixth = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                            return Json(new
                            {
                                redirectUrl = RedirectURLSixth
                            });
                        }
                        else
                        {
                            _logger.LogError("DbUpdateConcurrencyException Exception, Project not edited, Project: {ProjectData}, User: {User}", new { ProjectId = projects.ProjectId, NameAr = projects.NameAr, NameEn = projects.NameEn, ProjectTypeId = projects.ProjectTypeId, Capital = projects.Capital, NumberOfInstallments = projects.NumberOfInstallments, MonthlyInstallmentAmount = projects.MonthlyInstallmentAmount, NumberOfFunds = projects.NumberOfFundsId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                            TempData["ProjectError"] = "ProjectError";
                            var RedirectURLSeventh = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                            return Json(new
                            {
                                redirectUrl = RedirectURLSeventh
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Project not edited, Message: {ErrorData}, Project: {ProjectData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { ProjectId = projects.ProjectId, NameAr = projects.NameAr, NameEn = projects.NameEn, ProjectTypeId = projects.ProjectTypeId, Capital = projects.Capital, NumberOfInstallments = projects.NumberOfInstallments, MonthlyInstallmentAmount = projects.MonthlyInstallmentAmount, NumberOfFunds = projects.NumberOfFundsId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                TempData["ProjectError"] = "Project";
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", projects.UserId));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
        }

        // GET: Project/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // .Include(p => p.IdentityUser)
                var projects = await _context.Projects
                    .FirstOrDefaultAsync(m => m.ProjectId == id);
                if (projects == null)
                {
                    return NotFound();
                }

                return View(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);
                // In each of these models create actions you should check first if there's no not canceled rows that have
                // the same ProjectId in them
                var FinancialStatemntId = _context.FinancialStatements.Where(f => f.ProjectId == id && f.IsCanceled == false).Select(f => f.FinancialStatementId);
                var ClearanceId = _context.Clearances.Where(c => c.ProjectId == id && c.IsCanceled == false).Select(c => c.ClearanceId);
                var PaymentVoucherId = _context.PaymentVouchers.Where(p => p.ProjectId == id && p.IsCanceled == false).Select(p => p.PaymentVoucherId);
                // This is a business validation
                if (FinancialStatemntId.Any())
                {
                    TempData["FinancialStatemntId"] = "FinancialStatemntId";
                    //var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", project.UserId));
                    //return Json(new
                    //{
                    //    redirectUrl = RedirectURL
                    //});
                    return RedirectToAction(nameof(Index));
                }
                else if (ClearanceId.Any())
                {
                    TempData["ClearanceId"] = "ClearanceId";
                    //var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", project.UserId));
                    //return Json(new
                    //{
                    //    redirectUrl = RedirectURL
                    //});
                    return RedirectToAction(nameof(Index));
                }
                else if (PaymentVoucherId.Any())
                {
                    TempData["PaymentVoucherId"] = "PaymentVoucherId";
                    //var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", project.UserId));
                    //return Json(new
                    //{
                    //    redirectUrl = RedirectURL
                    //});
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    project.IsCanceled = true;
                    await _context.SaveChangesAsync();  
                    _logger.LogInformation("Project canceled, Project: {ProjectData}, User: {User}", new { ProjectId = project.ProjectId, NameAr = project.NameAr, NameEn = project.NameEn, ProjectTypeId = project.ProjectTypeId, Capital = project.Capital, NumberOfInstallments = project.NumberOfInstallments, MonthlyInstallmentAmount = project.MonthlyInstallmentAmount, NumberOfFunds = project.NumberOfFundsId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                    TempData["Project"] = "Project";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                var project = await _context.Projects.FindAsync(id);
                _logger.LogError("Error, Project not canceled, Message: {ErrorData}, Project: {ProjectData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { ProjectId = project.ProjectId, NameAr = project.NameAr, NameEn = project.NameEn, ProjectTypeId = project.ProjectTypeId, Capital = project.Capital, NumberOfInstallments = project.NumberOfInstallments, MonthlyInstallmentAmount = project.MonthlyInstallmentAmount, NumberOfFunds = project.NumberOfFundsId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                TempData["ProjectError"] = "Project";
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
        }

        private bool ProjectsExists(int id)
        {
            try
            {
                return _context.Projects.Any(e => e.ProjectId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return false;
            }
        }

        [HttpPost]
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

                string FilePath = Path.Combine(_hostingEnv.WebRootPath, FileDic);

                if (!Directory.Exists(FilePath))
                    Directory.CreateDirectory(FilePath);
                string SubFilePath = Path.Combine(FilePath, SubFileDic);
                if (!Directory.Exists(SubFilePath))
                    Directory.CreateDirectory(SubFilePath);
                var fileName = file.FileName;

                string filePath = Path.Combine(SubFilePath, fileName);
                HttpContext.Session.SetString("filePath", filePath);
                ViewData["filePath"] = filePath;
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(fs);
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
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        public static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},  
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        // Get content type
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        //[HttpGet]
        public async Task<IActionResult> Download(int ProjectId)
        {
            try
            {
                //var fileSession = HttpContext.Session.GetString("filePath");
                //HttpContext.Session.Remove("filePath");
                
                var fileSession = await (from project in this._context.Projects
                                         where project.ProjectId.Equals(ProjectId)
                                         select (string?)project.SignatureofApplicantPath).SingleOrDefaultAsync();

                var fileName = fileSession;
                var fileExists = System.IO.File.Exists(fileName);
                if (fileExists)
                {
                    string FileExtension = GetContentType(fileName);
                    return PhysicalFile(fileName, FileExtension, fileName);
                    //return PhysicalFile(fileName, "application/pdf", fileName);
                }
                else
                {
                    return View("Error");
                }

                //var fileStream = System.IO.File.OpenRead(fileSession);
                //string FileExtension = GetContentType(fileSession);
                //return File(fileStream, FileExtension, fileSession);
                //return View("Error");

            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }
    }
}
