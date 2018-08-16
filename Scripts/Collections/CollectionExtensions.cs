using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Plugins.Collections
{
    public static class CollectionExtensions
    {
        public static T Front<T>(this Deque<T> deque) => deque.First();
        public static T Back<T>(this Deque<T> deque) => deque.Last();

        public static T WeightedRandom<T>(this ICollection<T> coll, Func<T, float> weight)
        {
            var total = coll.Sum(weight);
            var rand = Random.Range(0, total);
            var test = 0f;
            foreach (var p in coll)
            {
                var w = weight(p);
                if (rand < w + test)
                    return p;
                test += w;
            }
            throw new Exception("Could not choose by weight. Are there any in the list?");
        }
    }
}