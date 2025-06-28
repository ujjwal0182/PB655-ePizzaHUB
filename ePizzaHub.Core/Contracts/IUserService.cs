using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ePizzaHub.Models.ApiModels.Request;

namespace ePizzaHub.Core.Contracts
{
    public interface IUserService
    {
        Task<bool> CreateUserRequestAsync(CreateUserRequest createUserRequest);
    }
}
