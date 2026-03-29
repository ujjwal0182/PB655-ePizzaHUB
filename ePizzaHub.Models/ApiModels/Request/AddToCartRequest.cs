using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Models.ApiModels.Request
{
    public class AddToCartRequest
    {
        public int UserId { get; set; }
        public Guid cartId { get; set; }
        public int ItemId { get; set; }
        public decimal Unitprice { get; set; }
        public int Quantity { get; set; }
    }
}
