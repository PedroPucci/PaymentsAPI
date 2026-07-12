using PaymentsAPI.Domain.Entitities;

namespace PaymentsAPI.Shared.Logging
{
    public static class LogMessages
    {
        #region Payment Validation

        public static string InvalidPaymentInputs() => "Invalid payment data.";

        #endregion

        #region Payment CRUD

        public static string AddingPaymentError(Exception ex) =>
            $"Error processing payment. Details: {ex.Message}";

        public static string AddingPaymentSuccess(PaymentEntity paymentEntity) =>
            $"Payment id:{paymentEntity.Id} processed successfully.";

        public static string GetByPaymentIdError(Exception ex) =>
            $"Error retrieving Payment by id. Details: {ex.Message}";

        public static string GetByPaymentIdSuccess(PaymentEntity paymentEntity) =>
            $"Payment id:{paymentEntity.Id} retrieved successfully.";

        #endregion
    }
}