using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.Data;
using UnityEngine;

namespace Store
{
    [CreateAssetMenu(menuName = "Buyables/Manager")]
    public class BuyableManager : ScriptableObject
    {
        private static readonly Dictionary<string, BuyableManager> Registry 
            = new Dictionary<string, BuyableManager>();

        public string Id;
        public bool SingleEquip;
        
        public Buyable[] Buyables;
        
        public EventHandler<Buyable> OnAcquireChanged;
        public EventHandler<Buyable> OnEquipChanged;

        static BuyableManager()
        {
            ResetButton.OnGlobalReset += (sender, args) =>
            {
                foreach (var b in Resources.FindObjectsOfTypeAll<BuyableManager>())
                    b.OnReset(sender, args);
            };
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnLoad()
        {
            foreach (var b in Resources.FindObjectsOfTypeAll<BuyableManager>())
                b.Init();
        }

        private bool _hasInit;
        private void Init()
        {
            if (_hasInit) return;
            if (!Application.isPlaying) return;
            _hasInit = true;
            Registry[Id] = this;
            
            var ids = new HashSet<string>();
            foreach (var b in Buyables)
            {
                if (ids.Contains(b.Id)) throw new Exception($"Duplicate id {b.Id}");
                ids.Add(b.Id);
                b.Init(this);
                b.OnAcquireChanged += OnAcquireChanged;
                b.OnEquipChanged += _OnEquip;
            }
        }

        private void OnReset(object sender, EventArgs eventArgs)
        {
            foreach (var b in Buyables)
                b.Reconfigure();
        }

        private void _OnEquip(object sender, Buyable e)
        {
            if (!SingleEquip)
            {
                OnEquipChanged?.Invoke(sender, e);
                return;
            }
            
            if (e.Equipped)
            {
                foreach (var buyable in Buyables)
                    buyable.Equipped = buyable != e;
                OnEquipChanged?.Invoke(sender, e);
            }
            else
            {
                OnEquipChanged?.Invoke(sender, e);
            }
        }

        public static BuyableManager GetManager(string id) => Registry[id];
        public Buyable Find(string id) => Buyables.First(b => b.Id == id);
        
        public override string ToString()
        {
            return $"Buyable Manager {Id}";
        }
    }
}
