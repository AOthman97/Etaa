namespace Etaa.Controllers
{
    public class FinancialStatementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment hostingEnv;
        private readonly ILogger<FinancialStatementController> _logger;

        public FinancialStatementController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<FinancialStatementController> logger)
        {
            _context = context;
            hostingEnv = webHostEnvironment;
            _logger = logger;
        }

        [Authorize]
        // GET: FinancialStatement
        public async Task<IActionResult> Index()
        {
            try
            {
                var applicationDbContext = _context.FinancialStatements.Include(f => f.Projects).AsNoTracking();
                return View(await applicationDbContext.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
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
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.FinancialStatementId == id);
                if (financialStatement == null)
                {
                    return NotFound();
                }

                return View(financialStatement);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // GET: FinancialStatement/Create
        public IActionResult Create()
        {
            try
            {
                ViewData["ProjectId"] = new SelectList(_context.Projects.AsNoTracking(), "ProjectId", "ProjectId");
                ViewData["UserId"] = new SelectList(_context.IdentityUser.AsNoTracking(), "UserId", "NameAr");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
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
        //    ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", financialStatement.UserId);
        //    return View(financialStatement);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FinancialStatement financialStatement)
        {
            try
            {
                var Project = _context.FinancialStatements.Where(f => f.ProjectId == financialStatement.ProjectId && f.IsCanceled == false).Select(f => f.ProjectId);
                if(Project.Any() == false)
                {
                    var userId = User.GetLoggedInUserId<string>();
                    financialStatement.UserId = userId;
                    var filePath = HttpContext.Session.GetString("filePath");
                    HttpContext.Session.Clear();
                    financialStatement.DocumentPath = filePath;
                    financialStatement.IsApprovedByManagement = false;
                    _context.Add(financialStatement);
                    await _context.SaveChangesAsync();
                    TempData["FinancialStatement"] = "FinancialStatement";
                    _logger.LogInformation("Financial statement added, Financial statement: {FinancialStatementData}, User: {User}", new { FinancialStatementId = financialStatement.FinancialStatementId, ProjectId = financialStatement.ProjectId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                    var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                    return Json(new
                    {
                        redirectUrl = RedirectURL
                    });
                }
                else
                {
                    TempData["ProjectAlreadyHasAFinancialStatement"] = "FinancialStatement";
                    var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                    return Json(new
                    {
                        redirectUrl = RedirectURL
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Financial statement not added, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                TempData["FinancialStatementError"] = "FinancialStatement";
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
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
                ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", financialStatement.UserId);
                ViewData["ProjectNameAr"] = _context.Projects.Where(f => f.ProjectId == financialStatement.ProjectId).Select(f => f.NameAr).Single();
                return View(financialStatement);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // POST: FinancialStatement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int financialStatementId, [Bind("FinancialStatementId,DocumentPath,IsApprovedByManagement,IsCanceled,ProjectId,UserId,ManagementUserId")] FinancialStatement financialStatement)
        {
            try
            {
                if (financialStatementId != financialStatement.FinancialStatementId)
                {
                    TempData["FinancialStatementError"] = "FinancialStatement";
                    var RedirectURLFirst = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                    return Json(new
                    {
                        redirectUrl = RedirectURLFirst
                    });
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        // If the session value exists then the user has added a file to update instead of the existing file
                        var NewFilePath = HttpContext.Session.GetString("filePath");
                        if (NewFilePath != null)
                        {
                            HttpContext.Session.Clear();
                            var OldFilePath = "";
                            OldFilePath = _context.FinancialStatements.Where(f => f.FinancialStatementId == financialStatement.FinancialStatementId).Select(f => f.DocumentPath).Single();
                            if(OldFilePath != null && !string.IsNullOrEmpty(OldFilePath))
                            {
                                FileInfo file = new FileInfo(OldFilePath);
                                if (file.Exists)
                                {
                                    file.Delete();
                                }
                            }
                            financialStatement.DocumentPath = NewFilePath;
                        }

                        _context.Update(financialStatement);
                        await _context.SaveChangesAsync();
                        TempData["FinancialStatement"] = "FinancialStatement";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!FinancialStatementExists(financialStatement.FinancialStatementId))
                        {
                            _logger.LogError("DbUpdateConcurrencyException Exception, Financial statement not edited, Financial statement: {FinancialStatementData}, User: {User}", new { FinancialStatementId = financialStatement.FinancialStatementId, ProjectId = financialStatement.ProjectId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                            TempData["FinancialStatementError"] = "FinancialStatement";
                            var RedirectURLSecond = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                            return Json(new
                            {
                                redirectUrl = RedirectURLSecond
                            });
                        }
                        else
                        {
                            _logger.LogError("DbUpdateConcurrencyException Exception, Financial statement not edited, Financial statement: {FinancialStatementData}, User: {User}", new { FinancialStatementId = financialStatement.FinancialStatementId, ProjectId = financialStatement.ProjectId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                            TempData["FinancialStatementError"] = "FinancialStatement";
                            var RedirectURLThird = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                            return Json(new
                            {
                                redirectUrl = RedirectURLThird
                            });
                        }
                    }
                    _logger.LogInformation("Financial statement edited, Financial statement: {FinancialStatementData}, User: {User}", new { FinancialStatementId = financialStatement.FinancialStatementId, ProjectId = financialStatement.ProjectId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                    var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                    return Json(new
                    {
                        redirectUrl = RedirectURL
                    });
                }
                ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", financialStatement.ProjectId);
                ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", financialStatement.UserId);
                TempData["FinancialStatementError"] = "FinancialStatement";
                var RedirectURLFourth = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURLFourth
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Financial statement not edited, Message: {ErrorData}, User: {User}, Financial statement: {FinancialStatementData}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() }, new { FinancialStatementId = financialStatement.FinancialStatementId, ProjectId = financialStatement.ProjectId });
                TempData["FinancialStatementError"] = "FinancialStatement";
                var RedirectURLFourth = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURLFourth
                });
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
                    .FirstOrDefaultAsync(m => m.FinancialStatementId == id);
                if (financialStatement == null)
                {
                    return NotFound();
                }

                return View(financialStatement);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
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
                var Project = _context.PaymentVouchers.Where(p => p.ProjectId == financialStatement.ProjectId && p.IsCanceled == false).Select(p => p.Projects);
                if (Project.Any() == false)
                {
                    financialStatement.IsCanceled = true;
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Financial statement canceled, Financial statement: {FinancialStatementData}, User: {User}", new { FinancialStatementId = financialStatement.FinancialStatementId, ProjectId = financialStatement.ProjectId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                    TempData["FinancialStatement"] = "FinancialStatement";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["CantDeleteTheresAProjectAttachedWitIt"] = "FinancialStatement";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                var financialStatement = await _context.FinancialStatements.FindAsync(id);
                _logger.LogError("Financial statement not canceled, Message: {ErrorData}, User: {User}, Financial statement: {FinancialStatementData}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() }, new { FinancialStatementId = financialStatement.FinancialStatementId, ProjectId = financialStatement.ProjectId });
                TempData["FinancialStatementError"] = "FinancialStatement";
                return RedirectToAction(nameof(Index));
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
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
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
                    await file.CopyToAsync(fs);
                }

                return RedirectToAction("Index");
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

        public async Task<IActionResult> Download(int FinancialStatementId)
        {
            try
            {
                var fileSession = await (from financialStatement in this._context.FinancialStatements
                                         where financialStatement.FinancialStatementId.Equals(FinancialStatementId)
                                         select (string?)financialStatement.DocumentPath).SingleOrDefaultAsync();

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
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }
    }
}
