using ePizzaHub.Models.ApiModels.Request;
using ePizzaHub.Models.ApiModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Core.Contracts
{
    public interface ICartService
    {
        Task<int> GetCartItemCountAsync(Guid cardId);
        Task<CartResponseModel> GetCartDetailsAsync (Guid cartId);
        Task<bool> AddItemsToCart (AddToCartRequest request);
        Task<bool> UpdateItemInCartAsync(Guid cartId, int itemId, int quantity);
    }
}
