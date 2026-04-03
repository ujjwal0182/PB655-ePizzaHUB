using ePizzaHub.UI.Helpers;
using ePizzaHub.UI.Models.ApiModels.Response;
using ePizzaHub.UI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace ePizzaHub.UI.Controllers
{
    [Route("Cart")]
    public class CartController : BaseController
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ITokenService tokenService;

        public CartController(IHttpClientFactory httpClientFactory, ITokenService tokenService)
        {
            this.httpClientFactory = httpClientFactory;
            this.tokenService = tokenService;
        }
        Guid CartId
        {
            get
            {
                Guid id;
                string cartId = Request.Cookies["CartId"];
                // If the cookie does not exist, create a new (Guid) CartId and set it in the cookie
                if (cartId == null)
                {
                    id = Guid.NewGuid(); //NewGuid function is used to generate new guid
                    Response.Cookies.Append("CartId", id.ToString(), new CookieOptions()
                    {
                        Expires = DateTime.UtcNow.AddDays(2) // Set the cookie to expire in 7 days
                    });
                }
                else
                {
                    id = Guid.Parse(cartId);
                }
                return id;
            }
        }
        [HttpGet("Index")]
        public async Task <IActionResult> Index()
        {
            //Create API to get current cart details.
            var client = httpClientFactory.CreateClient("ePizzaAPI");
            var items = await client.GetFromJsonAsync<ApiResponseModel<GetCartResponseModel>>($"Cart/get-cart-details?cartId={CartId}");
            return View(items.Data);
        }

        [HttpGet("AddToCart/{itemId:int}/{unitprice:decimal}/{quantity:int}")]
        public async Task<IActionResult> AddToCart(int itemId, decimal unitprice, int quantity)
        {
            var client = httpClientFactory.CreateClient("ePizzaAPI");
            var addToCartRequest = new
            {
                ItemId = itemId,
                Quantity = quantity,
                UnitPrice = unitprice,
                CartId = CartId,
                UserId = CurrentUser != null ? CurrentUser.UserId : 0
            };
            var itemAdded = await client.PostAsJsonAsync("Cart/add-item-to-cart", addToCartRequest);

            //Update the Cart Counter as well
            var CartItemCount = await CartItemsCount(CartId);
            return Json(new { Count = CartItemCount });
        }

        [HttpGet("GetCartItemCount")]
        public async Task<JsonResult> GetCartItemCount()
        {
            var CartItemCount = await CartItemsCount(CartId);
            return Json(new { Count = CartItemCount });
            //I don't need to return the view here, I just need to return the cart count as a JSON response.
        }

        [HttpPut("UpdateQuantity/{itemId:int}/{quantity:int}")]
        public async Task<JsonResult> UpdateQuantity(int itemId, int quantity)
        {
            var client = httpClientFactory.CreateClient("ePizzaAPI");

            var updateCartItems = new
            {
                CartId = CartId,
                ItemId = itemId,
                Quantity = quantity
            };

            var itemAdded = await client.PutAsJsonAsync("Cart/update-item", updateCartItems);

            // After updaing the item quantity, we need to get the updated cart item count and return it as a JSON response.
            var CartItemCount = await CartItemsCount(CartId);
            return Json(new { Count = CartItemCount });
        }

        [NonAction]
        public async Task<int> CartItemsCount(Guid cartId)
        {
            //Create API to get current cart item count.
            var client = httpClientFactory.CreateClient("ePizzaAPI");
            var items = await client.GetFromJsonAsync<ApiResponseModel<int>>($"Cart/get-item-count?guid={cartId}");
           
            return items.Data;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(AddressViewModel addressViewModel)
        {
            if (ModelState.IsValid && CurrentUser is not null)
            {
                var client = httpClientFactory.CreateClient("ePizzaAPI");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenService.GetToken()}");

                var updateCartUserRequest = new
                {
                    CartId = CartId,
                    UserId = CurrentUser.UserId
                };

                var response = await client.PutAsJsonAsync("Cart/update-cart-user", updateCartUserRequest);
                
                response.EnsureSuccessStatusCode(); //It will throw an exception if the response status code is not successful (2xx)

                return View();
            }
            return View();
        }
    }
}
