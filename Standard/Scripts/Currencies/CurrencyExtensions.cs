using Currencies;

namespace Standard.Currencies
{
    public static class CurrencyExtensions
    {
        public static Currency Coins(this CurrencyController mgr) => mgr.Get("Coins");
    }
}