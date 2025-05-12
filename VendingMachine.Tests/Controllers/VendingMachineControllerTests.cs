using Microsoft.AspNetCore.Mvc;
using static VendingMachine.API.Models.Enums;
using VendingMachine.API.Controllers;
using VendingMachine.API.Models;
using VendingMachine.API.Services;
using Moq;

namespace VendingMachine.Tests.Controllers
{
    [TestClass]
    public class VendingMachineControllerTests
    {
        private Mock<IVendingMachineService> _mockService;
        private VendingMachineController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockService = new Mock<IVendingMachineService>();
            _controller = new VendingMachineController(_mockService.Object);
        }

        [TestMethod]
        public void GetCurrentState_ReturnsOkResultWithState()
        {
            // Arrange
            var expectedState = new VendingMachineState
            {
                CurrentAmount = 25,
                DisplayMessage = "$0.25"
            };
            _mockService.Setup(s => s.GetCurrentState()).Returns(expectedState);

            // Act
            var result = _controller.GetCurrentState();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(expectedState, okResult.Value);
        }

        [TestMethod]
        public void InsertCoin_WithValidCoin_ReturnsOkResultWithUpdatedState()
        {
            // Arrange
            var coin = CoinType.Quarter;
            var expectedState = new VendingMachineState
            {
                CurrentAmount = 25,
                DisplayMessage = "$0.25"
            };
            _mockService.Setup(s => s.InsertCoin(coin)).Returns(expectedState);

            // Act
            var result = _controller.InsertCoin(coin);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(expectedState, okResult.Value);
            _mockService.Verify(s => s.InsertCoin(coin), Times.Once);
        }

        [TestMethod]
        public void SelectProduct_WithValidProduct_ReturnsOkResultWithUpdatedState()
        {
            // Arrange
            var product = ProductType.Cola;
            var expectedState = new VendingMachineState
            {
                CurrentAmount = 0,
                DisplayMessage = "THANK YOU",
                DispensedProduct = "Cola"
            };
            _mockService.Setup(s => s.SelectProduct(product)).Returns(expectedState);

            // Act
            var result = _controller.SelectProduct(product);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(expectedState, okResult.Value);
            _mockService.Verify(s => s.SelectProduct(product), Times.Once);
        }

        [TestMethod]
        public void ReturnCoins_ReturnsOkResultWithUpdatedState()
        {
            // Arrange
            var expectedState = new VendingMachineState
            {
                CurrentAmount = 0,
                DisplayMessage = "INSERT COIN",
                CoinReturn = new List<CoinType> { CoinType.Quarter }
            };
            _mockService.Setup(s => s.ReturnCoins()).Returns(expectedState);

            // Act
            var result = _controller.ReturnCoins();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(expectedState, okResult.Value);
            _mockService.Verify(s => s.ReturnCoins(), Times.Once);
        }

        [TestMethod]
        public void GetCurrentState_AfterProductSelection_ResetsStateOnSecondCall()
        {
            // Arrange
            var thankYouState = new VendingMachineState
            {
                DisplayMessage = "THANK YOU"
            };
            var resetState = new VendingMachineState
            {
                DisplayMessage = "INSERT COIN",
                CurrentAmount = 0
            };

            _mockService.SetupSequence(s => s.GetCurrentState())
                .Returns(thankYouState)
                .Returns(resetState);

            // Act - First call
            var firstResult = _controller.GetCurrentState();

            // Assert - First call
            Assert.IsInstanceOfType(firstResult, typeof(OkObjectResult));
            var firstOkResult = firstResult as OkObjectResult;
            Assert.AreEqual("THANK YOU", ((VendingMachineState)firstOkResult.Value).DisplayMessage);

            // Act - Second call
            var secondResult = _controller.GetCurrentState();

            // Assert - Second call
            Assert.IsInstanceOfType(secondResult, typeof(OkObjectResult));
            var secondOkResult = secondResult as OkObjectResult;
            Assert.AreEqual("INSERT COIN", ((VendingMachineState)secondOkResult.Value).DisplayMessage);
        }

        [TestMethod]
        public void InsertCoin_WithInvalidCoin_ReturnsOkResultWithRejectedCoin()
        {
            // Arrange
            var coin = CoinType.Penny;
            var expectedState = new VendingMachineState
            {
                CurrentAmount = 0,
                DisplayMessage = "INSERT COIN",
                CoinReturn = new List<CoinType> { CoinType.Penny }
            };
            _mockService.Setup(s => s.InsertCoin(coin)).Returns(expectedState);

            // Act
            var result = _controller.InsertCoin(coin);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            var returnedState = okResult.Value as VendingMachineState;
            Assert.AreEqual(1, returnedState.CoinReturn.Count);
            Assert.AreEqual(CoinType.Penny, returnedState.CoinReturn[0]);
        }
    }
}
