using System;
using System.Collections.Generic;
using UnityEngine;

namespace Storage
{
    [Serializable]
    internal class PrefData
    {
        [SerializeField]
        public Dictionary<string, object> Values;
        [SerializeField]
        public Dictionary<string, PrefMetadata> Metadata;

        public PrefData(Dictionary<string, object> values, Dictionary<string, PrefMetadata> metadata)
        {
            Values = values;
            Metadata = metadata;
        }
    }
}