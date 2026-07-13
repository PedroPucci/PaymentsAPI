using Microsoft.EntityFrameworkCore;
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
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the payment.", ex);
            }
        }

        //public async Task<PaymentEntity> Add(PaymentEntity paymentEntity)
        //{
        //    try
        //    {
        //        var result = await _context.Payments.AddAsync(paymentEntity);

        //        return result.Entity;
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        var databaseMessage =
        //            ex.InnerException?.Message ?? ex.Message;

        //        throw new InvalidOperationException(
        //            $"Erro ao salvar pagamento no banco: {databaseMessage}",
        //            ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new InvalidOperationException(
        //            $"Erro inesperado ao adicionar pagamento: {ex.Message}",
        //            ex);
        //    }
        //}
    }
}