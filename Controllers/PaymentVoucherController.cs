namespace Etaa.Controllers
{
    public class PaymentVoucherController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment hostingEnv;
        private readonly ILogger<PaymentVoucherController> _logger;

        public PaymentVoucherController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<PaymentVoucherController> logger)
        {
            _context = context;
            hostingEnv = webHostEnvironment;
            _logger = logger;
        }

        // Get Installments select list
        [HttpPost]
        public async Task<JsonResult> GetInstallments()
        {
            try
            {
                var Result = new SelectList(await _context.Installments.AsNoTracking().ToListAsync(), "InstallmentsId", "NameAr");
                return Json(Result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return Json(default);
            }
        }

        // Get the Installment no select item for current payment voucher
        public async Task<JsonResult> GetInstallmentForPaymentVoucher(int PaymentVoucherId)
        {
            try
            {
                // await _context.PaymentVouchers.Where(f => f.PaymentVoucherId == PaymentVoucherId).Select(f => f.InstallmentsId).ToListAsync()
                var InstallmentId = _context.PaymentVouchers.Where(p => p.PaymentVoucherId == PaymentVoucherId).AsNoTracking().Select(p => p.InstallmentsId).Single();
                var Result = new SelectList(await _context.Installments.Where(i => i.InstallmentsId == InstallmentId).AsNoTracking().ToListAsync(), "InstallmentsId", "NameAr");
                return Json(Result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return Json(default);
            }
        }

        // This action and the below are for the autocomplete functionality to firstly select the Project and get the ProjectID
        [HttpPost]  
        public JsonResult AutoComplete(string prefix)
        {
            try
            {
                var Project = (from project in _context.Projects
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

        [HttpPost]
        public IActionResult GetAllInstallments(int ProjectId)
        {
            try
            {
                // Variables for the datatable config
                //int totalRecord = 0;
                //int filterRecord = 0;
                //var draw = Request.Form["draw"].FirstOrDefault();
                //var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                //var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                //var searchValue = Request.Form["search[value]"].FirstOrDefault();
                //int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                //int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

                List<JsonResult> data = new List<JsonResult>();
                //List<string> data = new List<string>();
                JsonResult result;

                var ProjectNumberOfInstallments = (from project in _context.Projects
                                                   where project.ProjectId == ProjectId
                                                   select (int?)project.NumberOfInstallments).Single();
                List<string> Installments = new List<string>();

                for (int Increment = 1; Increment <= ProjectNumberOfInstallments; Increment++)
                {
                    var InstallmentName = (from Installment in _context.Installments
                                           where Installment.InstallmentNumber == Increment
                                           select (string)Installment.NameAr).AsNoTracking().Single();

                    Installments.Add(InstallmentName);

                    DateTime DueDate = DateTime.Now;

                    if (Increment == 1)
                    {
                        DueDate = (from project in _context.Projects
                                   where project.ProjectId == ProjectId
                                   select (DateTime)project.FirstInstallmentDueDate).Single();
                    }
                    else
                    {
                        DueDate = (from project in _context.Projects
                                   where project.ProjectId == ProjectId
                                   select (DateTime)project.FirstInstallmentDueDate).Single();
                        DueDate = DueDate.AddMonths(Increment - 1);
                    }
                    Installments.Add(DueDate.ToShortDateString());

                    var MonthlyInstallmentAmount = (from project in _context.Projects
                                                    where project.ProjectId == ProjectId
                                                    select (decimal?)project.MonthlyInstallmentAmount).Single();

                    Installments.Add(MonthlyInstallmentAmount.ToString());

                    decimal SumPaidAmountForInstallmentNo = (from paymentVoucher in _context.PaymentVouchers
                                                             where paymentVoucher.ProjectId == ProjectId &&
                                                             paymentVoucher.InstallmentsId == Increment &&
                                                             paymentVoucher.IsCanceled == false
                                                             select (decimal)paymentVoucher.PaymentAmount).Sum();

                    Installments.Add(SumPaidAmountForInstallmentNo.ToString());

                    var RemainAmount = MonthlyInstallmentAmount - SumPaidAmountForInstallmentNo;

                    Installments.Add(RemainAmount.ToString());

                    // Coloring for the table
                    string ColorClass = "";
                    // Firstly if the Installment due date is not due yet
                    int CompareDates = DateTime.Compare(DateTime.Now.Date, DueDate.Date);
                    if (CompareDates < 0)
                    {
                        ColorClass = "text-secondary";
                    }
                    else if (CompareDates == 0 || CompareDates > 1)
                    {
                        ColorClass = "text-danger";
                    }

                    if (RemainAmount > 0 && RemainAmount < MonthlyInstallmentAmount)
                    {
                        ColorClass = "text-warning";
                    }
                    else if (RemainAmount == 0)
                    {
                        ColorClass = "text-success";
                    }

                    result = this.Json(new
                    {
                        InstallmentName = InstallmentName,
                        dueDate = DueDate.ToShortDateString(),
                        monthlyInstallmentAmount = MonthlyInstallmentAmount.ToString(),
                        sumPaidAmountForInstallmentNo = SumPaidAmountForInstallmentNo.ToString(),
                        remainAmount = RemainAmount.ToString(),
                        colorClass = ColorClass
                    });
                    data.Add(result);
                }

                //get total count of data in table
                //totalRecord = payments.Count();
                //// get total count of records after search
                //filterRecord = payments.Count();

                // return Json(Installments);
                var returnObj = new
                {
                    //recordsTotal = totalRecord,
                    //recordsFiltered = filterRecord,
                    data = data
                };

                //return JsonConvert.SerializeObject(data.ToString());
                return Json(data.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        [HttpPost]
        public JsonResult GetMaxInstallmentNo(int projectId)
        {
            try
            {
                var InstallmentsNo = (from paymentVoucher in _context.PaymentVouchers
                                      where paymentVoucher.ProjectId == projectId && 
                                      paymentVoucher.IsCanceled == false
                                      select (int?)paymentVoucher.InstallmentsId).Max();

                // Check if it's the first installment or not, If it's the first just return 1 meaning it's the first installment.
                // If it's not null then check whether to get just the max or max + 1 depending on if the current installment has
                // been fully paid and that would be managed through getting the monthly installment amount from the Project or
                // get the Capital and divide it on the no of installments and check if the sum of PaidAmount in the PaymentVoucher
                // for this project equals the monthly amount then return max + 1, else return max meaning there's still a remain
                // unpaid amount in this installment
                if (InstallmentsNo.Equals(null))
                {
                    InstallmentsNo = 1;
                }
                else
                {
                    //decimal MonthlyInstallmentAmount = _context.Projects.Where(p => p.ProjectId == projectId).Single(p => p.MonthlyInstallmentAmount);

                    decimal MonthlyInstallmentAmount = (from project in _context.Projects
                                                        where project.ProjectId == projectId
                                                        select (decimal)project.MonthlyInstallmentAmount).Single();

                    decimal SumPaidAmountForInstallmentNo = (from paymentVoucher in _context.PaymentVouchers
                                                             where paymentVoucher.ProjectId == projectId &&
                                                             paymentVoucher.InstallmentsId == InstallmentsNo
                                                             select (decimal)paymentVoucher.PaymentAmount).Sum();

                    //var SumPaidAmountForInstallmentNo = _context.PaymentVouchers.Where(Payment => Payment.ProjectId == projectId).SumAsync(p => p.PaymentAmount);

                    if (MonthlyInstallmentAmount == SumPaidAmountForInstallmentNo)
                    {
                        InstallmentsNo++;
                    }
                }

                return Json(InstallmentsNo);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return Json(default);
            }
        }

        [Authorize]
        // GET: PaymentVoucher
        public async Task<IActionResult> Index()
        {
            try
            {
                var applicationDbContext = _context.PaymentVouchers.Include(p => p.Installments).Include(p => p.Projects).AsNoTracking();
                return View(await applicationDbContext.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // GET: PaymentVoucher/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var paymentVoucher = await _context.PaymentVouchers
                    .Include(p => p.Installments)
                    .Include(p => p.Projects)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.PaymentVoucherId == id);
                if (paymentVoucher == null)
                {
                    return NotFound();
                }

                return View(paymentVoucher);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // GET: PaymentVoucher/Create
        public IActionResult Create()
        {
            try
            {
                ViewData["InstallmentsId"] = new SelectList(_context.Installments.AsNoTracking(), "InstallmentsId", "NameAr");
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

        // POST: PaymentVoucher/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("PaymentVoucherId,PaymentDocumentPath,PaymentDate,PaymentAmount,IsApprovedByManagement,IsCanceled,ProjectId,InstallmentsId,UserId,ManagementUserId")] PaymentVoucher paymentVoucher)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(paymentVoucher);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["InstallmentsId"] = new SelectList(_context.Installments, "InstallmentsId", "NameAr", paymentVoucher.InstallmentsId);
        //    ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", paymentVoucher.ProjectId);
        //    ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", paymentVoucher.UserId);
        //    return View(paymentVoucher);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentVoucher paymentVoucher)
        {
            try
            {
                var userId = User.GetLoggedInUserId<string>();
                var filePath = HttpContext.Session.GetString("filePath");
                HttpContext.Session.Clear();
                paymentVoucher.PaymentDocumentPath = filePath;
                paymentVoucher.UserId = userId;

                int ProjectId = paymentVoucher.ProjectId;

                var IsFinancialStatementConfirmed = (from financialStatement in _context.FinancialStatements
                                                     where financialStatement.ProjectId == ProjectId
                                                     select financialStatement.FinancialStatementId).SingleOrDefaultAsync();

                // Meaning it should have a financial statement before this step
                if (!IsFinancialStatementConfirmed.Equals(null) && IsFinancialStatementConfirmed.Result != 0) 
                {
                    int NumberOfInstallments = (from project in _context.Projects
                                                where project.ProjectId == paymentVoucher.ProjectId
                                                select (int)project.NumberOfInstallments).Single();

                    decimal MonthlyInstallmentAmount = (from project in _context.Projects
                                                        where project.ProjectId == paymentVoucher.ProjectId
                                                        select (decimal)project.MonthlyInstallmentAmount).Single();

                    decimal PaymentAmount = paymentVoucher.PaymentAmount;

                    List<PaymentVoucher> paymentVouchers = new List<PaymentVoucher>();
                    //_context.Add(paymentVoucher);

                    // Make sure that the installment no that we're paying to is not greater than the number of installments
                    // that the project already saved with
                    if(paymentVoucher.InstallmentsId <= NumberOfInstallments)
                    {
                        for (int Increment = paymentVoucher.InstallmentsId; Increment <= NumberOfInstallments; Increment++)
                        {
                            if (PaymentAmount > 0)
                            {
                                decimal SumPaidAmountForInstallmentNo = (from paymentVoucherVar in _context.PaymentVouchers
                                                                         where paymentVoucherVar.ProjectId == paymentVoucher.ProjectId &&
                                                                         paymentVoucherVar.InstallmentsId == paymentVoucher.InstallmentsId &&
                                                                         paymentVoucherVar.IsCanceled == false
                                                                         select (decimal)paymentVoucherVar.PaymentAmount).Sum();

                                if (SumPaidAmountForInstallmentNo.Equals(0) || SumPaidAmountForInstallmentNo.Equals(null) || SumPaidAmountForInstallmentNo == 0)
                                {
                                    // First payment for this installment no
                                    if (PaymentAmount <= MonthlyInstallmentAmount)
                                    {
                                        paymentVoucher.PaymentAmount = PaymentAmount;
                                        paymentVouchers.Add(new PaymentVoucher { UserId = userId, ManagementUserId = null, PaymentDocumentPath = filePath, ProjectId = ProjectId, InstallmentsId = Increment, PaymentAmount = PaymentAmount, PaymentDate = paymentVoucher.PaymentDate });
                                        PaymentAmount = 0;
                                    }
                                    else if (PaymentAmount > MonthlyInstallmentAmount)
                                    {
                                        paymentVoucher.PaymentAmount = MonthlyInstallmentAmount;
                                        paymentVouchers.Add(new PaymentVoucher { UserId = userId, ManagementUserId = null, PaymentDocumentPath = filePath, ProjectId = ProjectId, InstallmentsId = Increment, PaymentAmount = MonthlyInstallmentAmount, PaymentDate = paymentVoucher.PaymentDate });
                                        //_context.Add(paymentVoucher);
                                        //await _context.SaveChangesAsync();
                                        PaymentAmount -= MonthlyInstallmentAmount;
                                        //Increment++;
                                        paymentVoucher.InstallmentsId += 1;
                                    }
                                }
                                else if (SumPaidAmountForInstallmentNo > 0 && SumPaidAmountForInstallmentNo < MonthlyInstallmentAmount)
                                {
                                    // Pay the remaining/part of this installment no and maybe proceed to the next installment no if there's
                                    // still remaining to pay from the PaymentAmount
                                    if ((PaymentAmount + SumPaidAmountForInstallmentNo) <= MonthlyInstallmentAmount)
                                    {
                                        paymentVoucher.PaymentAmount = PaymentAmount;
                                        paymentVouchers.Add(new PaymentVoucher { UserId = userId, ManagementUserId = null, PaymentDocumentPath = filePath, ProjectId = ProjectId, InstallmentsId = Increment, PaymentAmount = PaymentAmount, PaymentDate = paymentVoucher.PaymentDate });
                                        //_context.Add(paymentVoucher);
                                        //await _context.SaveChangesAsync();
                                        PaymentAmount = 0;
                                    }
                                    else if ((PaymentAmount + SumPaidAmountForInstallmentNo) > MonthlyInstallmentAmount)
                                    {
                                        paymentVoucher.PaymentAmount = (MonthlyInstallmentAmount - SumPaidAmountForInstallmentNo);
                                        paymentVouchers.Add(new PaymentVoucher { UserId = userId, ManagementUserId = null, PaymentDocumentPath = filePath, ProjectId = ProjectId, InstallmentsId = Increment, PaymentAmount = (MonthlyInstallmentAmount - SumPaidAmountForInstallmentNo), PaymentDate = paymentVoucher.PaymentDate });
                                        //_context.Add(paymentVoucher);
                                        //await _context.SaveChangesAsync();
                                        PaymentAmount -= (MonthlyInstallmentAmount - SumPaidAmountForInstallmentNo);
                                        //Increment++;
                                        paymentVoucher.InstallmentsId += 1;
                                    }
                                }
                                else if (SumPaidAmountForInstallmentNo >= MonthlyInstallmentAmount)
                                {
                                    // Current installment no is fully paid and thus go the next installments no
                                    //Increment++;
                                    paymentVoucher.InstallmentsId += 1;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        _context.Set<PaymentVoucher>().AddRange(paymentVouchers);
                        await _context.SaveChangesAsync();
                        paymentVouchers.ForEach(x => _logger.LogInformation("Payment voucher added, Payment voucher: {PaymentVoucherData}, User: {User}", new { PaymentVoucherId = x.PaymentVoucherId, ProjectId = x.ProjectId, InstallmentId = x.InstallmentsId, PaidAmount = x.PaymentAmount, PaymentDate = x.PaymentDate }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() }));
                        TempData["PaymentVoucher"] = "PaymentVoucher";
                        //return RedirectToAction(nameof(Index));
                        var RedirectURLThird = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                        return Json(new
                        {
                            redirectUrl = RedirectURLThird
                        });
                    }
                    else
                    {
                        TempData["InstallmentsNoGreaterThanProjectNumberOfInstallments"] = "PaymentVoucher";
                        var RedirectURLThird = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                        return Json(new
                        {
                            redirectUrl = RedirectURLThird
                        });
                    }
                }
                else
                {
                    TempData["NoFinancialStatement"] = "PaymentVoucher";
                    var RedirectURLFourth = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                    return Json(new
                    {
                        redirectUrl = RedirectURLFourth
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Payment voucher not added, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                TempData["PaymentVoucherError"] = "PaymentVoucher";
                var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURL
                });
            }
        }

        // GET: PaymentVoucher/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var paymentVoucher = await _context.PaymentVouchers.FindAsync(id);
                if (paymentVoucher == null)
                {
                    return NotFound();
                }
                ViewData["InstallmentsId"] = paymentVoucher.InstallmentsId;
                ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", paymentVoucher.ProjectId);
                ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", paymentVoucher.UserId);
                ViewData["ProjectNameAr"] = _context.Projects.Where(f => f.ProjectId == paymentVoucher.ProjectId).Select(f => f.NameAr).Single();
                return View(paymentVoucher);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // POST: PaymentVoucher/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int paymentVoucherId, [Bind("PaymentVoucherId,PaymentDocumentPath,PaymentDate,PaymentAmount,IsApprovedByManagement,IsCanceled,ProjectId,InstallmentsId,UserId,ManagementUserId")] PaymentVoucher paymentVoucher)
        {
            try
            {
                var NewFilePath = HttpContext.Session.GetString("filePath");
                HttpContext.Session.Clear();
                if (paymentVoucherId != paymentVoucher.PaymentVoucherId)
                {
                    TempData["PaymentVoucherError"] = "PaymentVoucher";
                    var RedirectURLThird = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                    return Json(new
                    {
                        redirectUrl = RedirectURLThird
                    });
                }
                
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Firstly check if this payment voucher is canceled then it can't be modeified again
                        var canceled = (from paymentVouchers in _context.PaymentVouchers
                                        where paymentVouchers.PaymentVoucherId == paymentVoucher.PaymentVoucherId
                                        select (bool)paymentVouchers.IsCanceled).Single();
                        if (!canceled)
                        {
                            // Then check if the paid amount is greater than the monthly installment amount then it's not allowed
                            decimal MonthlyInstallmentAmount = (from project in _context.Projects
                                                                where project.ProjectId == paymentVoucher.ProjectId
                                                                select (decimal)project.MonthlyInstallmentAmount).Single();
                            if(paymentVoucher.PaymentAmount > MonthlyInstallmentAmount)
                            {
                                TempData["PaymentAmountGreaterThanMonthlyAmount"] = "PaymentVoucher";
                                var RedirectURLSixth = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                                return Json(new
                                {
                                    redirectUrl = RedirectURLSixth
                                });
                            }
                            else
                            {
                                // If the session value exists then the user has added a file to update instead of the existing file
                                if (NewFilePath != null)
                                {
                                    var OldFilePath = "";
                                    OldFilePath = _context.PaymentVouchers.Where(f => f.PaymentVoucherId == paymentVoucher.PaymentVoucherId).Select(f => f.PaymentDocumentPath).Single();
                                    if (OldFilePath != null && !string.IsNullOrEmpty(OldFilePath))
                                    {
                                        FileInfo file = new FileInfo(OldFilePath);
                                        if (file.Exists)
                                        {
                                            file.Delete();
                                        }
                                    }
                                    paymentVoucher.PaymentDocumentPath = NewFilePath;
                                }

                                _context.Update(paymentVoucher);
                                await _context.SaveChangesAsync();
                                _logger.LogInformation("Payment voucher edited, Payment voucher: {PaymentVoucherData}, User: {User}", new { PaymentVoucherId = paymentVoucher.PaymentVoucherId, ProjectId = paymentVoucher.ProjectId, InstallmentId = paymentVoucher.InstallmentsId, PaidAmount = paymentVoucher.PaymentAmount, PaymentDate = paymentVoucher.PaymentDate }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                                TempData["PaymentVoucher"] = "PaymentVoucher";
                            }
                        }
                        else
                        {
                            TempData["PaymentVoucherCanceled"] = "PaymentVoucher";
                            var RedirectURLSixth = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                            return Json(new
                            {
                                redirectUrl = RedirectURLSixth
                            });
                        }
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PaymentVoucherExists(paymentVoucher.PaymentVoucherId))
                        {
                            _logger.LogError("DbUpdateConcurrencyException Exception, Payment voucher not edited, Payment voucher: {PaymentVoucherData}, User: {User}", new { PaymentVoucherId = paymentVoucher.PaymentVoucherId, ProjectId = paymentVoucher.ProjectId, InstallmentId = paymentVoucher.InstallmentsId, PaidAmount = paymentVoucher.PaymentAmount, PaymentDate = paymentVoucher.PaymentDate }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                            TempData["PaymentVoucherError"] = "PaymentVoucher";
                            var RedirectURLSixth = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                            return Json(new
                            {
                                redirectUrl = RedirectURLSixth
                            });
                        }
                        else
                        {
                            _logger.LogError("DbUpdateConcurrencyException Exception, Payment voucher not edited, Payment voucher: {PaymentVoucherData}, User: {User}", paymentVoucher, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                            TempData["PaymentVoucherError"] = "PaymentVoucher";
                            var RedirectURLSeventh = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                            return Json(new
                            {
                                redirectUrl = RedirectURLSeventh
                            });
                        }
                    }
                    //return RedirectToAction(nameof(Index));
                    var RedirectURLThird = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                    return Json(new
                    {
                        redirectUrl = RedirectURLThird
                    });
                }
                ViewData["InstallmentsId"] = new SelectList(_context.Installments, "InstallmentsId", "NameAr", paymentVoucher.InstallmentsId);
                ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", paymentVoucher.ProjectId);
                ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", paymentVoucher.UserId);
                TempData["PaymentVoucherError"] = "PaymentVoucher";
                var RedirectURLFirst = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURLFirst
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Payment voucher not edited, Message: {ErrorData}, User: {User}, Payment voucher: {PaymentVoucherData}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() }, paymentVoucher);
                TempData["PaymentVoucherError"] = "PaymentVoucher";
                var RedirectURLThird = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                return Json(new
                {
                    redirectUrl = RedirectURLThird
                });
            }
        }

        // GET: PaymentVoucher/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var paymentVoucher = await _context.PaymentVouchers
                    .Include(p => p.Installments)
                    .Include(p => p.Projects)
                    .FirstOrDefaultAsync(m => m.PaymentVoucherId == id);
                if (paymentVoucher == null)
                {
                    return NotFound();
                }

                return View(paymentVoucher);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error, Message: {ErrorData}, User: {User}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return View("Error");
            }
        }

        // POST: PaymentVoucher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                TempData["PaymentVoucher"] = "PaymentVoucher";
                var paymentVoucher = await _context.PaymentVouchers.FindAsync(id);
                paymentVoucher.IsCanceled = true;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Payment voucher canceled, Payment voucher: {PaymentVoucherData}, User: {User}", new { PaymentVoucherId = paymentVoucher.PaymentVoucherId, ProjectId = paymentVoucher.ProjectId, InstallmentId = paymentVoucher.InstallmentsId, PaidAmount = paymentVoucher.PaymentAmount, PaymentDate = paymentVoucher.PaymentDate }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() });
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var paymentVoucher = await _context.PaymentVouchers.FindAsync(id);
                _logger.LogError("Payment voucher not canceled, Message: {ErrorData}, User: {User}, Payment voucher: {PaymentVoucherData}", new { ex.Message, ex.StackTrace, ex.InnerException }, new { Id = User.GetLoggedInUserId<string>(), name = User.GetLoggedInUserName() }, new { PaymentVoucherId = paymentVoucher.PaymentVoucherId, ProjectId = paymentVoucher.ProjectId, InstallmentId = paymentVoucher.InstallmentsId, PaidAmount = paymentVoucher.PaymentAmount, PaymentDate = paymentVoucher.PaymentDate });
                TempData["PaymentVoucherError"] = "PaymentVoucher";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool PaymentVoucherExists(int id)
        {
            try
            {
                return _context.PaymentVouchers.Any(e => e.PaymentVoucherId == id);
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

        //[HttpGet]
        public async Task<IActionResult> Download(int PaymentVoucherId)
        {
            try
            {
                var fileSession = await (from paymentVoucher in this._context.PaymentVouchers
                                         where paymentVoucher.PaymentVoucherId.Equals(PaymentVoucherId)
                                         select (string)paymentVoucher.PaymentDocumentPath).SingleOrDefaultAsync();

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