using System;
using System.Collections.Generic;
using System.Linq;

namespace Plugins.Collections
{
    public static class CollectionExtensions
    {
        private static Random rng = new Random();
        
        public static T Front<T>(this Deque<T> deque) => deque.First();
        public static T Back<T>(this Deque<T> deque) => deque.Last();

        public static T Random<T>(this IList<T> coll)
        {
            var idx = UnityEngine.Random.Range(0, coll.Count);
            return coll[idx];
        }
        
        public static void Shuffle<T>(this IList<T> list)  
        {  
            var n = list.Count;  
            while (n > 1) {  
                n--;  
                var k = rng.Next(n + 1);  
                var value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }
    }
}