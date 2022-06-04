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
using Newtonsoft.Json;
using Etaa.Extensions;

namespace Etaa.Controllers
{
    public class PaymentVoucherController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment hostingEnv;

        public PaymentVoucherController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            hostingEnv = webHostEnvironment;
        }

        // Get Installments select list
        public async Task<JsonResult> GetInstallments()
        {
            try
            {
                var Result = new SelectList(await _context.Installments.ToListAsync(), "InstallmentsId", "NameAr");
                return Json(Result);
            }
            catch (Exception ex)
            {
                return Json(default);
            }
        }

        // Get the Installment no select item for current payment voucher
        public async Task<JsonResult> GetInstallmentForPaymentVoucher(int PaymentVoucherId)
        {
            try
            {
                // await _context.PaymentVouchers.Where(f => f.PaymentVoucherId == PaymentVoucherId).Select(f => f.InstallmentsId).ToListAsync()
                var InstallmentId = _context.PaymentVouchers.Where(p => p.PaymentVoucherId == PaymentVoucherId).Select(p => p.InstallmentsId).Single();
                var Result = new SelectList(await _context.Installments.Where(i => i.InstallmentsId == InstallmentId).ToListAsync(), "InstallmentsId", "NameAr");
                return Json(Result);
            }
            catch (Exception ex)
            {
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
                               where project.NameEn.StartsWith(prefix)
                               select new
                               {
                                   label = project.NameEn,
                                   val = project.ProjectId
                               }).ToListAsync();
                return Json(Project);
            }
            catch (Exception ex)
            {
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
                                                   select (int)project.NumberOfInstallments).Single();
                List<string> Installments = new List<string>();

                for (int Increment = 1; Increment <= ProjectNumberOfInstallments; Increment++)
                {
                    var InstallmentName = (from Installment in _context.Installments
                                           where Installment.InstallmentNumber == Increment
                                           select (string)Installment.NameAr).Single();

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
                                                    select (decimal)project.MonthlyInstallmentAmount).Single();

                    Installments.Add(MonthlyInstallmentAmount.ToString());

                    decimal SumPaidAmountForInstallmentNo = (from paymentVoucher in _context.PaymentVouchers
                                                             where paymentVoucher.ProjectId == ProjectId &&
                                                             paymentVoucher.InstallmentsId == Increment
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
                return View("Error");
            }
        }

        [HttpPost]
        public JsonResult GetMaxInstallmentNo(int projectId)
        {
            try
            {
                var InstallmentsNo = (from paymentVoucher in _context.PaymentVouchers
                                      where paymentVoucher.ProjectId == projectId
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
                return Json(default);
            }
        }

        // GET: PaymentVoucher
        public async Task<IActionResult> Index()
        {
            try
            {
                var applicationDbContext = _context.PaymentVouchers.Include(p => p.Installments).Include(p => p.Projects);
                return View(await applicationDbContext.ToListAsync());
            }
            catch (Exception ex)
            {
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
                    .FirstOrDefaultAsync(m => m.PaymentVoucherId == id);
                if (paymentVoucher == null)
                {
                    return NotFound();
                }

                return View(paymentVoucher);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: PaymentVoucher/Create
        public IActionResult Create()
        {
            try
            {
                ViewData["InstallmentsId"] = new SelectList(_context.Installments, "InstallmentsId", "NameAr");
                ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId");
                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr");
                return View();
            }
            catch (Exception ex)
            {
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
        //    ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr", paymentVoucher.UserId);
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
                                                                         paymentVoucherVar.InstallmentsId == paymentVoucher.InstallmentsId
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

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                return View("Error");
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
                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr", paymentVoucher.UserId);
                ViewData["ProjectNameAr"] = _context.Projects.Where(f => f.ProjectId == paymentVoucher.ProjectId).Select(f => f.NameAr).Single();
                return View(paymentVoucher);
            }
            catch (Exception ex)
            {
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
                if (paymentVoucherId != paymentVoucher.PaymentVoucherId)
                {
                    return NotFound();
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
                            OldFilePath = _context.PaymentVouchers.Where(f => f.PaymentVoucherId == paymentVoucher.PaymentVoucherId).Select(f => f.PaymentDocumentPath).Single();
                            FileInfo file = new FileInfo(OldFilePath);
                            if (file.Exists)
                            {
                                file.Delete();
                            }
                            paymentVoucher.PaymentDocumentPath = NewFilePath;
                        }

                        _context.Update(paymentVoucher);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PaymentVoucherExists(paymentVoucher.PaymentVoucherId))
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
                ViewData["InstallmentsId"] = new SelectList(_context.Installments, "InstallmentsId", "NameAr", paymentVoucher.InstallmentsId);
                ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", paymentVoucher.ProjectId);
                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr", paymentVoucher.UserId);
                return View(paymentVoucher);
            }
            catch (Exception ex)
            {
                return View("Error");
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
                var paymentVoucher = await _context.PaymentVouchers.FindAsync(id);
                paymentVoucher.IsCanceled = true;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error");
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
                    file.CopyTo(fs);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
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
                return View("Error");
            }
        }
    }
}
