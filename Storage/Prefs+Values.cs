using System;

namespace Storage
{
    public partial class Prefs
    {
        #region Getters
        
        internal PrefValue<T> GetStruct<T>(string key) where T : struct
        {
            _lock.EnterReadLock();
            var retval = new PrefValue<T>(this, (T?) (_map.ContainsKey(key) ? _map[key] : null), GetMeta(key));
            _lock.ExitReadLock();
            return retval;
        }

        internal PrefObject<T> GetClass<T>(string key) where T : class
        {
            _lock.EnterReadLock();
            var retval = new PrefObject<T>(this, (T) (_map.ContainsKey(key) ? _map[key] : null), GetMeta(key));
            _lock.ExitReadLock();
            return retval;
        }

        public PrefValue<bool> GetBool(string key)
            => GetStruct<bool>(key);

        public PrefValue<int> GetInt(string key) 
            => GetStruct<int>(key);

        public PrefValue<float> GetFloat(string key)
            => GetStruct<float>(key);

        public PrefValue<long> GetLong(string key)
            => GetStruct<long>(key);

        public PrefValue<double> GetDouble(string key)
            => GetStruct<double>(key);

        public PrefObject<string> GetString(string key)
            => GetClass<string>(key);

        public PrefValue<DateTime> GetDateTime(string key)
        {
            _lock.EnterReadLock();
            var value = _map.ContainsKey(key) ? (DateTime?) DateTime.FromBinary((long) _map[key]) : null;
            var retval = new PrefValue<DateTime>(this, value, GetMeta(key));
            _lock.ExitReadLock();
            return retval;
        }
        
        #endregion
        #region Setters
        
        internal PrefInsertion SetValue<T>(string key, T value)
        {
            _lock.EnterWriteLock();
            _map[key] = value;
            _lock.ExitWriteLock();
            return _meta == null ? new PrefInsertion(this, key) : new PrefInsertion(this, key, UpdateMeta(key));
        }
        
        public PrefInsertion SetBool(string key, bool value) 
            => SetValue(key, value);

        public PrefInsertion SetInt(string key, int value)
            => SetValue(key, value);

        public PrefInsertion SetFloat(string key, float value)
            => SetValue(key, value);

        public PrefInsertion SetLong(string key, long value)
            => SetValue(key, value);

        public PrefInsertion SetDouble(string key, double value)
            => SetValue(key, value);

        public PrefInsertion SetString(string key, string value)
            => SetValue(key, value);

        public PrefInsertion SetDateTime(string key, DateTime value)
            => SetValue(key, value.ToBinary());
        
        #endregion
    }
}