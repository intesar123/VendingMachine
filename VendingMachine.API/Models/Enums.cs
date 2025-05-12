namespace VendingMachine.API.Models
{
    public class Enums
    {
        public enum CoinType
        {
            Nickel = 5,
            Dime = 10,
            Quarter = 25,
            Penny = 1 // Invalid coin
        }

        public enum ProductType
        {
            Cola = 100,
            Chips = 50,
            Candy = 65
        }
    }
}
