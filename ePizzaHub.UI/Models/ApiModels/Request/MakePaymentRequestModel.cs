namespace ePizzaHub.UI.Models.ApiModels.Request
{
    public class MakePaymentRequestModel
    {
        public string Id { get; set; }
        public string TransactionId { get; set; }
        public decimal Tax { get; set; }
        public string Currency { get; set; }
        public decimal Total { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public Guid CartId { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UserId { get; set; }
        public OrderRequest OrderRequest { get; set; }
    }
    public class OrderRequest
    {
        public OrderRequest()
        {
            OrderItems = new List<OrderItems>();
        }
        public string Id { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Locality { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string ZipCode { get; set; } = default!;
        public int UserId { get; set; } = default!;

        /// <summary>
        /// Represents a collection of items in a single order.
        /// One OrderRequest can contain multiple OrderItems (One-to-Many relationship).
        /// Initialized in constructor to avoid null reference issues.
        /// </summary>
        public ICollection<OrderItems> OrderItems { get; set; }

    }
    public class OrderItems
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public int OrderId { get; set; }
    }
}
