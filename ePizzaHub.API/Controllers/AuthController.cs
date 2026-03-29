using ePizzaHub.Core.Contracts;
using ePizzaHub.Models.ApiModels.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenGeneratorService _tokenGeneratorService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, ITokenGeneratorService tokenGeneratorService, IConfiguration configuration) //inject dependency of IAuthService in the constructor, so that we can use it in our controller methods.
        {
            _authService = authService;
            _tokenGeneratorService = tokenGeneratorService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> ValidateUser(string username, string password)
        {
            var userDetails = await _authService.ValidateUserAsync(username, password);
            if (userDetails.UserId > 0)
            {
                var accessToken = _tokenGeneratorService.GenerateToekn(userDetails);
                var authAPIResponse = new AuthAPIResponse()
                {
                    AccessToken = accessToken,
                    TokenExpiryInMinutes = Convert.ToInt32(_configuration["Jwt:TokenExpiryInMinutes"]) // Set token expiry time as needed
                };
                return Ok(authAPIResponse);
            }
            return BadRequest("Invalid username or password");
        }
    }
}
