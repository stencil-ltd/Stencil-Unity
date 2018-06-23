using UnityEngine;

namespace Currencies
{
    public static class CurrencyExtensions
    {
        public static Sprite BestSprite(this Currency res)
        {
            Sprite icon = null;
            var mult = res.MultiplierInt();
            var infinite = res.IsInfinite();
            if (mult > 1)
                icon = res.SpriteForMultiplier(mult);
            else if (infinite) 
                icon = res.InfiniteSprite;
            if (icon == null) icon = res.SpecialSprite("small");
            if (icon == null) icon = res.ColorSprite;
            if (icon == null) icon = res.PlainSprite;
            return icon;
        }
    }
}