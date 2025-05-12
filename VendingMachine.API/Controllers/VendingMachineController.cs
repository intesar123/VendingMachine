using Microsoft.AspNetCore.Mvc;
using static VendingMachine.API.Models.Enums;
using VendingMachine.API.Services;

namespace VendingMachine.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendingMachineController : ControllerBase
    {
        private readonly IVendingMachineService _vendingMachineService;

        public VendingMachineController(IVendingMachineService vendingMachineService)
        {
            _vendingMachineService = vendingMachineService;
        }

        [HttpGet]
        public IActionResult GetCurrentState()
        {
            var state = _vendingMachineService.GetCurrentState();
            return Ok(state);
        }

        [HttpPost("insert-coin")]
        public IActionResult InsertCoin([FromBody] CoinType coin)
        {
            var state = _vendingMachineService.InsertCoin(coin);
            return Ok(state);
        }

        [HttpPost("select-product")]
        public IActionResult SelectProduct([FromBody] ProductType product)
        {
            var state = _vendingMachineService.SelectProduct(product);
            return Ok(state);
        }

        [HttpPost("return-coins")]
        public IActionResult ReturnCoins()
        {
            var state = _vendingMachineService.ReturnCoins();
            return Ok(state);
        }
    }
}
