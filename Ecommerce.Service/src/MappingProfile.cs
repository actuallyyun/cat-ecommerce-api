using AutoMapper;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Token mapping
        CreateMap<ResponseToken, ResponseTokenReadDto>();

        // User mappings
        CreateMap<User, UserReadDto>();

        CreateMap<UserCreateDto, User>()
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar));

        // Product mappings
        CreateMap<ProductCreateDto, Product>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ImageCreateDto));

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

        // Review mappings
        CreateMap<Review, ReviewReadDto>();
        
        CreateMap<ReviewImage, ImageReadDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Url));

        CreateMap<ReviewCreateDto, Review>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ImageCreateDto));

        // Category mappings
        CreateMap<Category, CategoryReadDto>();
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryUpdateDto, Category>();
    }
}
