using AutoMapper;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserReadDto>();
        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>();

        // Product mappings
        CreateMap<Product, ProductReadDto>();
        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>();

        // Address mappings
        CreateMap<Address, AddressReadDto>();
        CreateMap<AddressCreateDto, Address>();
        CreateMap<AddressUpdateDto, Address>();

        // Order mappings
        CreateMap<OrderCreateDto, Order>();
        CreateMap<OrderUpdateDto, Order>();

        // Review mappings
        CreateMap<Review, ReviewReadDto>();
        CreateMap<ReviewCreateDto, Review>();
        CreateMap<ReviewUpdateDto, Review>();

        // Category mappings
        CreateMap<Category, CategoryReadDto>();
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryUpdateDto, Category>();
    }
}
