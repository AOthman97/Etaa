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
            var Result = new SelectList(await _context.Installments.ToListAsync(), "InstallmentsId", "NameAr");
            return Json(Result);
        }

        // This action and the below are for the autocomplete functionality to firstly select the Project and get the ProjectID
        [HttpPost]  
        public JsonResult AutoComplete(string prefix)
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

        [HttpPost]
        public IActionResult GetAllInstallments(int ProjectId)
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

            for(int Increment = 1; Increment <= ProjectNumberOfInstallments; Increment++)
            {
                var InstallmentName = (from Installment in _context.Installments
                                       where Installment.InstallmentNumber == Increment
                                       select (string)Installment.NameAr).Single();

                Installments.Add(InstallmentName);

                DateTime DueDate = DateTime.Now;

                if(Increment == 1)
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
                if(CompareDates < 0)
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

            //return Json(returnObj);
            //return Ok(returnObj);
        }

        [HttpPost]
        public JsonResult GetMaxInstallmentNo(int projectId)
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

        // GET: PaymentVoucher
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PaymentVouchers.Include(p => p.Installments).Include(p => p.Projects).Include(p => p.Users);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PaymentVoucher/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentVoucher = await _context.PaymentVouchers
                .Include(p => p.Installments)
                .Include(p => p.Projects)
                .Include(p => p.Users)
                .FirstOrDefaultAsync(m => m.PaymentVoucherId == id);
            if (paymentVoucher == null)
            {
                return NotFound();
            }

            return View(paymentVoucher);
        }

        // GET: PaymentVoucher/Create
        public IActionResult Create()
        {
            ViewData["InstallmentsId"] = new SelectList(_context.Installments, "InstallmentsId", "NameAr");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr");
            return View();
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
                var filePath = HttpContext.Session.GetString("filePath");
                HttpContext.Session.Clear();
                paymentVoucher.PaymentDocumentPath = filePath;
                paymentVoucher.UserId = 1;
                paymentVoucher.ManagementUserId = 1;

                int ProjectId = paymentVoucher.ProjectId;

                int NumberOfInstallments = (from project in _context.Projects
                                            where project.ProjectId == paymentVoucher.ProjectId
                                            select (int)project.NumberOfInstallments).Single();

                decimal MonthlyInstallmentAmount = (from project in _context.Projects
                                                    where project.ProjectId == paymentVoucher.ProjectId
                                                    select (decimal)project.MonthlyInstallmentAmount).Single();

                decimal PaymentAmount = paymentVoucher.PaymentAmount;

                List<PaymentVoucher> paymentVouchers = new List<PaymentVoucher>();
                //_context.Add(paymentVoucher);

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
                                paymentVouchers.Add(new PaymentVoucher { UserId = 1, ManagementUserId = 1, PaymentDocumentPath = filePath, ProjectId = ProjectId, InstallmentsId = Increment, PaymentAmount = PaymentAmount, PaymentDate = paymentVoucher.PaymentDate });
                                PaymentAmount = 0;
                            }
                            else if (PaymentAmount > MonthlyInstallmentAmount)
                            {
                                paymentVoucher.PaymentAmount = MonthlyInstallmentAmount;
                                paymentVouchers.Add(new PaymentVoucher { UserId = 1, ManagementUserId = 1, PaymentDocumentPath = filePath, ProjectId = ProjectId, InstallmentsId = Increment, PaymentAmount = MonthlyInstallmentAmount, PaymentDate = paymentVoucher.PaymentDate });
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
                                paymentVouchers.Add(new PaymentVoucher { UserId = 1, ManagementUserId = 1, PaymentDocumentPath = filePath, ProjectId = ProjectId, InstallmentsId = Increment, PaymentAmount = PaymentAmount, PaymentDate = paymentVoucher.PaymentDate });
                                //_context.Add(paymentVoucher);
                                //await _context.SaveChangesAsync();
                                PaymentAmount = 0;
                            }
                            else if ((PaymentAmount + SumPaidAmountForInstallmentNo) > MonthlyInstallmentAmount)
                            {
                                paymentVoucher.PaymentAmount = (MonthlyInstallmentAmount - SumPaidAmountForInstallmentNo);
                                paymentVouchers.Add(new PaymentVoucher { UserId = 1, ManagementUserId = 1, PaymentDocumentPath = filePath, ProjectId = ProjectId, InstallmentsId = Increment, PaymentAmount = (MonthlyInstallmentAmount - SumPaidAmountForInstallmentNo), PaymentDate = paymentVoucher.PaymentDate });
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
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: PaymentVoucher/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["InstallmentsId"] = new SelectList(_context.Installments, "InstallmentsId", "NameAr", paymentVoucher.InstallmentsId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", paymentVoucher.ProjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "NameAr", paymentVoucher.UserId);
            return View(paymentVoucher);
        }

        // POST: PaymentVoucher/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentVoucherId,PaymentDocumentPath,PaymentDate,PaymentAmount,IsApprovedByManagement,IsCanceled,ProjectId,InstallmentsId,UserId,ManagementUserId")] PaymentVoucher paymentVoucher)
        {
            if (id != paymentVoucher.PaymentVoucherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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

        // GET: PaymentVoucher/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentVoucher = await _context.PaymentVouchers
                .Include(p => p.Installments)
                .Include(p => p.Projects)
                .Include(p => p.Users)
                .FirstOrDefaultAsync(m => m.PaymentVoucherId == id);
            if (paymentVoucher == null)
            {
                return NotFound();
            }

            return View(paymentVoucher);
        }

        // POST: PaymentVoucher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paymentVoucher = await _context.PaymentVouchers.FindAsync(id);
            _context.PaymentVouchers.Remove(paymentVoucher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentVoucherExists(int id)
        {
            return _context.PaymentVouchers.Any(e => e.PaymentVoucherId == id);
        }

        public async Task<IActionResult> Upload(IFormFile file)
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
    }
}
