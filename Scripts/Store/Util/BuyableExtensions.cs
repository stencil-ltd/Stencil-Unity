namespace Store.Util
{
    public static class BuyableExtensions
    {
        public static bool CanBuy(this Buyable buyable) 
            => !buyable.Acquired && buyable.Currency.CanSpend(buyable.Price);
    }
}