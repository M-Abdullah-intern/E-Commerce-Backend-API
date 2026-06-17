using ECommerceAPI.Data;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories.Implementations
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext _context;

        public AddressRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ShippingAddress>> GetUserAddresses(int userId)
        {
            return await _context.ShippingAddresses
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<ShippingAddress?> GetById(int id)
        {
            return await _context.ShippingAddresses.FindAsync(id);
        }

        public async Task Add(ShippingAddress address)
        {
            await _context.ShippingAddresses.AddAsync(address);
        }

        public async Task Update(ShippingAddress address)
        {
            _context.ShippingAddresses.Update(address);
        }

        public async Task Delete(ShippingAddress address)
        {
            _context.ShippingAddresses.Remove(address);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
