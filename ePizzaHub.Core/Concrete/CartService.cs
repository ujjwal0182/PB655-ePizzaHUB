using ePizzaHub.Core.Contracts;
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
        public async Task<int> GetCartItemCountAsync(Guid cardId)
        {
            return await _cartRepository.GetCartItemQuantityAsync(cardId);
        }
    }
}
