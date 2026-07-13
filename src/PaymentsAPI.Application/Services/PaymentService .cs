using PaymentsAPI.Application.Abstractions.Persistence;
using PaymentsAPI.Application.Abstractions.Services;
using PaymentsAPI.Application.Contracts.Dto;
using PaymentsAPI.Application.Contracts.Enums;
using PaymentsAPI.Application.Validators;
using PaymentsAPI.Domain.Common;
using PaymentsAPI.Domain.Entitities;
using PaymentsAPI.Shared.Logging;
using Serilog;

namespace PaymentsAPI.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public PaymentService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<Result<PaymentResponseDto>> ProcessPayment(
            CreatePaymentRequestDto request,
            string userId)
        {
            using var transaction = _repositoryUoW.BeginTransaction();

            try
            {
                var paymentEntity = new PaymentEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    GameId = request.GameId,
                    Amount = request.Amount,
                    PaymentMethod = request.PaymentMethod,
                    TransactionId = GenerateTransactionId(),
                    Status = PaymentStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                var validationResult =
                    await IsValidPaymentRequest(paymentEntity);

                if (!validationResult.Success)
                {
                    await transaction.RollbackAsync();

                    Log.Warning(LogMessages.InvalidPaymentInputs());

                    return Result<PaymentResponseDto>.Error(
                        validationResult.Message);
                }

                paymentEntity.Status = ProcessPaymentSimulation(
                    paymentEntity);

                paymentEntity.ProcessedAt = DateTime.UtcNow;

                var savedPayment =
                    await _repositoryUoW.PaymentRepository.Add(paymentEntity);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                Log.Information(
                    LogMessages.AddingPaymentSuccess(savedPayment));

                var response = new PaymentResponseDto
                {
                    Id = savedPayment.Id,
                    UserId = savedPayment.UserId,
                    GameId = savedPayment.GameId,
                    Amount = savedPayment.Amount,
                    PaymentMethod = savedPayment.PaymentMethod,
                    TransactionId = savedPayment.TransactionId,
                    Status = savedPayment.Status,
                    CreatedAt = savedPayment.CreatedAt,
                    ProcessedAt = savedPayment.ProcessedAt
                };

                return Result<PaymentResponseDto>.Ok(response);
            }
            //catch (Exception ex)
            //{
            //    await transaction.RollbackAsync();

            //    Log.Error(LogMessages.AddingPaymentError(ex));

            //    return Result<PaymentResponseDto>.Error(
            //        $"Error processing payment: {ex.Message}");
            //}
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                var errorMessage =
                    ex.InnerException?.InnerException?.Message
                    ?? ex.InnerException?.Message
                    ?? ex.Message;

                Log.Error(ex, "Erro ao processar pagamento");

                return Result<PaymentResponseDto>.Error(
                    $"Error processing payment: {errorMessage}");
            }
        }

        private static PaymentStatus ProcessPaymentSimulation(
            PaymentEntity paymentEntity)
        {
            if (paymentEntity.Amount <= 0)
                return PaymentStatus.Rejected;

            return PaymentStatus.Approved;
        }

        private static string GenerateTransactionId()
        {
            return $"PAY-{Guid.NewGuid():N}".ToUpperInvariant();
        }

        private static async Task<Result<PaymentEntity>>
            IsValidPaymentRequest(PaymentEntity paymentEntity)
        {
            var validator = new PaymentRequestValidator();

            var validationResult =
                await validator.ValidateAsync(paymentEntity);

            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(
                    " ",
                    validationResult.Errors.Select(
                        error => error.ErrorMessage));

                errorMessage = errorMessage.Replace(
                    Environment.NewLine,
                    string.Empty);

                return Result<PaymentEntity>.Error(errorMessage);
            }

            return Result<PaymentEntity>.Ok(paymentEntity);
        }
    }
}