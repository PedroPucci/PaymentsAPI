using System.ComponentModel;

namespace PaymentsAPI.Application.Contracts.DomainErrors
{
    public enum PaymentErrors
    {
        [Description("'UserId' can not be null or empty!")]
        Payment_Error_UserIdCanNotBeNullOrEmpty,

        [Description("'GameId' must be greater than zero!")]
        Payment_Error_GameIdMustBeGreaterThanZero,

        [Description("'Amount' must be greater than zero!")]
        Payment_Error_AmountMustBeGreaterThanZero,

        [Description("'PaymentMethod' can not be null or empty!")]
        Payment_Error_PaymentMethodCanNotBeNullOrEmpty,

        [Description("'PaymentMethod' must have a maximum of 50 characters!")]
        Payment_Error_PaymentMethodMaximumLength
    }
}