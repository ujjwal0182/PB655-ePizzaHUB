using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ePizzaHub.Models.ApiModels.Response;

namespace ePizzaHub.Core.Contracts
{
    public interface IAuthService
    {
        Task<ValidateUserResponse> ValidateUserAsync(string username, string password);
    }
}
