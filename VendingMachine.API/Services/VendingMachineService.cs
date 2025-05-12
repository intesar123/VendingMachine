using static VendingMachine.API.Models.Enums;
using VendingMachine.API.Models;

namespace VendingMachine.API.Services
{
    public class VendingMachineService : IVendingMachineService
    {
        private VendingMachineState _state = new VendingMachineState
        {
            CurrentAmount = 0,
            DisplayMessage = "INSERT COIN"
        };

        public VendingMachineState InsertCoin(CoinType coin)
        {
            if (coin == CoinType.Penny)
            {
                _state.CoinReturn.Add(coin);
                _state.DisplayMessage = _state.CurrentAmount > 0 ?
                    $"${_state.CurrentAmount / 100.0:F2}" :
                    "INSERT COIN";
                return _state;
            }

            _state.CurrentAmount += (int)coin;
            _state.DisplayMessage = $"${_state.CurrentAmount / 100.0:F2}";
            return _state;
        }

        public VendingMachineState SelectProduct(ProductType product)
        {
            if (_state.CurrentAmount >= (int)product)
            {
                _state.CurrentAmount -= (int)product;
                _state.DispensedProduct = product.ToString();
                _state.DisplayMessage = "THANK YOU";
                return _state;
            }

            _state.DisplayMessage = $"PRICE ${(int)product / 100.0:F2}";
            return _state;
        }

        public VendingMachineState GetCurrentState()
        {
            if (_state.DisplayMessage == "THANK YOU")
            {
                var newState = new VendingMachineState
                {
                    CurrentAmount = 0,
                    DisplayMessage = "INSERT COIN",
                    CoinReturn = new List<CoinType>(_state.CoinReturn)
                };
                _state = newState;
                return _state;
            }

            if (_state.DisplayMessage.StartsWith("PRICE"))
            {
                _state.DisplayMessage = _state.CurrentAmount > 0 ?
                    $"${_state.CurrentAmount / 100.0:F2}" :
                    "INSERT COIN";
                return _state;
            }

            return _state;
        }

        public VendingMachineState ReturnCoins()
        {
            var amountToReturn = _state.CurrentAmount;
            _state.CurrentAmount = 0;

            // Convert amount to coins (simplified - just returns quarters for demonstration)
            while (amountToReturn >= 25)
            {
                _state.CoinReturn.Add(CoinType.Quarter);
                amountToReturn -= 25;
            }

            _state.DisplayMessage = "INSERT COIN";
            return _state;
        }
    }
}