using PaymentsAPI.Application.Abstractions.Respositories;
using PaymentsAPI.Domain.Entitities;
using PaymentsAPI.Infrastructure.Connections;

namespace PaymentsAPI.Infrastructure.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DataContext _context;

        public PaymentRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<PaymentEntity> Add(PaymentEntity paymentEntity)
        {
            try
            {
                var result = await _context.Payments.AddAsync(paymentEntity);
                await _context.SaveChangesAsync();

                return result.Entity;
            }
            catch
            {
                throw;
            }
        }
    }
}