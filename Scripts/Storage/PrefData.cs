using System;
using System.Collections.Generic;

namespace Storage
{
    [Serializable]
    public class PrefData
    {
        public Dictionary<string, object> Values;
        public Dictionary<string, PrefMetadata> Metadata;

        public PrefData() {}
        public PrefData(Dictionary<string, object> values, Dictionary<string, PrefMetadata> metadata)
        {
            Values = new Dictionary<string, object>(values);
            Metadata = new Dictionary<string, PrefMetadata>(metadata);
        }
    }
}