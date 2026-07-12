using FCG.Contracts.Events;
using MassTransit;
using PaymentsAPI.Application.Contracts.Dto;

namespace PaymentsAPI.Messaging
{
    public class PaymentProcessedEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public PaymentProcessedEventPublisher(
            IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Publish(
            PaymentResponseDto payment,
            string email)
        {
            var paymentProcessedEvent = new PaymentProcessedEvent(
                payment.Id,
                payment.UserId,
                email,
                payment.GameId,
                payment.Amount,
                payment.Status.ToString(),
                payment.TransactionId,
                payment.ProcessedAt ?? DateTime.UtcNow);

            await _publishEndpoint.Publish(paymentProcessedEvent);
        }
    }
}