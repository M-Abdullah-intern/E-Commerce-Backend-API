using ECommerceAPI.DTOs.ShippingAddress;

namespace ECommerceAPI.Services.Interfaces
{
    public interface IAddressService
    {
        Task<List<ShippingAddressDto>> GetUserAddresses(int userId);
        Task<ShippingAddressDto> CreateAddress(int userId, CreateShippingAddressDto dto);
        Task UpdateAddress(int userId, int addressId, UpdateShippingAddressDto dto);
        Task DeleteAddress(int userId, int addressId);
    }
}
