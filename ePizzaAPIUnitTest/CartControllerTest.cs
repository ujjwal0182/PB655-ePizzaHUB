
using ePizzaHub.API.Controllers;
using ePizzaHub.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ePizzaAPIUnitTest
{
    /// <summary>
    /// Initializes test dependencies for CartController.
    /// Since interfaces cannot be instantiated directly, a mock of ICartService is created using Moq.
    /// The mock provides a fake implementation of the service, allowing control over its behavior.
    /// This mock object is then injected into CartController to test its logic independently
    /// without calling real services or external resources like database or APIs.
    /// </summary>
    public class CartControllerTest
    {
        private readonly Mock<ICartService> _cartService;
        private readonly CartController cartController;

        public CartControllerTest()
        {
            _cartService = new Mock<ICartService>(); //Can't create the object of an Interface so we used Mock package.
            cartController = new CartController(_cartService.Object); 
        }

        [Fact]
        public async Task ReturnsOkResult_When_ProperCartIdIsPassed()
        {
            //Arrange
            var cartId = Guid.NewGuid();
            _cartService.Setup(x => x.GetCartItemCountAsync(cartId)).ReturnsAsync(1);

            //Act
            var result =await cartController.GetCartItemCount(cartId);

            //Assert
            var OkObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(1, OkObjectResult.Value); //Here i am expecting the value of OKObjectResult is 1 (from ReturnsAsync(1))
        }

        [Fact]
        public async Task ReturnsBadRequest_When_ProperCartIdIsPassed()
        {
            //Arrange
            Guid cartId = Guid.Empty;
            _cartService.Setup(x => x.GetCartItemCountAsync(cartId)).ReturnsAsync(1);

            //Act
            var result = await cartController.GetCartItemCount(cartId);

            //Assert
            var badObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Cart Id can not be empty.", badObjectResult.Value); //Here i am expecting the value of OKObjectResult is 1 (from ReturnsAsync(1))
        }

        [Theory]
        [InlineData("John","Stark")]
        [InlineData("Rob","Stark")] //If is pass another param its consider as another test case and run the test case for each param.
        public async Task ReturnsABC_When_MultipleParamsPass(string firstname, string lastname)
        {
            //Arrange
            Guid cartId = Guid.Empty;
            _cartService.Setup(x => x.GetCartItemCountAsync(cartId)).ReturnsAsync(1);

            //Act
            var result = await cartController.GetCartItemCount(cartId);

            //Assert
            var badObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Cart Id can not be empty.", badObjectResult.Value); //Here i am expecting the value of OKObjectResult is 1 (from ReturnsAsync(1))
        }
    }
}
