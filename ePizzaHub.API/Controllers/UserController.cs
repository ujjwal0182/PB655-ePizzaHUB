using ePizzaHub.Core.Concrete;
using ePizzaHub.Core.Contracts;
using ePizzaHub.Models.ApiModels.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userservice;

        public UserController(IUserService userService)
        {
            this._userservice = userService;    
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest userRequest)
        {
            //Validation
            //Call Bal to pass the user request
            //Call Dal to save into database

            //Call the service
            var result = await _userservice.CreateUserRequestAsync(userRequest);


            return Ok(result);
        }
    }
}
