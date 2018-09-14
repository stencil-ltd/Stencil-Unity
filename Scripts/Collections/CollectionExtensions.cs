﻿using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Plugins.Collections
{
    public static class CollectionExtensions
    {
        public static T Front<T>(this Deque<T> deque) => deque.First();
        public static T Back<T>(this Deque<T> deque) => deque.Last();

        public static T Random<T>(this IList<T> coll)
        {
            var idx = UnityEngine.Random.Range(0, coll.Count);
            return coll[idx];
        }
    }
}