using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Plugins.Data;
using UnityEngine;

namespace Store
{

    [CreateAssetMenu(menuName = "Buyables/Manager")]
    public class BuyableManager : ScriptableObject
    {
        private static readonly Dictionary<string, BuyableManager> Registry 
            = new Dictionary<string, BuyableManager>();
        public static BuyableManager GetManager(string id) => Registry[id];

        public string Id;
        public bool SingleEquip;
        
        public Buyable[] Buyables;
        
        [CanBeNull] public Buyable SingleEquipped;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void OnLoad()
        {
            foreach (var r in Resources.FindObjectsOfTypeAll<BuyableManager>())
                r.Init();
        }

        private void Init()
        {
            Debug.Log($"Init {this}");
            if (!Application.isPlaying) return;
            Registry[Id] = this;
            
            var ids = new HashSet<string>();
            foreach (var b in Buyables)
            {
                if (ids.Contains(b.Id)) throw new Exception($"Duplicate id {b.Id}");
                ids.Add(b.Id);
                b.Init(this);
                if (SingleEquip && b.Equipped)
                {
                    if (SingleEquipped == null)
                        SingleEquipped = b;
                    else b.Equipped = false;
                }
            }
            ResetButton.OnGlobalReset += OnReset;
        }

        private void OnReset(object sender, EventArgs eventArgs)
        {
            foreach (var b in Buyables)
                b.ConfigureDefaults();
        }

        internal void _OnEquip(Buyable e)
        {
            if (!SingleEquip)
            {
                return;
            }

            if (e.Equipped)
            {
                var old = SingleEquipped;
                SingleEquipped = e;
                if (old != null)
                    old.Equipped = false;
            } 
            else if (SingleEquipped == e)
            {
                SingleEquipped = null;
            }
        }
        
        public override string ToString()
        {
            return $"Buyable Manager {Id}";
        }
    }
}