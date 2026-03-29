using ePizzaHub.Core.Contracts;
using ePizzaHub.Infrastructure.Models;
using ePizzaHub.Models.ApiModels.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // This API will be called when a user adds an item to the cart, to get the current cart item count from the database and display it in the cart icon in the header.
        [HttpGet]
        [Route("get-item-count")]
        public async Task<IActionResult> GetCartItemCount(Guid guid)
        {
            var itemCount = await _cartService.GetCartItemCountAsync(guid);
            return Ok(itemCount);
        }

        // This API will be called when a user opens the cart page, to get the cart details from the database and display it in the cart page.
        [HttpGet]
        [Route("get-cart-details")]
        public async Task<IActionResult> GetCartDetailsAsync(Guid cartId)
        {
            var cartDetails = await _cartService.GetCartDetailsAsync(cartId);
            return Ok(cartDetails);
        }

        // This API will be called when a user adds an item to the cart, to add the item to the cart in the database.
        [HttpPost]
        [Route("add-item-to-cart")]
        public async Task<IActionResult> AddItemToCart(AddToCartRequest request)
        {
            var cartDetails = await _cartService.AddItemsToCart(request);
            return Ok(cartDetails);
        }

        // This API will be called when a user deletes an item from the cart, to delete the item from the cart in the database.
        [HttpPut]
        [Route("delete-item")]
        public async Task<IActionResult> DeleteCart()
        {
            return Ok();
        }

        // This API will be called when a user updates the quantity of an item in the cart, to update the item quantity in the database.
        [HttpPut]
        [Route("update-item")]
        public async Task<IActionResult> UpdateItem()
        {
            return Ok();
        }

        //This API will be called when a user logs in, to update the cart with the user details (userId).
        [HttpPut]
        [Route("update-cart-user")]
        public async Task<IActionResult> UpdateCartUser()
        {
            return Ok();
        }
    }
}
