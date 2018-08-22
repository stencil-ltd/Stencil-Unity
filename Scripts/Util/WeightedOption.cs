using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Util
{
    [Serializable]
    public class WeightHaver
    {
        [Range(0,1)]
        public float Weight = 1f;
    }

    public static class WeightExtensions
    {
        public static T WeightedRandom<T>(this ICollection<T> coll) where T : WeightHaver
        {
            var total = coll.Sum(w => w.Weight);
            var rand = Random.Range(0, total);
            var test = 0f;
            foreach (var p in coll)
            {
                var w = p.Weight;
                if (rand < w + test)
                    return p;
                test += w;
            }
            throw new Exception("Could not choose by weight. Are there any in the list?");
        }
    }
}