using PaymentsAPI.Application.Contracts.Enums;

namespace PaymentsAPI.Domain.Entitities
{
    public class PaymentEntity
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int GameId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public PaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
    }
}