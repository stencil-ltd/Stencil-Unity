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
        
        public static implicit operator T?(PrefValue<T> obj) => obj.Value;
    }
}