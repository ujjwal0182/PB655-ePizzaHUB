using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.UI.Controllers
{
    [Route("Cart")]
    public class CartController : BaseController
    {
        Guid CartId
        {
            get
            {
                Guid id;
                string cartId = Request.Cookies["CartId"];
                // If the cookie does not exist, create a new (Guid) CartId and set it in the cookie
                if (CartId == null)
                {
                    id = Guid.NewGuid();
                    Response.Cookies.Append("CartId", id.ToString(), new CookieOptions()
                    {
                        Expires = DateTime.UtcNow.AddDays(2) // Set the cookie to expire in 7 days
                    });
                }
                else
                {
                    id = Guid.Parse(cartId);
                }
                return id;
            }
        }
        [HttpGet("Index")]
        public IActionResult Index()
        {
            //Create API to get current cart details.
            return View();
        }

        public async Task<IActionResult> AddToCart(int id, decimal unitPrice, int quantity)
        {
            return View();
        }
    }
}
