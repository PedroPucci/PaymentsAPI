using PaymentsAPI.Domain.Entitities;

namespace PaymentsAPI.Application.Abstractions.Respositories
{
    public interface IPaymentRepository
    {
        Task<PaymentEntity> Add(PaymentEntity paymentEntity);
    }
}