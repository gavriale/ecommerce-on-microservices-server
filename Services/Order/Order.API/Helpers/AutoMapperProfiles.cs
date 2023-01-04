using AutoMapper;
using Order.API.DTOs;
using Order.API.Entities;

namespace Order.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            
            CreateMap<ShopOrder, OrderItemDto>();
            CreateMap<ShopOrder, UserOrdersDto>()
                .ForMember(dest => dest.ProductId,
                    opt => opt.MapFrom(src => src.OrderItem.Id))
                .ForMember(dest => dest.Quantity,
                    opt => opt.MapFrom(src => src.OrderItem.Quantity));
        }
    }
}
