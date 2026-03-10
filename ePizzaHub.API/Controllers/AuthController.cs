using ePizzaHub.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) //inject dependency of IAuthService in the constructor, so that we can use it in our controller methods.
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> ValidateUser(string username, string password)
        {
            var response = await _authService.ValidateUserAsync(username, password);
            return Ok(response);
        }
    }
}
