using Microsoft.EntityFrameworkCore.Storage;
using PaymentsAPI.Application.Abstractions.Respositories;

namespace PaymentsAPI.Application.Abstractions.Persistence
{
    public interface IRepositoryUoW
    {
        IPaymentRepository PaymentRepository { get; }

        Task SaveAsync();
        void Commit();
        IDbContextTransaction BeginTransaction();
    }
}