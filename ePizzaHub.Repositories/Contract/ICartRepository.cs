using ePizzaHub.Infrastructure.Models;

namespace ePizzaHub.Repositories.Contract
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<int> GetCartItemQuantityAsync (Guid guid);
        Task<Cart> GetCartDetailsAsync(Guid cartId);
    }
}
