using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentsAPI.Application.Abstractions.Persistence;
using PaymentsAPI.Application.Contracts.Dto;
using System.Net.Mime;
using System.Security.Claims;

namespace PaymentsAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/payments")]
    [Produces(MediaTypeNames.Application.Json)]
    public class PaymentController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public PaymentController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(PaymentResponseDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ProcessPayment([FromBody] CreatePaymentRequestDto request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("Usuário não autenticado.");

            var result = await _uow.PaymentService.ProcessPayment(request, userId);

            if (!result.Success || result.Data is null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}