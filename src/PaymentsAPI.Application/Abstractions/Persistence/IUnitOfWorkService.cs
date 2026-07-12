using PaymentsAPI.Application.Services;

namespace PaymentsAPI.Application.Abstractions.Persistence
{
    public interface IUnitOfWorkService
    {
        PaymentService PaymentService { get; }
    }
}