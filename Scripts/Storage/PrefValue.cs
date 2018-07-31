using System;

namespace Storage
{
    public struct PrefValue<T> where T : struct
    {
        public readonly Prefs Parent;
        public readonly PrefMetadata? Metadata;
        private readonly T? _value;
        private bool _expired;

        internal PrefValue(Prefs parent, T? value, PrefMetadata? metadata = null)
        {
            Parent = parent;
            Metadata = metadata;
            _value = value;
            _expired = false;
        }

        public T? Value
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
        
        public static implicit operator T?(PrefValue<T> obj) => obj.Value;  
    }
    
    public class PrefValueHolder<T> where T : struct
    {
        public readonly Prefs Prefs;
        public readonly string Key;
        
        private T? _init;

        public PrefValueHolder(Prefs prefs, string key)
        {
            Prefs = prefs;
            Key = key;
        }

        public PrefValueHolder(string key) : this(Prefs.Get(), key)
        {
        }

        public PrefValue<T> Get()
        {
            var retval = Prefs.GetStruct<T>(Key);
            if (retval.Value == null && _init != null)
            {
                Set(_init.Value).AndSave();
                retval = Prefs.GetStruct<T>(Key);
            }
            return retval;
        }

        public PrefInsertion Set(T value)
        {
            return Prefs.SetValue(Key, value);
        }

        public PrefValueHolder<T> InitializeTo(T initialValue)
        {
            _init = initialValue;
            return this;
        }

        public override string ToString()
        {
            return $"{Get()}";
        }
        
        public static implicit operator T?(PrefValueHolder<T> obj) => obj.Get();
        public static implicit operator T(PrefValueHolder<T> obj) => obj.Get().Value ?? default(T);
    }
}