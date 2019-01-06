using System;
using System.Collections.Generic;
using System.Linq;
using Binding;
using JetBrains.Annotations;
using Scripts.Util;
using UnityEngine;

namespace Util
{
    public interface IWeightHaver
    {
        float GetWeight();
    }
    
    [Serializable]
    public class WeightHaver : IWeightHaver
    {
        [Range(0,1)]
        [RemoteField("weight")]
        public float Weight = 1f;
        public float GetWeight() => Weight;

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
        public static T WeightedRandom<T>(this ICollection<T> coll, float? total = null) where T : IWeightHaver
        {
            total = total ?? coll.Sum(w => w.GetWeight());
            var rand = StencilRandom.Range(0f, total.Value);
            var test = 0f;
            foreach (var p in coll)
            {
                var w = p.GetWeight();
                if (rand >= test && rand < w + test)
                    return p;
                test += w;
            }

            return default(T);
        }
    }
}