using System;

namespace Storage
{
    public partial class Prefs
    {
        private PrefMetadata UpdateMeta(string key)
        {
            if (_meta == null)
                throw new Exception("Metadata is turned off. How did you even get here.");
         
            _lock.EnterWriteLock();
            PrefMetadata meta;   
            _meta.TryGetValue(key, out meta);
            meta = new PrefMetadata
            {
                Expiration = null,
                LastUpdate = DateTime.Now
            };
            _meta[key] = meta;
            _lock.ExitWriteLock();
            return meta;
        }

        private PrefMetadata? GetMeta(string key)
        {
            if (_meta == null) return null;
            _lock.EnterReadLock();
            var retval = _meta?[key];
            _lock.ExitReadLock();
            return retval;
        }

        internal PrefInsertion Expire(ref PrefInsertion old, DateTime? expiration)
        {
            if (_meta == null)
                throw new Exception("Must enable metadata for expiration.");
            
            _lock.EnterWriteLock();
            var meta = _meta[old.Key];
            meta.Expiration = expiration;
            meta.LastUpdate = DateTime.Now;
            _map[old.Key] = meta;
            _lock.ExitWriteLock();
            
            return new PrefInsertion(this, old.Key, meta);
        }
    }
}