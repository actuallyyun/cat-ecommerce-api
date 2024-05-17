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
        CreateMap<ProductCreateDto, Product>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ImageCreateDto));

        CreateMap<ImageCreateDto, Image>()
            .ForMember(dest => dest.ProductId, opt => opt.AllowNull())
            .ForMember(dest => dest.ReviewId, opt => opt.AllowNull());

        CreateMap<ProductUpdateDto, Product>();

        // Address mappings
        CreateMap<Address, AddressReadDto>();
        CreateMap<AddressCreateDto, Address>();
        CreateMap<AddressUpdateDto, Address>();

        // Order mappings
        CreateMap<OrderCreateDto, Order>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItemCreateDto));

        CreateMap<OrderItemCreateDto, OrderItem>()
            .ForMember(dest => dest.OrderId, opt => opt.Ignore());

        CreateMap<OrderUpdateDto, Order>();

        // Review mappings
        CreateMap<Review, ReviewReadDto>();

        CreateMap<ReviewCreateDto, Review>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ImageCreateDto));

        CreateMap<ReviewUpdateDto, Review>();

        // Category mappings
        CreateMap<Category, CategoryReadDto>();
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryUpdateDto, Category>();
    }
}
