using Microsoft.EntityFrameworkCore.Storage;
using PaymentsAPI.Application.Abstractions.Persistence;
using PaymentsAPI.Application.Abstractions.Respositories;
using PaymentsAPI.Infrastructure.Connections;
using Serilog;

namespace PaymentsAPI.Infrastructure.Repository
{
    public class RepositoryUoW : IRepositoryUoW
    {
        private readonly DataContext _context;
        private bool _disposed = false;
        private IPaymentRepository? _paymentRepository = null;

        public RepositoryUoW(DataContext context)
        {
            _context = context;
        }

        public IPaymentRepository PaymentRepository
        {
            get
            {
                if (_paymentRepository is null)
                {
                    _paymentRepository = new PaymentRepository(_context);
                }
                return _paymentRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error($"Database connection failed: {ex.Message}");
                throw new ApplicationException("Database is not available. Please check the connection.");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}