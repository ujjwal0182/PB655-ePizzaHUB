using AutoMapper;
using ePizzaHub.Infrastructure.Models;
using ePizzaHub.Models.ApiModels.Request;

namespace ePizzaHub.Core.Mappers
{
    public class PaymentMappingProfile : Profile
    {
        public PaymentMappingProfile()
        {
            CreateMap<MakePaymentRequest, PaymentDetail>();
            CreateMap<OrderRequest, Order>();
            CreateMap<OrderItems, OrderItem>();
        }
    }
}
