namespace PaymentsAPI.Application.Contracts.Dto
{
    public class CreatePaymentRequestDto
    {
        public int GameId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}