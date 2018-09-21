using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Util
{
    [Serializable]
    public class WeightHaver
    {
        [Range(0,1)]
        public float Weight = 1f;

        public WeightHaver()
        {
        }

        public WeightHaver(float weight)
        {
            Weight = weight;
        }
    }

    public static class WeightExtensions
    {
        [CanBeNull]
        public static T WeightedRandom<T>(this ICollection<T> coll) where T : WeightHaver
        {
            var total = coll.Sum(w => w.Weight);
            var rand = Random.Range(0f, total);
            var test = 0f;
            foreach (var p in coll)
            {
                var w = p.Weight;
                if (rand < w + test)
                    return p;
                test += w;
            }

            return null;
        }
    }
}