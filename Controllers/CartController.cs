using ECommerceAPI.DTOs.CartDTOs;
using ECommerceAPI.Helpers;
using ECommerceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("User ID not found in token");

            var userId = int.Parse(userIdClaim.Value);

            var cart = await _cartService.GetCart(userId);

            return Ok(cart);
        }

        [HttpPost("additem")]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody]AddToCartDto dto)
        {
            var userId = UserClaimsHelper.GetUserId(User);
            var queryQty = Request.Query["quantity"];
            await _cartService.AddToCart(userId, dto.ProductId, dto.Quantity);

            return Ok(ApiResponseHelper.Success("Item added to cart"));
        }

        [HttpPut("update/{cartItemId}")]
        [Authorize]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, UpdateCartQuantityDto dto)
        {
            var userId = UserClaimsHelper.GetUserId(User);

            await _cartService.UpdateQuantity(cartItemId, dto.Quantity, userId);

            return Ok(ApiResponseHelper.Success("Quantity updated"));
        }

        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var userId = UserClaimsHelper.GetUserId(User);

            await _cartService.RemoveItem(cartItemId, userId);

            return Ok(ApiResponseHelper.Success("Item removed"));
        }
    }
}