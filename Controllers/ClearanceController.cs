﻿namespace Etaa.Controllers
{
    public class ClearanceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _hostingEnv;
        private readonly ILogger<ClearanceController> _logger;

        public ClearanceController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<ClearanceController> logger)
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
                               }).ToList();

                return Json(Project);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return Json(default);
            }
        }

        [HttpPost]
        public decimal GetRemainAmount(int projectId)
        {
            try
            {
                decimal SumPaidAmount = (from paymentVoucherVar in _context.PaymentVouchers
                                                         where paymentVoucherVar.ProjectId == projectId &&
                                                         paymentVoucherVar.IsCanceled == false
                                                         select (decimal)paymentVoucherVar.PaymentAmount).Sum();
                decimal Capital = (from project in _context.Projects
                                   where project.ProjectId == projectId
                                   select (decimal)project.Capital.GetValueOrDefault()).Single();

                return (Capital - SumPaidAmount);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return -1;
            }
        }

        [Authorize]
        // GET: Clearance
        public async Task<IActionResult> Index()
        {
            try
            {
                // .Include(c => c.IdentityUser)
                var applicationDbContext = _context.Clearances.Where(c => c.IsCanceled == false).Include(c => c.Projects).AsNoTracking();
                return View(await applicationDbContext.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // GET: Clearance/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null || _context.Clearances == null)
                {
                    return NotFound();
                }

                var clearance = await _context.Clearances
                    .Include(c => c.Projects)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.ClearanceId == id);
                if (clearance == null)
                {
                    return NotFound();
                }

                return View(clearance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // GET: Clearance/Create
        public IActionResult Create()
        {
            try
            {
                ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId");
                ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // POST: Clearance/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ClearanceId,ClearanceDocumentPath,Comments,ClearanceDate,IsApprovedByManagement,IsCanceled,ProjectId,UserId,ManagementUserId")] Clearance clearance)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _context.Add(clearance);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //        ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", clearance.ProjectId);
        //        ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", clearance.UserId);
        //        return View(clearance);
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("Error");
        //    }
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Clearance clearance)
        {
            try
            {
                var userId = User.GetLoggedInUserId<string>();
                var filePath = HttpContext.Session.GetString("filePath");
                HttpContext.Session.Clear();
                clearance.ClearanceDocumentPath = filePath;
                clearance.UserId = userId;

                // Firstly before saving the clearance we need to check if this project is fully paid
                int ProjectId = clearance.ProjectId;
                decimal Capital = (from project in _context.Projects
                                            where project.ProjectId == clearance.ProjectId
                                            select (decimal)project.Capital.GetValueOrDefault()).Single();

                decimal SumPaidAmount = (from paymentVoucherVar in _context.PaymentVouchers
                                                         where paymentVoucherVar.ProjectId == clearance.ProjectId &&
                                                         paymentVoucherVar.IsCanceled == false
                                                         select (decimal)paymentVoucherVar.PaymentAmount).Sum();

                var Project = _context.Clearances.Where(c => c.ProjectId == clearance.ProjectId).Select(c => c.ProjectId);
                if(Project.Any() == false)
                {
                    if (SumPaidAmount == Capital)
                    {
                        _context.Add(clearance);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation("Clearance added, Clearance: {ClearanceData}, User: {User}", new { ClearanceId = clearance.ClearanceId, ProjectId = clearance.ProjectId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                        TempData["Clearance"] = "Clearance";
                        var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                        return Json(new
                        {
                            redirectUrl = RedirectURL
                        });
                    }
                    else
                    {
                        TempData["PaidAmount"] = "Clearance";
                        var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                        return Json(new
                        {
                            redirectUrl = RedirectURL
                        });
                    }
                }
                else
                {
                    TempData["ProjectAlreadyHasAClearance"] = "Clearance";
                    var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                    return Json(new
                    {
                        redirectUrl = RedirectURL
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Clearance not added, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                TempData["ClearanceError"] = clearance.ClearanceDate;
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
        }

        // GET: Clearance/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null || _context.Clearances == null)
                {
                    return NotFound();
                }

                var clearance = await _context.Clearances.FindAsync(id);
                if (clearance == null)
                {
                    return NotFound();
                }
                ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", clearance.ProjectId);
                ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", clearance.UserId);
                ViewData["ProjectNameAr"] = _context.Projects.Where(f => f.ProjectId == clearance.ProjectId).Select(f => f.NameAr).Single();
                return View(clearance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // POST: Clearance/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int clearanceId, [Bind("ClearanceId,ClearanceDocumentPath,Comments,ClearanceDate,IsApprovedByManagement,IsCanceled,ProjectId,UserId,ManagementUserId")] Clearance clearance)
        {
            try
            {
                if (clearanceId != clearance.ClearanceId)
                {
                    var RedirectURLFourth = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                    return Json(new
                    {
                        redirectUrl = RedirectURLFourth
                    });
                }

                TempData["Clearance"] = "Clearance";
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
                            OldFilePath = _context.Clearances.Where(f => f.ClearanceId == clearance.ClearanceId).Select(f => f.ClearanceDocumentPath).Single();
                            if (OldFilePath != null && !string.IsNullOrEmpty(OldFilePath))
                            {
                                FileInfo file = new FileInfo(OldFilePath);
                                if (file.Exists)
                                {
                                    file.Delete();
                                }
                            }
                            clearance.ClearanceDocumentPath = NewFilePath;
                        }

                        _context.Update(clearance);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation("Clearance edited, Clearance: {ClearanceData}, User: {User}", new { ClearanceId = clearance.ClearanceId, ProjectId = clearance.ProjectId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ClearanceExists(clearance.ClearanceId))
                        {
                            _logger.LogError("DbUpdateConcurrencyException Exception, Clearance not edited, Clearance: {ClearanceData}, User: {User}", new { ClearanceId = clearance.ClearanceId, ProjectId = clearance.ProjectId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                            TempData["ClearanceError"] = clearance.ClearanceDate;
                            var RedirectURLSecond = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                            return Json(new
                            {
                                redirectUrl = RedirectURLSecond
                            });
                        }
                        else
                        {
                            _logger.LogError("DbUpdateConcurrencyException Exception, Clearance not edited, Clearance: {ClearanceData}, User: {User}", new { ClearanceId = clearance.ClearanceId, ProjectId = clearance.ProjectId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                            TempData["ClearanceError"] = clearance.ClearanceDate;
                            var RedirectURLThird = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                            return Json(new
                            {
                                redirectUrl = RedirectURLThird
                            });
                        }
                    }
                    var RedirectURLFifth = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                    return Json(new
                    {
                        redirectUrl = RedirectURLFifth
                    });
                }
                ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", clearance.ProjectId);
                ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", clearance.UserId);
                TempData["ClearanceError"] = clearance.ClearanceDate;
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Clearance not edited, Message: {ErrorData}, User: {User}, Clearance: {ClearanceData}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() }, new { ClearanceId = clearance.ClearanceId, ProjectId = clearance.ProjectId });
                TempData["ClearanceError"] = clearance.ClearanceDate;
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
        }

        // GET: Clearance/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null || _context.Clearances == null)
                {
                    return NotFound();
                }

                var clearance = await _context.Clearances
                    .Include(c => c.Projects)
                    .FirstOrDefaultAsync(m => m.ClearanceId == id);
                if (clearance == null)
                {
                    return NotFound();
                }

                return View(clearance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // POST: Clearance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromForm] int ClearanceId)
        {
            try
            {
                if (_context.Clearances == null)
                {
                    TempData["ClearanceError"] = "Clearance";
                    var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                    return Json(new
                    {
                        redirectUrl = RedirectURL
                    });
                }
                var clearance = await _context.Clearances.FindAsync(ClearanceId);
                
                if (clearance != null)
                {
                    clearance.IsCanceled = true;
                    //_context.Clearances.Remove(clearance);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Clearance canceled, Clearance: {ClearanceData}, User: {User}", new { ClearanceId = clearance.ClearanceId, ProjectId = clearance.ProjectId }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                    TempData["Clearance"] = "Clearance";
                    return RedirectToAction(nameof(Index));
                }
                else 
                {
                    TempData["ClearanceError"] = "Clearance";
                    var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                    return Json(new
                    {
                        redirectUrl = RedirectURL
                    });
                }
            }
            catch (Exception ex)
            {
                var clearance = await _context.Clearances.FindAsync(ClearanceId);
                _logger.LogError("Clearance not canceled, Message: {ErrorData}, User: {User}, Clearance: {ClearanceData}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() }, new { ClearanceId = clearance.ClearanceId, ProjectId = clearance.ProjectId });
                TempData["ClearanceError"] = "Clearance";
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
        }

        private bool ClearanceExists(int id)
        {
            try
            {
                return (_context.Clearances?.Any(e => e.ClearanceId == id)).GetValueOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return false;
            }
        }

        public IActionResult Upload(IFormFile file)
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
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
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

        //[HttpGet]
        public async Task<IActionResult> Download(int ClearanceId)
        {
            try
            {
                var fileSession = await (from clearance in this._context.Clearances
                                         where clearance.ClearanceId.Equals(ClearanceId)
                                         select (string?)clearance.ClearanceDocumentPath).SingleOrDefaultAsync();

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