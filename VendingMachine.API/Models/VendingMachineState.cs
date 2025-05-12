using static VendingMachine.API.Models.Enums;

namespace VendingMachine.API.Models
{
    public class VendingMachineState
    {
        public int CurrentAmount { get; set; } // Stored in cents to avoid floating point issues
        public string DisplayMessage { get; set; }
        public List<CoinType> CoinReturn { get; set; } = new List<CoinType>();
        public string? DispensedProduct { get; set; }
    }
}
