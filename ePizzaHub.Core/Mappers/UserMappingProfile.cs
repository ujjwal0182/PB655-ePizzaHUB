using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ePizzaHub.Infrastructure.Models;
using ePizzaHub.Models.ApiModels.Request;

namespace ePizzaHub.Core.Mappers
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<CreateUserRequest, User>(); //This will convert CreateUserRequest to User Model.
        }
    }
}
