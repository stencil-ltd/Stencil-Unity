using System;
using JetBrains.Annotations;

namespace Storage
{
    public interface IPrefs
    {
        string Name();
        bool HasKey(string key);
        void Clear();
        void Delete(string key);
        void Save();

        PrefValue<bool> GetBool(string key);
        PrefInsertion SetBool(string key, bool value);

        PrefValue<long> GetLong(string key);
        PrefInsertion SetLong(string key, long value);
        
        PrefValue<double> GetDouble(string key);
        PrefInsertion SetDouble(string key, double value);
        
        PrefObject<string> GetString(string key);
        PrefInsertion SetString(string key, [NotNull] string value);
        
        PrefValue<DateTime> GetDateTime(string key);
        PrefInsertion SetDateTime(string key, DateTime value);
    }
}