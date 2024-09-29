using AutoMapper;
using CustomerOrder.API.Contracts.CustomerOrders.Dtos;
using CustomerOrder.Data.Models;

namespace CustomerOrder.API.Contracts
{
    public class CustomerOrderProfile : Profile
    {
        public CustomerOrderProfile()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<CustomerOrder.Data.Models.CustomerOrder, CustomerOrderDto>()
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
                .ReverseMap();
        }
    }
}
