using System.Runtime.InteropServices;
using Dev;
using UnityEngine;

namespace Util
{
    public class PlatformValue<T>
    {
        public static implicit operator T(PlatformValue<T> plat) => plat.Value;
        
        public T Value { get; private set; }
        public bool IsDeveloper => Developers.Enabled;

        public PlatformValue()
        {
        }

        public PlatformValue(T value)
        {
            Value = value;
        }

        public PlatformValue<T> WithAndroid(T value, T developer) => WithAndroid(IsDeveloper ? developer : value);
        public PlatformValue<T> WithAndroid(T value)
        {
            if (Application.platform == RuntimePlatform.Android)
                Value =  value;
            return this;
        }

        public PlatformValue<T> WithIos(T value, T developer) => WithIos(IsDeveloper ? developer : value);
        public PlatformValue<T> WithIos(T value)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                Value = value;
            return this;
        }

        public PlatformValue<T> WithEditor(T value, T developer) => WithEditor(IsDeveloper ? developer : value);
        public PlatformValue<T> WithEditor(T value)
        {
            if (Application.isEditor)
                Value = value;
            return this;
        }
    }
}