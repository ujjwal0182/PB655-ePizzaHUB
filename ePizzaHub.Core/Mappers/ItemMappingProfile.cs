using AutoMapper;
using ePizzaHub.Infrastructure.Models;
using ePizzaHub.Models.ApiModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Core.Mappers
{
    public class ItemMappingProfile : Profile
    {
        public ItemMappingProfile()
        {
            CreateMap<Item, GetItemResponse>(); //This will convert CreateItemRequest to Item Model.
        }
    }
}
