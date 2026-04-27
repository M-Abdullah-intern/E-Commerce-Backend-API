using ECommerceAPI.DTOs.OrderDTOs;
using ECommerceAPI.Helpers;
using ECommerceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder()
        {
            var userId = UserClaimsHelper.GetUserId(User);

            await _orderService.PlaceOrder(userId);

            return Ok(ApiResponseHelper.Success("Order placed successfully"));
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userId = UserClaimsHelper.GetUserId(User);

            var orders = await _orderService.GetUserOrders(userId);

            return Ok(orders);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            return Ok(orders);
        }

        [HttpPut("{orderId}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(
            int orderId,
            UpdateOrderStatusDto dto)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, dto.Status);

            if (!result)
                return NotFound(ApiResponseHelper.Fail("Order not found"));

            return Ok(ApiResponseHelper.Success("Order status updated successfully"));
        }
    }
}