using PaymentsAPI.Application.Contracts.Dto;
using PaymentsAPI.Domain.Common;

namespace PaymentsAPI.Application.Abstractions.Services
{
    public interface IPaymentService
    {
        Task<Result<PaymentResponseDto>> ProcessPayment(CreatePaymentRequestDto request, string userId);
    }
}