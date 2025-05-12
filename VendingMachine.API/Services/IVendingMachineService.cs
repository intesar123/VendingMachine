using static VendingMachine.API.Models.Enums;
using VendingMachine.API.Models;

namespace VendingMachine.API.Services
{
    public interface IVendingMachineService
    {
        VendingMachineState InsertCoin(CoinType coin);
        VendingMachineState SelectProduct(ProductType product);
        VendingMachineState GetCurrentState();
        VendingMachineState ReturnCoins();
    }
}
