using System;

namespace Storage
{
    public partial class Prefs
    {
        private PrefMetadata UpdateMeta(string key)
        {
            if (_meta == null)
                throw new Exception("Metadata is turned off. How did you even get here.");
         
            PrefMetadata meta;   
            _meta.TryGetValue(key, out meta);
            meta = new PrefMetadata
            {
                Expiration = null,
                LastUpdate = DateTime.Now
            };
            _meta[key] = meta;
            return meta;
        }

        private PrefMetadata? GetMeta(string key)
        {
            return _meta?[key];
        }

        internal PrefInsertion Expire(ref PrefInsertion old, DateTime? expiration)
        {
            if (_meta == null)
                throw new Exception("Must enable metadata for expiration.");
            
            var meta = _meta[old.Key];
            meta.Expiration = expiration;
            meta.LastUpdate = DateTime.Now;
            return new PrefInsertion(this, old.Key, meta);
        }
    }
}