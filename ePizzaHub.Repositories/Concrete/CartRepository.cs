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

        public async Task<Cart> GetCartDetailsAsync(Guid guid)
        {
            return await _dbcontext
                .Carts
                .Include(x => x.CartItems)
                .ThenInclude(x => x.Item)
                .Where(x => x.Id == guid && x.IsActive == true)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetCartItemQuantityAsync(Guid guid)
        {
            int itemCounts = await _dbcontext.CartItems.Where(X => X.CartId == guid).CountAsync();
            return itemCounts;
        }

        public async Task<int> UpdateItemQuantity(Guid cartId, int itemId, int quantity)
        {
            var currentItems = await _dbcontext.CartItems
                .Where(x => x.CartId == cartId && x.ItemId == itemId)
                .FirstOrDefaultAsync();
            
            currentItems.Quantity = quantity;

            _dbcontext.Entry(currentItems).State = EntityState.Modified;
            return await _dbcontext.SaveChangesAsync();
        }
    }
}
