using ePizzaHub.Core.Contracts;
using ePizzaHub.Infrastructure.Models;
using ePizzaHub.Models.ApiModels.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [AllowAnonymous]
        public async Task<IActionResult> GetCartItemCount(Guid guid)
        {
            if(guid == Guid.Empty)
            {
                return BadRequest("Cart Id can not be empty.");
            }
            var itemCount = await _cartService.GetCartItemCountAsync(guid);
            return Ok(itemCount);
        }

        // This API will be called when a user opens the cart page, to get the cart details from the database and display it in the cart page.
        [HttpGet]
        [Route("get-cart-details")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCartDetailsAsync(Guid cartId)
        {
            var cartDetails = await _cartService.GetCartDetailsAsync(cartId);
            return Ok(cartDetails);
        }

        // This API will be called when a user adds an item to the cart, to add the item to the cart in the database.
        [HttpPost]
        [Route("add-item-to-cart")]
        [AllowAnonymous]
        public async Task<IActionResult> AddItemToCart(AddToCartRequest request)
        {
            var cartDetails = await _cartService.AddItemsToCart(request);
            return Ok(cartDetails);
        }

        // This API will be called when a user deletes an item from the cart, to delete the item from the cart in the database.
        [HttpPut]
        [Route("delete-item")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteItem(Guid CartId, int itemId)
        {
            return Ok();
        }

        // This API will be called when a user updates the quantity of an item in the cart, to update the item quantity in the database.
        [HttpPut]
        [Route("update-item")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateItem(UpdateCartItemRequest updateCartItemRequest)
        {
            var cartDetails = await _cartService.UpdateItemInCartAsync(updateCartItemRequest.CartId, updateCartItemRequest.ItemId, updateCartItemRequest.Quantity);
            return Ok(cartDetails);
        }

        /// <summary>
        /// Purpose:
        /// This API is used to attach the logged-in user's UserId to an existing cart.
        /// (Mostly called just after user login)
        ///
        /// Why authentication is required:
        /// - We need a valid UserId to update the cart.
        /// - Without login, UserId is not available.
        /// 
        /// Security:
        /// - This API is protected by [Authorize] (applied at controller level).
        /// - So it can ONLY be called if user is logged in.
        /// - Request must contain a valid JWT token.
        ///
        /// Behavior:
        /// - If token is missing/invalid → API returns 401 Unauthorized.
        /// - If token is valid → cart will be updated with UserId.
        ///
        /// Important Note (for future practice):
        /// - Make sure JWT token is generated during login.
        /// - Token should be stored (client-side) and sent in request headers.
        /// - Token creation logic is usually handled using ITokenService.
        /// </summary>
        /// <param name="userRequest">
        /// Contains:
        /// - CartId → which cart to update
        /// - UserId → logged-in user's ID
        /// </param>
        /// <returns>
        /// Returns updated cart details after linking it with the user.
        /// </returns>

        [HttpPut]
        [Route("update-cart-user")]
        public async Task<IActionResult> UpdateCartUser(UpdateCartUserRequest userRequest)
        {
            var cartDetails = await _cartService.UpdateCartUser(userRequest.CartId, userRequest.UserId);
            return Ok(cartDetails);
        }
    }
}
