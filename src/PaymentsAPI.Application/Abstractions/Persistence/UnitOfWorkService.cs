using PaymentsAPI.Application.Services;

namespace PaymentsAPI.Application.Abstractions.Persistence
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private readonly IRepositoryUoW _repositoryUoW;
        private PaymentService paymentService;

        public UnitOfWorkService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public PaymentService PaymentService
        {
            get
            {
                if (paymentService is null)
                    paymentService = new PaymentService(_repositoryUoW);
                return paymentService;
            }
        }
    }
}