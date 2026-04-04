using ePizzaHub.Core.Contracts;
using ePizzaHub.Models.ApiModels.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Win32.SafeHandles;

namespace ePizzaHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// This enpoint will be invoked only once when my payment successfull from razorpay
        /// </summary>
        /// <param name="makePaymentRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> MakePayment([FromBody] MakePaymentRequest paymentRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _paymentService.MakePayment(paymentRequest);
                return Ok();
            }
            return BadRequest("Please check view");
        }
    }
}
