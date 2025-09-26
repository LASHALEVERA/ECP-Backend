using AutoMapper;
using ECPAPI.Data;
using ECPAPI.Models;

namespace ECPAPI.Configurations
{
    public class AutoMapperConfig : Profile
    {

        public AutoMapperConfig()
        {
            CreateMap<ProductDTO, Product>().ReverseMap();
            CreateMap<CategoryDTO, Category>().ReverseMap();
            CreateMap<RoleDTO, Role>().ReverseMap();
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<UserReadOnlyDTO, User>().ReverseMap();
            CreateMap<CartDTO, Cart>().ReverseMap();
            CreateMap<CartItemDTO, CartItem>().ReverseMap();
            CreateMap<CheckoutRequest, CheckoutRequest>().ReverseMap();  //DTO არააქ
            CreateMap<LoginDTO, LoginDTO>().ReverseMap();  //login არააქ
            CreateMap<OrderDTO, Order>().ReverseMap();
            CreateMap<OrderItemDTO, OrderItem>().ReverseMap();
            CreateMap<ProductDTO, Product>().ReverseMap();
            CreateMap<RolePrivilegeDTO, RolePrivilegeDTO>().ReverseMap();
            //CreateMap<RoleDTO, RoleController>().ReverseMap();


        }

    }
}

//CreateMap<Product, ProductDTO>().ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ImageUrls));
//CreateMap<ProductDTO, Product>().ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images));