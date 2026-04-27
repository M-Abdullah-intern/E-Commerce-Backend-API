using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Services.Interfaces;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetInvoice(int orderId)
        {
            try
            {
                var invoice = await _invoiceService.GenerateInvoiceAsync(orderId);
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}