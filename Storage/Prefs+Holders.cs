using System;

namespace Storage
{
    public partial class Prefs
    {
        public PrefValueHolder<bool> HoldBool(string key)
            => HoldValue<bool>(key);

        public PrefValueHolder<int> HoldInt(string key) 
            => HoldValue<int>(key);

        public PrefValueHolder<float> HoldFloat(string key)
            => HoldValue<float>(key);

        public PrefValueHolder<long> HoldLong(string key)
            => HoldValue<long>(key);

        public PrefValueHolder<double> HoldDouble(string key)
            => HoldValue<double>(key);

        public PrefObjectHolder<string> HoldString(string key)
            => HoldObject<string>(key);

        public PrefValueHolder<DateTime> HoldDateTime(string key)
            => HoldValue<DateTime>(key);

        private PrefValueHolder<T> HoldValue<T>(string key) where T : struct 
            => new PrefValueHolder<T>(this, key);

        private PrefObjectHolder<T> HoldObject<T>(string key) where T : class 
            => new PrefObjectHolder<T>(this, key);
    }
}