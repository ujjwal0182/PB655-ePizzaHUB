namespace ePizzaHub.UI.Models.ViewModels
{
    public class PaymentModel
    {
        public string Name { get; set; }
        public string RazorPayKey { get; set; }
        public decimal GrandTotal { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public string OrderId { get; set; }
        public GetCartResponseModel Cart { get; set; }
        public string Receipt { get; set; }

    }
}
