using ePizzaHub.Models.ApiModels.Response;

namespace ePizzaHub.Core.Contracts
{
    public interface ITokenGeneratorService
    {
        string GenerateToekn(ValidateUserResponse userResponse);
    }
}
