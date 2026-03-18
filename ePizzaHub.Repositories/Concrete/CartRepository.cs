using ePizzaHub.Infrastructure.Models;
using ePizzaHub.Repositories.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Repositories.Concrete
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(PB655Context dbcontext) : base(dbcontext)
        {
        }

        public async Task<int> GetCartItemQuantityAsync(Guid guid)
        {
            int itemCounts = await _dbcontext.CartItems.Where(X => X.CartId == guid).CountAsync();
            return itemCounts;
        }
    }
}
