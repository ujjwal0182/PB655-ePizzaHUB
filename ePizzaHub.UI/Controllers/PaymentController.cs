using ePizzaHub.UI.Helpers;
using ePizzaHub.UI.Models.ApiModels.Request;
using ePizzaHub.UI.Models.ViewModels;
using ePizzaHub.UI.RazorPay;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.UI.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IRazorPayService _razorPayService;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenService tokenService;

        public PaymentController(
            IRazorPayService razorPayService, 
            IConfiguration configuration, 
            IHttpClientFactory httpClientFactory,
            ITokenService tokenService)
        {
            _razorPayService = razorPayService;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            this.tokenService = tokenService;
        }
        public IActionResult Index()
        {
            PaymentModel payment = new PaymentModel();

            GetCartResponseModel cart = TempData.Peek<GetCartResponseModel>("CartDetail");

            if (cart is not null)
            {
                payment.RazorPayKey = _configuration["RazorPay:Key"]!;
                payment.Cart = cart;
                payment.GrandTotal = Math.Round(cart.GrandTotal);
                payment.Currency = "INR";
                payment.Description = "Payment for Order";
                payment.Receipt = Guid.NewGuid().ToString();

                payment.OrderId = _razorPayService.CreateOrder(payment.GrandTotal, payment.Currency, payment.Receipt); //comes from razorpay

                // order is ready & now this navigate to payment gateway page (.\Payment\Index.cshtml), After successful payment, razorpay will call Status action method of this controller
                // After this we called MakePayment api to store the payment information into my database
                return View(payment);
            }

            return View();
        }

        public async Task<IActionResult> Status(IFormCollection form)
        {
            if (form.Keys.Count > 0)
            {
                string paymentId = form["rzp_paymentid"]!;
                string orderId = form["rzp_orderid"]!;
                string signature = form["rzp_signature"]!;
                string transactionId = form["Receipt"]!;
                string currency = form["Currency"]!;

                bool isSignaturevalid = _razorPayService.VerifySignature(signature, orderId, paymentId);
                if (isSignaturevalid)
                {
                    var payment = _razorPayService.GetPayment(paymentId);
                    string status = payment["status"];

                    //make payment api to store the payment information into database

                    var paymentModel = GetPaymentRequest(paymentId, orderId, transactionId, currency, status);

                    var client = _httpClientFactory.CreateClient("ePizzaAPI");
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenService.GetToken()}");

                    var paymentRequest = await client.PostAsJsonAsync("Payment", paymentModel);
                    paymentRequest.EnsureSuccessStatusCode();

                    Response.Cookies.Delete("CartId"); //after successful payment, we need to clear the cart cookie, because the cart is already converted into an order and we don't want to use the same cart for the next order.
                    TempData.Remove("CartDetail"); //after successful payment, we need to clear the cart detail from temp data, because the cart is already converted into an order and we don't want to use the same cart detail for the next order.
                    TempData.Remove("Address"); //after successful payment, we need to clear the address detail from temp data, because the address is already used for the current order and we don't want to use the same address for the next order.

                    TempData.Set("Paymentdetails", paymentModel); //after successful payment, we need to set the payment status into temp data, because I want to show the payment status on the order confirmation page.)

                    return RedirectToAction("Receipt");
                }
            }
            ViewBag.Message = "Payment failed. Please try again.";
            return View();
        }

        public IActionResult Receipt()
        {
            MakePaymentRequestModel model = TempData.Peek<MakePaymentRequestModel>("Paymentdetails");
            if(model is not null)
            {
                ReceiptViewModel receiptViewModel = new ReceiptViewModel()
                {
                    Currency = model.Currency,
                    GrandTotal = model.GrandTotal,
                    Tax = model.Tax,
                    Total = model.Total,
                    TransactionId = model.TransactionId
                };


                TempData.Remove("Paymentdetails");
                return View(receiptViewModel);
            }
            return View();
        }

        private MakePaymentRequestModel GetPaymentRequest(string paymentId, string orderId, string transactionId, string currency, string status)
        {
            GetCartResponseModel cart = TempData.Peek<GetCartResponseModel>("CartDetail");
            AddressViewModel address = TempData.Peek<AddressViewModel>("Address");

            return new MakePaymentRequestModel()
            {
                CartId = cart.Id,
                Total = cart.Total,
                Tax = cart.Tax,
                GrandTotal = cart.GrandTotal,
                Currency = currency,
                Status = status,
                Email = CurrentUser.Email,
                UserId = CurrentUser.UserId,
                CreatedDate = DateTime.UtcNow,
                Id = paymentId,
                TransactionId = transactionId,
                OrderRequest = new OrderRequest()
                {
                    Id = orderId,
                    Street = address.Street,
                    City = address.City,
                    Locality = address.Locality,
                    ZipCode = address.ZipCode,
                    UserId = CurrentUser.UserId,
                    PhoneNumber = address.PhoneNumber,
                    OrderItems = GetOrderItems(cart.Items)
                }
            };
        }

        private List<OrderItems> GetOrderItems(List<CartItemResponseModel> items)
        {
            List<OrderItems> orderItems = [];

            foreach (CartItemResponseModel item in items)
            {
                OrderItems orderItem = new OrderItems()
                {
                    ItemId = item.Id,
                    Quantity = item.Quantity,
                    Total = item.ItemTotal,
                    UnitPrice = item.UnitPrice,
                };
                orderItems.Add(orderItem);
            }
            return orderItems;
        }
    }
}
