using FCG.Contracts.Events;
using MassTransit;
using PaymentsAPI.Application.Abstractions.Persistence;
using PaymentsAPI.Application.Contracts.Dto;
using PaymentsAPI.Messaging;
using Serilog;

namespace PaymentsAPI.Consumers
{
    public class OrderPlacedEventConsumer
        : IConsumer<OrderPlacedEvent>
    {
        private readonly IUnitOfWorkService _unitOfWorkService;
        private readonly PaymentProcessedEventPublisher _publisher;

        public OrderPlacedEventConsumer(
            IUnitOfWorkService unitOfWorkService,
            PaymentProcessedEventPublisher publisher)
        {
            _unitOfWorkService = unitOfWorkService;
            _publisher = publisher;
        }

        public async Task Consume(
            ConsumeContext<OrderPlacedEvent> context)
        {
            var orderPlacedEvent = context.Message;

            Log.Information(
                "OrderPlacedEvent received. UserId: {UserId}, GameId: {GameId}, Price: {Price}",
                orderPlacedEvent.UserId,
                orderPlacedEvent.GameId,
                orderPlacedEvent.Price);

            Console.WriteLine(
                $"OrderPlacedEvent received - UserId: {orderPlacedEvent.UserId}, " +
                $"GameId: {orderPlacedEvent.GameId}, Price: {orderPlacedEvent.Price}");

            var request = new CreatePaymentRequestDto
            {
                GameId = orderPlacedEvent.GameId,
                Amount = orderPlacedEvent.Price,
                PaymentMethod = "Simulated"
            };

            var result =
                await _unitOfWorkService.PaymentService.ProcessPayment(
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