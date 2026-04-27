using AutoMapper;
using ECommerceAPI.Data;
using ECommerceAPI.DTOs.CartDTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using ECommerceAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartDto> GetCart(int userId)
        {
            var cart = await _cartRepository.GetCartWithItems(userId);
            return _mapper.Map<CartDto>(cart);
        }

        public async Task AddToCart(int userId, int productId, int quantity)
        {
            var cart = await _cartRepository.GetCartByUserId(userId);

            if (quantity <= 0)
                throw new Exception("Quantity must be greater than zero");

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };

                await _cartRepository.AddCart(cart);
            }

            var existingItem = cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                });
            }

            await _cartRepository.SaveChangesAsync();

        }

        public async Task UpdateQuantity(int cartItemId, int quantity, int userId)
        {
            var item = await _cartRepository.GetCartItemById(cartItemId);

            if (item == null)
                throw new Exception("Item not found");

            // SECURITY CHECK
            if (item.Cart.UserId != userId)
                throw new UnauthorizedAccessException("You cannot modify this cart");

            item.Quantity = quantity;

            await _cartRepository.SaveChangesAsync();
        }

        public async Task RemoveItem(int cartItemId, int userId)
        {
            var item = await _cartRepository.GetCartItemById(cartItemId);

            if (item == null)
                throw new Exception("Item not found");

            // SECURITY CHECK
            if (item.Cart.UserId != userId)
                throw new UnauthorizedAccessException("You cannot remove this item");

            await _cartRepository.RemoveCartItem(item);

            await _cartRepository.SaveChangesAsync();
        }
    }
}