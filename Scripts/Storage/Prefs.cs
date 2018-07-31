using System.Collections.Generic;
using System.IO;
using System.Threading;
using JetBrains.Annotations;
using Stencil.Util;
using UnityEngine;

namespace Storage
{
    // TODO: observe writes
    // TODO: background saves
    // TODO: transactions

    public partial class Prefs : IPrefs
    {
        private static bool _globalInit;
        private static readonly Dictionary<string, Prefs> Instances 
            = new Dictionary<string, Prefs>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void CreateObject()
        {
            if (_globalInit) return;
            _globalInit = true;
            var obj = new GameObject("Prefs").AddComponent<PrefsBehaviour>();
            Object.DontDestroyOnLoad(obj.gameObject);
        }

        public static Prefs Get(string path = "default")
        {
            Prefs retval;
            lock (Instances)
            {
                if (Instances.TryGetValue(path, out retval))
                    return retval;
                retval = new Prefs(path);
                Instances[path] = retval;
            }
            return retval;
        }

        private bool _init;
        private readonly string _name;
        private Dictionary<string, object> _map;
        [CanBeNull] private Dictionary<string, PrefMetadata> _meta;
        
        private ReaderWriterLockSlim _lock 
            = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        private string Dir => $"{Application.persistentDataPath}/StencilPrefs";
        private string Path => $"{Dir}/{_name}.json";

        private Prefs(string name)
        {
            _name = name;
            _map = new Dictionary<string, object>();
            _meta = new Dictionary<string, PrefMetadata>();
        }

        internal void Init()
        {
            if (_init) return;
            _lock.EnterWriteLock();
            if (_init)
            {
                _lock.ExitWriteLock();
                return;
            }

            if (!Directory.Exists(Dir))
                Directory.CreateDirectory(Dir);
            
            var path = Path;
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var map = (Dictionary<string, object>) Json.Deserialize(json);
                _map = map["map"] as Dictionary<string, object>;
                if (map.ContainsKey("meta"))
                {
                    var meta = map["meta"] as Dictionary<string, object>;
                    if (meta == null)
                    {
                        Debug.LogWarning($"Could not read meta info: {map["meta"]}");
                    }
                    else
                    {
                        foreach (var kv in meta)
                        {
                            var pmeta = PrefMetadata.FromDict(kv.Value as Dictionary<string, object>);
                            if (pmeta.HasValue)
                                _meta[kv.Key] = pmeta.Value; 
                        }
                    }
                }
            }

            _init = true;
            _lock.ExitWriteLock();
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
            if (_map.Count == 0 && _meta?.Count == 0)
            {
                if (File.Exists(name))
                    File.Delete(name);
                _lock.ExitReadLock();
            }
            else
            {
                Dictionary<string, Dictionary<string, object>> meta = null;
                if (_meta != null)
                {
                    meta = new Dictionary<string, Dictionary<string, object>>();
                    foreach (var kv in _meta)
                        meta[kv.Key] = _meta[kv.Key].ToDict();
                }
                var data = new Dictionary<string, object>
                {
                    { "map", _map },
                    { "meta", meta }
                };
                var str = Json.Serialize(data);
                _lock.ExitReadLock();
                File.WriteAllText(Path, str);
            }
        }
        
        public static void SaveAll()
        {
            lock (Instances)
                foreach (var kv in Instances)
                    kv.Value.Save();
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