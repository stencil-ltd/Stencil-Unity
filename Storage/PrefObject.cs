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

        public static implicit operator T(PrefObject<T> obj) => obj.Value;
    }
}