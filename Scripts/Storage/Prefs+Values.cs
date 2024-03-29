﻿using System;
using UnityEngine;

namespace Storage
{
    public partial class Prefs
    {
        #region Getters

        internal PrefValue<T> GetStruct<T>(string key) where T : struct
        {
            Init();
            _lock.EnterReadLock();
            T? val = null;
            if (_map.ContainsKey(key))
                val = (T)_map[key];
            var retval = new PrefValue<T>(this, val, GetMeta(key));
            _lock.ExitReadLock();
            return retval;
        }

        internal PrefObject<T> GetClass<T>(string key) where T : class
        {
            Init();
            _lock.EnterReadLock();
            T val = null;
            if (_map.ContainsKey(key))
                val = (T) _map[key];
            var retval = new PrefObject<T>(this, val, GetMeta(key));
            _lock.ExitReadLock();
            return retval;
        }

        public PrefValue<bool> GetBool(string key)
            => GetStruct<bool>(key);

        public PrefValue<long> GetLong(string key)
            => GetStruct<long>(key);

        public PrefValue<double> GetDouble(string key)
            => GetStruct<double>(key);

        public PrefObject<string> GetString(string key)
            => GetClass<string>(key);

        public PrefValue<DateTime> GetDateTime(string key)
        {
            Init();
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
            Init();
            _lock.EnterWriteLock();
            _map[key] = value;
            _lock.ExitWriteLock();
            return _meta == null ? new PrefInsertion(this, key) : new PrefInsertion(this, key, UpdateMeta(key));
        }
        
        public PrefInsertion SetBool(string key, bool value) 
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