using AutoMapper;
using ECommerceAPI.DTOs.ShippingAddress;
using ECommerceAPI.Exceptions;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using ECommerceAPI.Services.Interfaces;

namespace ECommerceAPI.Services.Implementations
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repository;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ShippingAddressDto>> GetUserAddresses(int userId)
        {
            var addresses = await _repository.GetUserAddresses(userId);
            return _mapper.Map<List<ShippingAddressDto>>(addresses);
        }

        public async Task<ShippingAddressDto> CreateAddress(int userId, CreateShippingAddressDto dto)
        {
            var address = new ShippingAddress
            {
                UserId = userId,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                StreetAddress = dto.StreetAddress,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode,
                Country = dto.Country,
                IsDefault = dto.IsDefault
            };

            if (address.IsDefault)
                await ResetDefaultFlag(userId);

            await _repository.Add(address);
            await _repository.SaveChangesAsync();

            return _mapper.Map<ShippingAddressDto>(address);
        }

        public async Task UpdateAddress(int userId, int addressId, UpdateShippingAddressDto dto)
        {
            var address = await _repository.GetById(addressId);

            if (address == null || address.UserId != userId)
                throw new NotFoundException("Address not found");

            address.FullName = dto.FullName;
            address.PhoneNumber = dto.PhoneNumber;
            address.StreetAddress = dto.StreetAddress;
            address.City = dto.City;
            address.State = dto.State;
            address.ZipCode = dto.ZipCode;
            address.Country = dto.Country;
            address.IsDefault = dto.IsDefault;

            if (address.IsDefault)
                await ResetDefaultFlag(userId, excludeId: addressId);

            await _repository.Update(address);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAddress(int userId, int addressId)
        {
            var address = await _repository.GetById(addressId);

            if (address == null || address.UserId != userId)
                throw new NotFoundException("Address not found");

            await _repository.Delete(address);
            await _repository.SaveChangesAsync();
        }

        private async Task ResetDefaultFlag(int userId, int? excludeId = null)
        {
            var addresses = await _repository.GetUserAddresses(userId);
            foreach (var addr in addresses)
            {
                if (excludeId.HasValue && addr.ShippingAddressId == excludeId.Value)
                    continue;
                addr.IsDefault = false;
            }
        }
    }
}
