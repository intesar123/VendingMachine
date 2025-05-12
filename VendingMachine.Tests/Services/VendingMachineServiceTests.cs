using VendingMachine.API.Services;
using static VendingMachine.API.Models.Enums;

namespace VendingMachine.Tests.Services
{
    [TestClass]
    public class VendingMachineServiceTests
    {
        private IVendingMachineService _service;

        [TestInitialize]
        public void TestInitialize()
        {
            _service = new VendingMachineService();
        }

        [TestMethod]
        public void InsertCoin_WithPenny_RejectsCoin()
        {
            var state = _service.InsertCoin(CoinType.Penny);

            Assert.AreEqual(0, state.CurrentAmount);
            Assert.AreEqual("INSERT COIN", state.DisplayMessage);
            Assert.AreEqual(1, state.CoinReturn.Count);
            Assert.AreEqual(CoinType.Penny, state.CoinReturn[0]);
        }

        [TestMethod]
        public void InsertCoin_WithNickel_AcceptsCoin()
        {
            var state = _service.InsertCoin(CoinType.Nickel);

            Assert.AreEqual(5, state.CurrentAmount);
            Assert.AreEqual("$0.05", state.DisplayMessage);
            Assert.AreEqual(0, state.CoinReturn.Count);
        }

        [TestMethod]
        public void InsertCoin_WithDime_AcceptsCoin()
        {
            var state = _service.InsertCoin(CoinType.Dime);

            Assert.AreEqual(10, state.CurrentAmount);
            Assert.AreEqual("$0.10", state.DisplayMessage);
            Assert.AreEqual(0, state.CoinReturn.Count);
        }

        [TestMethod]
        public void InsertCoin_WithQuarter_AcceptsCoin()
        {
            var state = _service.InsertCoin(CoinType.Quarter);

            Assert.AreEqual(25, state.CurrentAmount);
            Assert.AreEqual("$0.25", state.DisplayMessage);
            Assert.AreEqual(0, state.CoinReturn.Count);
        }

        [TestMethod]
        public void SelectProduct_WithInsufficientFunds_ShowsPrice()
        {
            _service.InsertCoin(CoinType.Quarter); // $0.25
            var state = _service.SelectProduct(ProductType.Cola); // $1.00

            Assert.AreEqual("PRICE $1.00", state.DisplayMessage);
            Assert.IsNull(state.DispensedProduct);
        }

        [TestMethod]
        public void SelectProduct_WithExactChange_DispensesProduct()
        {
            _service.InsertCoin(CoinType.Quarter);
            _service.InsertCoin(CoinType.Quarter);
            _service.InsertCoin(CoinType.Quarter);
            _service.InsertCoin(CoinType.Quarter); // $1.00
            var state = _service.SelectProduct(ProductType.Cola); // $1.00

            Assert.AreEqual("THANK YOU", state.DisplayMessage);
            Assert.AreEqual("Cola", state.DispensedProduct);
        }

        [TestMethod]
        public void GetCurrentState_AfterThankYou_ResetsToInsertCoin()
        {
            // Arrange
            _service.InsertCoin(CoinType.Quarter);
            _service.InsertCoin(CoinType.Quarter); // $0.50
            //_service.SelectProduct(ProductType.Chips); // $0.50

            // Act - First check shows THANK YOU
            var firstState = _service.SelectProduct(ProductType.Chips);

            // Act - Second check should show INSERT COIN
            var secondState = _service.GetCurrentState();

            Assert.AreEqual("THANK YOU", firstState.DisplayMessage);
            Assert.AreEqual("INSERT COIN", secondState.DisplayMessage);
            Assert.AreEqual(0, secondState.CurrentAmount);
        }

        [TestMethod]
        public void ReturnCoins_ReturnsInsertedCoins()
        {
            // Arrange
            _service.InsertCoin(CoinType.Quarter);
            _service.InsertCoin(CoinType.Dime);
            _service.InsertCoin(CoinType.Nickel); // $0.40

            // Act
            var state = _service.ReturnCoins();

            // Assert
            Assert.AreEqual(0, state.CurrentAmount);
            Assert.AreEqual("INSERT COIN", state.DisplayMessage);
            Assert.IsTrue(state.CoinReturn.Count > 0);
        }

        [TestMethod]
        public void GetCurrentState_AfterPrice_ShowsCurrentAmount()
        {
            // Arrange
            _service.InsertCoin(CoinType.Quarter); // $0.25
            _service.SelectProduct(ProductType.Candy); // $0.65 - shows PRICE

            // Act
            var state = _service.GetCurrentState();

            // Assert
            Assert.AreEqual("$0.25", state.DisplayMessage);
        }

        [TestMethod]
        public void GetCurrentState_AfterPriceWithNoMoney_ShowsInsertCoin()
        {
            // Arrange - No coins inserted
            _service.SelectProduct(ProductType.Candy); // $0.65 - shows PRICE

            // Act
            var state = _service.GetCurrentState();

            // Assert
            Assert.AreEqual("INSERT COIN", state.DisplayMessage);
        }
    }
}
