using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Stencil.Util;

namespace Storage
{
    // TODO: thread safety
    // TODO: observe writes
    // TODO: background saves
    
    public partial class Prefs : IPrefs
    {
        static Prefs()
        {
            Directory.CreateDirectory("StencilPrefs");
        }
        
        private readonly string _name;
        private readonly Dictionary<string, object> _map;
        [CanBeNull] private readonly Dictionary<string, PrefMetadata> _meta;

        public Prefs(string name, bool enableMetadata)
        {
            _name = name;
            _map = new Dictionary<string, object>();
            if (enableMetadata)
                _meta = new Dictionary<string, PrefMetadata>();
        }

        public bool MetadataEnabled() => _meta != null;

        public string Name() 
            => _name;
        
        public bool HasKey(string key)
            => _map.ContainsKey(key);
        
        public void Delete(string key)
        {
            _map.Remove(key);
            _meta?.Remove(key);
        }

        public void Clear()
        {
            _map.Clear();
            _meta?.Clear();
        }

        public void Save()
        {
            var str = Json.Serialize(new PrefData(_map, _meta));
            File.WriteAllText($"StencilPrefs/{_name}.json", str);
        }
    }
}