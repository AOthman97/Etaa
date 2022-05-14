using Etaa.Models;

namespace Etaa.Data.Services
{
    public interface IPaymentVouchersService
    {
        public Task<IEnumerable<PaymentVoucher>> GetAllAsync();
        Task<PaymentVoucher> GetPaymentVoucherAsync(int PaymentVoucherId);
        Task AddPaymentVoucherAsync(PaymentVoucher paymentVoucher);
        Task<PaymentVoucher> UpdatePaymentVoucherAsync(int PaymentVoucherId, PaymentVoucher paymentVoucher);
        bool DeletePaymentVoucher(int PaymentVoucherId);
    }
}
