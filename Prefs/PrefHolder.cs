﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Prefs
{
    public static class PrefHolders
    {
        private static Dictionary<Type, IPrefStrategy> _strategies = new Dictionary<Type, IPrefStrategy>
        {
            { typeof(string), new PrefStrategyString() },
            { typeof(int), new PrefStrategyInt() },
            { typeof(float), new PrefStrategyFloat() },
        };

        internal static IPrefStrategy Get(Type t) 
            => _strategies[t];
        
        public static void RegisterTypeStrategy(Type t, IPrefStrategy strategy)
        {
            _strategies[t] = strategy;
        }
        
        public static PrefHolder<T> Get<T>(string key, T defaultValue = default(T))
        {
            return new PrefHolder<T>(key, defaultValue);
        }

        public static void Save() => PlayerPrefs.Save();
        public static void Clear(bool andSave = false)
        {
            PlayerPrefs.DeleteAll();
            MaybeSave(andSave);
        }

        internal static void MaybeSave(bool andSave)
        {
            if (andSave) Save();
        } 
    }
    
    public class PrefHolder<T>
    {
        public readonly string Key;
        public readonly T Default;
        private readonly IPrefStrategy _strategy;

        internal PrefHolder(string key, T defaultValue)
        {
            Key = key;
            Default = defaultValue;
            _strategy = PrefHolders.Get(typeof(T));
        }

        public T Get() => (T) _strategy.GetValue(Key, Default);

        // Used for random defaults like guids.
        public PrefHolder<T> PersistDefault(bool andSave = false)
        {
            var get = Get();
            if (EqualityComparer<T>.Default.Equals(_strategy.GetValue(Key, default(T))))
                Set(get, andSave);
            return this;
        }

        public void Set(T value, bool andSave = false)
        {
            _strategy.SetValue(Key, value);
            MaybeSave(andSave);
        }
        
        public void Delete(bool andSave = false)
        {
            PlayerPrefs.DeleteKey(Key);
            MaybeSave(andSave);
        }

        public override string ToString()
        {
            return Get()?.ToString() ?? "";
        }

        private static void MaybeSave(bool andSave)
        {
            PrefHolders.MaybeSave(andSave);
        }

        public static implicit operator T(PrefHolder<T> pref) => pref.Get();
    }
}