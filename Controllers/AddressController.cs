using ECommerceAPI.DTOs.ShippingAddress;
using ECommerceAPI.Helpers;
using ECommerceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/addresses")]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddresses()
        {
            var userId = UserClaimsHelper.GetUserId(User);
            var addresses = await _addressService.GetUserAddresses(userId);
            return Ok(addresses);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress(CreateShippingAddressDto dto)
        {
            var userId = UserClaimsHelper.GetUserId(User);
            var address = await _addressService.CreateAddress(userId, dto);
            return CreatedAtAction(nameof(GetAddresses), new { }, address);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int id, UpdateShippingAddressDto dto)
        {
            var userId = UserClaimsHelper.GetUserId(User);
            await _addressService.UpdateAddress(userId, id, dto);
            return Ok(ApiResponseHelper.Success("Address updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var userId = UserClaimsHelper.GetUserId(User);
            await _addressService.DeleteAddress(userId, id);
            return Ok(ApiResponseHelper.Success("Address deleted successfully"));
        }
    }
}
