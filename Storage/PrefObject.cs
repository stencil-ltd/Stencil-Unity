using System;
using JetBrains.Annotations;

namespace Storage
{
    public struct PrefObject<T> where T : class
    {
        public readonly IPrefs Parent;
        public readonly PrefMetadata? Metadata;
        [CanBeNull] private readonly T _value;
        private bool _expired;

        internal PrefObject(IPrefs parent, [CanBeNull] T value, PrefMetadata? metadata = null)
        {
            Parent = parent;
            Metadata = metadata;
            _value = value;
            _expired = false;
        }

        [CanBeNull]
        public T Value
        {
            get
            {
                if (_expired) return null;
                var expiration = Metadata?.Expiration;
                if (expiration == null) return _value;
                _expired = expiration.Value < DateTime.Now; 
                return _expired ? null : _value;
            }
        }

        public override string ToString()
        {
            return $"{Value}";
        }

        public static implicit operator T(PrefObject<T> obj) => obj.Value;
    }
    
    public class PrefObjectHolder<T> where T : class
    {
        public readonly Prefs Prefs;
        public readonly string Key;

        public PrefObjectHolder(Prefs prefs, string key)
        {
            Prefs = prefs;
            Key = key;
        }

        public PrefObject<T> Get()
        {
            return Prefs.GetClass<T>(Key);
        }

        public PrefInsertion Set(T value)
        {
            return Prefs.SetValue(Key, value);
        }

        public override string ToString()
        {
            return $"{Get()}";
        }

        public static implicit operator T(PrefObjectHolder<T> obj) => obj.Get();  
    }
}