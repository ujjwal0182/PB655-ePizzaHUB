using ePizzaHub.Core.Contracts;
using ePizzaHub.Infrastructure.Models;
using ePizzaHub.Models.ApiModels.Request;
using ePizzaHub.Models.ApiModels.Response;
using ePizzaHub.Repositories.Concrete;
using ePizzaHub.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Core.Concrete
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<bool> AddItemsToCart(AddToCartRequest request)
        {
            var CartDetails = await _cartRepository.GetCartDetailsAsync(request.cartId);
            if (CartDetails == null) // if cart does not exist, new cart
            {
                var inserted = await AddNewCartAsync(request);
                return inserted > 0;
            }
            else
            {
                return await AddItemsToCart(request, CartDetails);
            }
        }

        public async Task<CartResponseModel> GetCartDetailsAsync(Guid cartId)
        {
            var cartDetails = await _cartRepository.GetCartDetailsAsync(cartId);
            if(cartDetails is not null)
            {
                return cartDetails.ConvertToCartResponseModel();
            }
            return new CartResponseModel();
        }

        public async Task<int> GetCartItemCountAsync(Guid cardId)
        {
            return await _cartRepository.GetCartItemQuantityAsync(cardId);
        }
        private async Task<int> AddNewCartAsync(AddToCartRequest request)
        {
            Cart? cartDetails = new Cart()
            {
                Id = request.cartId,
                UserId = request.UserId,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };
            CartItem cartItem = new CartItem()
            {
                ItemId = request.ItemId,
                UnitPrice = request.Unitprice,
                Quantity = request.Quantity,
            };

            cartDetails.CartItems.Add(cartItem);
            await _cartRepository.AddAsync(cartDetails);
            return await _cartRepository.CommitAsync();
        }

        public async Task<bool> AddItemsToCart(AddToCartRequest request, Cart cartDetails)
        {
            CartItem cartItem = cartDetails.CartItems.FirstOrDefault(x => x.ItemId == request.ItemId);
            if(cartItem == null)
            {
                cartItem = new CartItem()
                {
                    CartId = request.cartId,
                    ItemId = request.ItemId,
                    UnitPrice = request.Unitprice,
                    Quantity = request.Quantity,
                };
                cartDetails.CartItems.Add(cartItem);
            }
            else
            {
                // if item already exists in cart, update quantity and unit price
                cartItem.Quantity += request.Quantity;
            }
            _cartRepository.Update(cartDetails);
            int itemAdded = await _cartRepository.CommitAsync();
            return itemAdded > 0;
        }

        public async Task<bool> UpdateItemInCartAsync(Guid cartId, int itemId, int quantity)
        {
            var cartExists = await _cartRepository.GetAllAsync(x => x.Id == cartId);
            if (!cartExists.Any())
            {
                throw new Exception($"Cart with Id {cartId} does not exist");
            }
            int itemAdded = await _cartRepository.UpdateItemQuantity(cartId, itemId, quantity);
            return itemAdded > 0;
        }
    }
}
