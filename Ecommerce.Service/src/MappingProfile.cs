using AutoMapper;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Address mapping

        CreateMap<AddressCreateDto, Address>();
        CreateMap<Address, AddressReadDto>();

        // Token mapping
        CreateMap<ResponseToken, ResponseTokenReadDto>()
            .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.AccessToken))
            .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken));

        // User mappings
        CreateMap<User, UserReadDto>();

        CreateMap<UserCreateDto, User>()
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar));

        // Product mappings
        CreateMap<ProductCreateDto, Product>()
            .ForMember(dest => dest.Images, opt => opt.Ignore());

        CreateMap<ImageCreateDto, ProductImage>()
            .ForMember(dest => dest.ProductId, opt => opt.AllowNull());

        CreateMap<Product, ProductReadDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

        CreateMap<ProductImage, ImageReadDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Url));

        // Order mappings
        CreateMap<OrderCreateDto, Order>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItemCreateDto));

        CreateMap<OrderItemCreateDto, OrderItem>()
            .ForMember(dest => dest.OrderId, opt => opt.Ignore());

        CreateMap<Order, OrderReadDto>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

        CreateMap<OrderItem, OrderItemReadDto>();

        // Review mappings
        CreateMap<Review, ReviewReadDto>()
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

        CreateMap<ReviewCreateDto, Review>();

        // Category mappings
        CreateMap<Category, CategoryReadDto>();
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryUpdateDto, Category>();
    }
}
