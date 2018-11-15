using Currencies;

namespace Standard.Currencies
{
    public static class CurrencyExtensions
    {
        public static Currency Coins(this CurrencyController mgr) => mgr.Get("Coins");
        public static Currency Cash(this CurrencyController mgr) => mgr.Get("Cash");
        public static Currency Wrenches(this CurrencyController mgr) => mgr.Get("Wrenches");
        
        public static Currency Coins(this CurrencyManager mgr) => mgr.Get("Coins");
        public static Currency Cash(this CurrencyManager mgr) => mgr.Get("Cash");
        public static Currency Wrenches(this CurrencyManager mgr) => mgr.Get("Wrenches");
    }
}