using ePizzaHub.Core.Concrete;
using ePizzaHub.Core.Contracts;
using ePizzaHub.Repositories.Concrete;
using ePizzaHub.Repositories.Contract;
using Moq;
using Xunit;

namespace ePizzaServiceTest
{
    public class CartServiceTest
    {
        private readonly Mock<ICartRepository> _cartRepository;
        private readonly CartService _cartService;
        public CartServiceTest()
        {
            _cartRepository = new Mock<ICartRepository>();
            _cartService = new CartService(_cartRepository.Object);
        }

        [Fact]
        public async Task GetCartItemCountAsync_When_ProperGuidIsPassed()
        {
            var CartId = Guid.NewGuid();
            var expectedResult = 5;
            _cartRepository.Setup(x => x.GetCartItemQuantityAsync(CartId)).ReturnsAsync(expectedResult);

            var result = await _cartService.GetCartItemCountAsync(CartId);

            Assert.Equal(expectedResult, result);
        }
    }
}
