using ePizzaHub.Core.Concrete;
using ePizzaHub.Core.Contracts;
using ePizzaHub.Models.ApiModels.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ePizzaHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await _itemService.GetItemsAsync();

            ApiResponseModel<IEnumerable<GetItemResponse>> responseFormat 
                = new ApiResponseModel<IEnumerable<GetItemResponse>>(true, items, "Record Fetched");
            return Ok(responseFormat);
        }
    }
}
