using Etaa.Models;
using Microsoft.EntityFrameworkCore;

namespace Etaa.Data.Services
{
    public class PaymentVouchersService : IPaymentVouchersService
    {
        private readonly ApplicationDbContext _dbContext;

        public PaymentVouchersService(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task AddPaymentVoucherAsync(PaymentVoucher paymentVoucher)
        {
            var Result = await _dbContext.PaymentVouchers.AddAsync(paymentVoucher);
            await _dbContext.SaveChangesAsync();
        }

        public bool DeletePaymentVoucher(int PaymentVoucherId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PaymentVoucher>> GetAllAsync()
        {
            var Result = await _dbContext.PaymentVouchers.OrderBy(PaymentVoucher => PaymentVoucher.PaymentDate).ToListAsync();
            return Result;
        }

        public async Task<PaymentVoucher> GetPaymentVoucherAsync(int PaymentVoucherId)
        {
            var Result = await _dbContext.PaymentVouchers.FirstOrDefaultAsync(PaymentVoucher => PaymentVoucher.PaymentVoucherId == PaymentVoucherId);
            return Result;
        }

        public async Task<PaymentVoucher> UpdatePaymentVoucherAsync(int PaymentVoucherId, PaymentVoucher paymentVoucher)
        {
            _dbContext.Update(paymentVoucher);
            await _dbContext.SaveChangesAsync();
            return paymentVoucher;
        }
    }
}
