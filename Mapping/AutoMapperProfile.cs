using AutoMapper;
using ECommerceAPI.DTOs.CartDTOs;
using ECommerceAPI.DTOs.Category;
using ECommerceAPI.DTOs.OrderDTOs;
using ECommerceAPI.DTOs.ProductDTOs;
using ECommerceAPI.DTOs.ProductImage;
using ECommerceAPI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECommerceAPI.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Product
            CreateMap<Product, ProductReadDto>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<ProductCreateDto, Product>()
                .ForMember(
                    dest => dest.ProductImages,
                    opt => opt.MapFrom(src => src.ProductImageCreateDtos)
                );
            CreateMap<ProductImage, ProductImageDto>();
            CreateMap<ProductImageCreateDto, ProductImage>();


            CreateMap<ProductUpdateDto, Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            // Category
            CreateMap<Category, CategoryDto>();

            // Cart
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => src.Product.Price));

            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.Items,
                    opt => opt.MapFrom(src => src.CartItems));

            // Order
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product.Name));

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Items,
                    opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<Order, AdminOrderDto>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Items,
                    opt => opt.MapFrom(src => src.OrderItems));
        }
    }
}