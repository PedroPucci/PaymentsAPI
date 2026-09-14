using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using FCG.Contracts.Events;
using Microsoft.Extensions.Configuration;
using PaymentsAPI.Application.Contracts.Dto;

namespace PaymentsAPI.Messaging
{
    public class PaymentProcessedEventPublisher
    {
        private readonly IAmazonSQS _sqs;
        private readonly IConfiguration _configuration;

        public PaymentProcessedEventPublisher(
            IAmazonSQS sqs,
            IConfiguration configuration)
        {
            _sqs = sqs;
            _configuration = configuration;
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

            var message = new
            {
                eventType = "PaymentProcessed",
                data = paymentProcessedEvent
            };

            var queueUrl =
                _configuration["AWS:SQS:NotificationsQueueUrl"];

            if (string.IsNullOrWhiteSpace(queueUrl))
            {
                throw new InvalidOperationException(
                    "AWS:SQS:NotificationsQueueUrl não configurada.");
            }

            await _sqs.SendMessageAsync(
                new SendMessageRequest
                {
                    QueueUrl = queueUrl,
                    MessageBody = JsonSerializer.Serialize(message)
                });
        }
    }
}