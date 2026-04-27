using ECommerceAPI.DTOs.OrderDTOs;
using ECommerceAPI.Helpers;
using ECommerceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Pay([FromBody] PaymentRequestDto dto)
        {
            var userId = UserClaimsHelper.GetUserId(User);

            var result = await _paymentService.ProcessPayment(userId, dto);

            return Ok(ApiResponseHelper.Success(result));
        }
    }
}