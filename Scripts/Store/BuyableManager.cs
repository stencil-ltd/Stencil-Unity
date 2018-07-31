using System;
using System.Collections.Generic;
using System.Linq;
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

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadInit()
        {
            foreach (var bm in Resources.FindObjectsOfTypeAll<BuyableManager>())
                bm.Init();
        }

        private void Init()
        {
            Registry[Id] = this;
            foreach (var b in Buyables)
            {
                b.Init(this);
                b.OnAcquireChanged += OnAcquireChanged;
                b.OnEquipChanged += _OnEquip;
            }
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
