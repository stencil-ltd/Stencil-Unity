using System;
using JetBrains.Annotations;
using Prefs;
using UnityEngine;

namespace Util
{
    public static class PlayerPrefsX
    {
        #region Strategies

        private class BoolStrategy : IPrefStrategy
        {
            public object GetValue(string key, object defaultValue)
            {
                return GetBool(key, (bool) defaultValue);
            }

            public void SetValue(string key, object value)
            {
                SetBool(key, (bool) value);
            }
        }

        private class LongStrategy : IPrefStrategy
        {
            public object GetValue(string key, object defaultValue)
            {
                return GetLong(key, (long) defaultValue);
            }

            public void SetValue(string key, object value)
            {
                SetLong(key, (long) value);
            }
        }

        private class DateTimeStrategy : IPrefStrategy
        {
            public object GetValue(string key, object defaultValue)
            {
                return GetDateTime(key, (DateTime?) defaultValue);
            }

            public void SetValue(string key, object value)
            {
                SetDateTime(key, (DateTime?) value);
            }
        }
        
        #endregion
        
        public static void RegisterStrategies()
        {
            PrefHolders.RegisterTypeStrategy(typeof(bool), new BoolStrategy());
            PrefHolders.RegisterTypeStrategy(typeof(long), new LongStrategy());
            PrefHolders.RegisterTypeStrategy(typeof(DateTime?), new DateTimeStrategy());
        }
        
        public static bool GetBool(string key, bool defaultValue = false)
        {
            var intVal = PlayerPrefs.GetInt(key, -1);
            switch (intVal)
            {
                case 1:
                    return true;
                case -1:
                    return defaultValue;
                default:
                    return false;
            }
        }

        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }
        
        public static long GetLong(string key, long defaultValue = 0L)
        {
            var str = PlayerPrefs.GetString(key);
            return string.IsNullOrEmpty(str) ? defaultValue : long.Parse(str);
        }

        public static void SetLong(string key, long value)
        {
            PlayerPrefs.SetString(key, value.ToString());
        }

        public static DateTime? GetDateTime(string key, DateTime? defaultValue = null)
        {
            var binary = GetLong(key);
            return binary == 0 ? defaultValue : DateTime.FromBinary(binary);
        }

        public static void SetDateTime(string key, DateTime? value)
        {
            if (value.HasValue)
                SetLong(key, value.Value.ToBinary());
            else PlayerPrefs.DeleteKey(key);
        }

        public static T GetJson<T>(string key)
        {
            var str = PlayerPrefs.GetString(key);
            if (string.IsNullOrEmpty(str)) return default(T);
            return JsonUtility.FromJson<T>(str);
        }

        public static void SetJson<T>(string key, T value)
        {
            PlayerPrefs.SetString(key, JsonUtility.ToJson(value));
        }
    }
}