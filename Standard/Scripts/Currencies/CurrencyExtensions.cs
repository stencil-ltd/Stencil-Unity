using Currencies;

namespace Standard.Currencies
{
    public static class CurrencyExtensions
    {
        public static Currency Coins(this CurrencyController mgr) => mgr.Get("Coins");
        public static Currency Cash(this CurrencyController mgr) => mgr.Get("Cash");
        public static Currency Wrenches(this CurrencyController mgr) => mgr.Get("Wrenches");
    }
}