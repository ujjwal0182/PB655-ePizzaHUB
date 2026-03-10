using ePizzaHub.Core.Contracts;
using ePizzaHub.Models.ApiModels.Response;
using ePizzaHub.Repositories.Contract;

namespace ePizzaHub.Core.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository) // this is Constructor Dependency Injection
        {
            _userRepository = userRepository;
        }
        public async Task<ValidateUserResponse> ValidateUserAsync(string userName, string password)
        {
            var userDetails = await _userRepository.FindByUserNameAsync(userName);

            if (userDetails is null)
                throw new Exception($"The user with email address {userName} doesn't exists");

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, userDetails.Password);

            if (!isValidPassword)
            {
                throw new Exception($"Invalid credentials passed for Email Address: {userName}");
            }
            return new ValidateUserResponse
            {
                Email = userDetails.Email,
                Name = userDetails.Name,
                UserId = userDetails.Id,
                Roles = userDetails.Roles.Select(x => x.Name).ToList() //if user can have both role that's why we use ToList() here
            };
        }
    }
}
