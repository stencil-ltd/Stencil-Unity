using System.Collections.Generic;
using System.IO;
using System.Threading;
using JetBrains.Annotations;
using Stencil.Util;

namespace Storage
{
    // TODO: write on exit
    // TODO: observe writes
    // TODO: background saves
    
    public partial class Prefs : IPrefs
    {
        static Prefs()
        {
            Directory.CreateDirectory("StencilPrefs");
        }
        
        private static readonly Dictionary<string, Prefs> Instances 
            = new Dictionary<string, Prefs>();

        public static Prefs Get(string path)
        {
            lock (Instances)
            {
                var retval = Instances[path];
                if (retval != null) return retval;
                retval = new Prefs(path);
                Instances[path] = retval;
                return retval;
            }
        }
        
        private readonly string _name;
        private readonly Dictionary<string, object> _map;
        [CanBeNull] private readonly Dictionary<string, PrefMetadata> _meta;
        
        private ReaderWriterLockSlim _lock 
            = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        private Prefs(string name)
        {
            _name = name;
            _map = new Dictionary<string, object>();
            _meta = new Dictionary<string, PrefMetadata>();
        }

        public string Name() => _name;
        
        public bool HasKey(string key)
        {
            _lock.EnterReadLock();
            var retval = _map.ContainsKey(key);
            _lock.ExitReadLock();
            return retval;
        }

        public void Delete(string key)
        {
            _lock.EnterWriteLock();
            _map.Remove(key);
            _meta?.Remove(key);
            _lock.ExitWriteLock();
        }

        public void Clear()
        {
            _lock.EnterWriteLock();
            _map.Clear();
            _meta?.Clear();
            _lock.ExitWriteLock();
        }

        public void Save()
        {
            _lock.EnterReadLock();
            var name = $"StencilPrefs/{_name}.json";
            if (_map.Count == 0 && _meta.Count == 0)
            {
                File.Delete(name);
                _lock.ExitReadLock();
            }
            else
            {
                var str = Json.Serialize(new PrefData(_map, _meta));
                _lock.ExitReadLock();
                File.WriteAllText($"StencilPrefs/{_name}.json", str);   
            }
        }

        public static void ClearAll()
        {
            lock(Instances)
            {
                foreach (var kv in Instances)
                {
                    kv.Value.Clear();
                    kv.Value.Save();
                }
            }
        }
    }
}