using FCG.Contracts.Events;
using MassTransit;
using PaymentsAPI.Application.Abstractions.Services;
using PaymentsAPI.Application.Contracts.Dto;
using PaymentsAPI.Messaging;

namespace PaymentsAPI.Consumers
{
    public class OrderPlacedEventConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly IPaymentService _paymentService;
        private readonly PaymentProcessedEventPublisher _publisher;

        public OrderPlacedEventConsumer(
            IPaymentService paymentService,
            PaymentProcessedEventPublisher publisher)
        {
            _paymentService = paymentService;
            _publisher = publisher;
        }

        public async Task Consume(
            ConsumeContext<OrderPlacedEvent> context)
        {
            var orderPlacedEvent = context.Message;

            var request = new CreatePaymentRequestDto
            {
                GameId = orderPlacedEvent.GameId,
                Amount = orderPlacedEvent.Price,
                PaymentMethod = "Simulated"
            };

            var result = await _paymentService.ProcessPayment(
                request,
                orderPlacedEvent.UserId);

            if (!result.Success || result.Data is null)
                return;

            await _publisher.Publish(
                result.Data,
                orderPlacedEvent.Email);
        }
    }
}