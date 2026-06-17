using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        Task<List<ShippingAddress>> GetUserAddresses(int userId);
        Task<ShippingAddress?> GetById(int id);
        Task Add(ShippingAddress address);
        Task Update(ShippingAddress address);
        Task Delete(ShippingAddress address);
        Task SaveChangesAsync();
    }
}
