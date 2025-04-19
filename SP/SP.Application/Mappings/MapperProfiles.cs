using AutoMapper;
using SP.Application.Dto.BrandDto;
using SP.Application.Dto.CartDto;
using SP.Application.Dto.CategoryDto;
using SP.Application.Dto.DiscountDto;
using SP.Application.Dto.EmployeeDto;
using SP.Application.Dto.FeedbackDto;
using SP.Application.Dto.ImageDto;
using SP.Application.Dto.LoginDto;
using SP.Application.Dto.OrderDetailDto;
using SP.Application.Dto.OrderDto;
using SP.Application.Dto.ProductDto;
using SP.Application.Dto.ProductVariantDto;
using SP.Application.Dto.ProvinceDto;
using SP.Application.Dto.RoleDto;
using SP.Application.Dto.UserDto;
using SP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Application.Mappings
{
    public class MapperProfiles : Profile
    {

        public MapperProfiles()
        {


            CreateMap<Discount, DiscountViewDto>().ReverseMap();
            CreateMap<Discount, DiscountCreateDto>().ReverseMap();

            CreateMap<FeedBack, FeedbackViewDto>().ReverseMap();
            CreateMap<FeedBack, FeedbackCreateDto>().ReverseMap();

            CreateMap<OrderDetail, OrderDetailViewDto>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailCreateDto>().ReverseMap();

            CreateMap<Order, OrderViewDto>().ReverseMap();
            CreateMap<Order, OrderCreateDto>().ReverseMap();

            CreateMap<Cart, CartViewDto>().ReverseMap();
            CreateMap<Cart, CartCreateDto>().ReverseMap();
            
            CreateMap<Role, RoleViewDto>().ReverseMap();       
            CreateMap<Role, RoleCreateDto>().ReverseMap();

            CreateMap<Category,CategoryViewDto >().ReverseMap();
            CreateMap<Category, CategoryCreateDto>().ReverseMap();

            CreateMap<Brand, BrandViewDto>().ReverseMap();
            CreateMap<Brand, BrandCreateDto>().ReverseMap();

            CreateMap<User, LoginViewDto>().ReverseMap();

            CreateMap<Province, ProvinceViewDto>().ReverseMap();
            CreateMap<District, DistrictViewDto>().ReverseMap();
            CreateMap<Ward, WardViewDto>().ReverseMap();

            CreateMap<Image, ImageFileDto>().ReverseMap();


            CreateMap<User, UserViewDto>().ReverseMap();            
            CreateMap<User, UserCreateDto>().ReverseMap();

            CreateMap<Employee, EmployeeViewDto>().ReverseMap();
            CreateMap<Employee, EmployeeCreateDto>().ReverseMap();
    
            CreateMap<Product, ProductViewDto>().ReverseMap();
            CreateMap<Product, ProductCreateDto>().ReverseMap();

            CreateMap<ProductVariant, VariantViewDto>().ReverseMap();
            CreateMap<ProductVariant, VariantCreateDto>().ReverseMap();




        }
               
    }
}
