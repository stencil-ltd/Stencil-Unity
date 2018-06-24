using System;
using JetBrains.Annotations;

namespace Storage
{
    public struct PrefInsertion
    {
        public readonly Prefs Parent;
        public readonly string Key;
        public readonly PrefMetadata Metadata;
        
        public PrefInsertion(Prefs parent, string key)
        {
            Parent = parent;
            Key = key;
        }

        public PrefInsertion(Prefs parent, string key, PrefMetadata metadata)
        {
            Parent = parent;
            Key = key;
            Metadata = metadata;
        }

        PrefInsertion Expire(DateTime? expiration) => Parent.Expire(ref this, expiration);
        public void AndSave() => Parent.Save();
    }
}