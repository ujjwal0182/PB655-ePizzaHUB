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
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(PB655Context dbcontext) : base(dbcontext)
        {
        }

        public async Task<bool> AddNewOrder(Order order)
        {
            _dbcontext.Orders.Add(order);
            int rowsAffected = await _dbcontext.SaveChangesAsync();

            return rowsAffected > 0;
        }
    }
}
