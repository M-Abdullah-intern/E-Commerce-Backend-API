using ECommerceAPI.Helpers;
using ECommerceAPI.Repositories.Interfaces;
using ECommerceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/invoice")]
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IOrderRepository _orderRepository;

        public InvoiceController(IInvoiceService invoiceService, IOrderRepository orderRepository)
        {
            _invoiceService = invoiceService;
            _orderRepository = orderRepository;
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetInvoice(int orderId)
        {
            var userId = UserClaimsHelper.GetUserId(User);
            var role = UserClaimsHelper.GetRole(User);

            var order = await _orderRepository.GetOrderById(orderId);

            if (order == null)
                return NotFound(new { message = "Order not found" });

            if (order.UserId != userId && role != "Admin")
                return Forbid();

            var invoice = await _invoiceService.GenerateInvoiceAsync(orderId);
            return Ok(invoice);
        }
    }
}