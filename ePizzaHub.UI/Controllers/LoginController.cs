using ePizzaHub.UI.Models.ApiModels.Request;
using ePizzaHub.UI.Models.ApiModels.Response;
using ePizzaHub.UI.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ePizzaHub.UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// This is how we call an API in the controller from which we configured in program.cs
        /// </summary>
        public LoginController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel request)
        {
            if (ModelState.IsValid)
            {
                var client = httpClientFactory.CreateClient("ePizzaAPI"); //create client and pass the same name (ePizzaAPI) as given in DI

                var userDetails = await client.GetFromJsonAsync<ApiResponseModel<ValidateUserResponse>>(
                                        $"Auth?userName={request.EmailAddress}&password={request.Password}");

                if (userDetails.Success)
                {
                    var accessToken = userDetails.Data.AccessToken;
                    var tokenExpiryInMinutes = userDetails.Data.TokenExpiryInMinutes;
                    var TokenHandler = new JwtSecurityTokenHandler();
                    var TokenDetails = TokenHandler.ReadJwtToken(accessToken) as JwtSecurityToken;

                    List<Claim> claims = new List<Claim>();

                    foreach(var item in TokenDetails.Claims)
                    {
                        claims.Add(new Claim(item.Type, item.Value));
                    }
                    await GenerateTicket(claims);
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel request)
        {
            if (ModelState.IsValid)
            {
                var client = httpClientFactory.CreateClient("ePizzaAPI"); //call the API from which we configured in program.cs

                //here we are calling the API to create a user, we are passing the user details in the form of CreateUserRequestModel, which we have created in the Models/ApiModels/Request folder.
                HttpResponseMessage? userDetails = await client.PostAsJsonAsync<CreateUserRequestModel>("User",
                    new CreateUserRequestModel()
                    {
                        Name = request.UserName,
                        Email = request.Email,
                        Password = request.Password,
                        PhoneNumber = request.PhoneNumber
                    });
                userDetails.EnsureSuccessStatusCode();
            }
            return View();
        }

        /// <summary>
        /// This is the non-action method GenerateTickets, Inside the generatetickets basically I need to paas a list of claims.
        /// abhi tk we not done Token based authentication in my API, ye hamne abhi simple ese he method banaya hai further move krne k loye.
        /// & CookieAuthenticationDefaults also save in the browser.
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>

        /// purpose of this method is to generate a cookie based authentication ticket for the user, which will be used to authenticate the user in subsequent requests.
        /// Is method ka kaam hai user ke claims ko use karke authentication ticket create karna aur cookie me store karna, taaki user login ho jaye.

        private async Task GenerateTicket(List<Claim> claims)
        {
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            //with the use of SignInAsync i am able to use a cookie based authentication to simply login my user, my frontend application.
            //This line basically save everything in the current session, and it will be used to authenticate the user in subsequent requests, it will create a cookie based authentication ticket for the user, which will be used to authenticate the user in subsequent requests.
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties()
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
                });
        }


        ///
        /// This is not done yet, I also need to tell to program.cs as well that i am using a cookie authentication
        ///
    }
}
