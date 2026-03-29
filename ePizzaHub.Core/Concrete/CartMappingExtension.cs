using ePizzaHub.Infrastructure.Models;
using ePizzaHub.Models.ApiModels.Response;

namespace ePizzaHub.Core.Concrete
{
    public static class CartMappingExtension
    {
        public static CartResponseModel ConvertToCartResponseModel(this Cart cartDetails)
        {
            CartResponseModel cartData = new CartResponseModel()
            {
                Id = cartDetails.Id,
                UserId = cartDetails.UserId,
                CreatedDate = cartDetails.CreatedDate,
            };
            cartData.Items = cartDetails.CartItems
                .Select(x => new CartItemResponseModel
                {
                    Id = x.Id,
                    ItemId = x.ItemId,
                    UnitPrice = x.UnitPrice,
                    Quantity = x.Quantity,
                    ItemName = x.Item.Name,
                    ImageUrl = x.Item.ImageUrl
                }).ToList();

            cartData.Total = cartData.Items.Sum(x => x.UnitPrice * x.Quantity);
            cartData.Tax = Math.Round(cartData.Total * 0.05m, 2); // Assuming a tax rate of 10%
            cartData.GrandTotal = cartData.Total + cartData.Tax;

            return cartData;
        }
    }
}
