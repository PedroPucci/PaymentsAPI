using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PaymentsAPI.Extensions.SwaggerDocumentation
{
    public class CustomOperationDescriptions : IOperationFilter
    {
        public void Apply(OpenApiOperation operation,OperationFilterContext context)
        {
            if (context.ApiDescription.HttpMethod is null ||
                context.ApiDescription.RelativePath is null)
            {
                return;
            }

            var path = context.ApiDescription.RelativePath.ToLowerInvariant();

            var routeHandlers = new Dictionary<string, Action>(
                StringComparer.OrdinalIgnoreCase)
            {
                {
                    "payments",
                    () => HandlePaymentOperations(operation, context)
                }
            };

            foreach (var routeHandler in routeHandlers.OrderByDescending(item => item.Key.Length))
            {
                if (path.Contains(routeHandler.Key))
                {
                    routeHandler.Value.Invoke();
                    return;
                }
            }
        }

        private static void HandlePaymentOperations(
            OpenApiOperation operation,
            OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod?
                .ToUpperInvariant();

            if (method != "POST")
                return;

            operation.Summary = "Process a game purchase payment.";

            operation.Description =
                "Processes or simulates the payment for a game purchase. " +
                "In the event-driven flow, the PaymentsAPI consumes an " +
                "OrderPlacedEvent containing UserId, GameId and Price, " +
                "processes the payment, and publishes a " +
                "PaymentProcessedEvent with Approved or Rejected status.";

            AddResponse(
                operation,
                StatusCodes.Status200OK.ToString(),
                "Payment processed successfully.");

            AddResponse(
                operation,
                StatusCodes.Status400BadRequest.ToString(),
                "The payment request is invalid or the payment was rejected.");

            AddResponse(
                operation,
                StatusCodes.Status401Unauthorized.ToString(),
                "The user is not authenticated.");
        }

        private static void AddResponse(
            OpenApiOperation operation,
            string statusCode,
            string description)
        {
            if (operation.Responses.ContainsKey(statusCode))
            {
                operation.Responses[statusCode].Description = description;
                return;
            }

            operation.Responses.Add(
                statusCode,
                new OpenApiResponse
                {
                    Description = description
                });
        }
    }
}