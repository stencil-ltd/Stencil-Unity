using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Plugins.Data;
using UnityEngine;
using Util;

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

        public event EventHandler OnAcquireChanged;
        public event EventHandler OnEquipChanged;

        [Header("Debug")]
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
            SingleEquipped = null;
            foreach (var b in Buyables)
            {
                if (ids.Contains(b.Id)) throw new Exception($"Duplicate id {b.Id}");
                ids.Add(b.Id);
                if (b.Init(this))
                    b.OnAcquireChanged += (sender, args) => OnAcquireChanged?.Invoke(sender, null);
                if (SingleEquip && b.Equipped)
                {
                    if (SingleEquipped == null || SingleEquipped == b)
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

        private bool _recursing;
        internal void _OnEquip(Buyable e)
        {
            if (!SingleEquip)
            {
                OnEquipChanged?.Invoke();
                return;
            }

            if (e.Equipped)
            {
                _recursing = true;
                var old = SingleEquipped;
                SingleEquipped = e;
                if (old != null)
                    old.Equipped = false;
                _recursing = false;
            } 
            else if (SingleEquipped == e)
            {
                SingleEquipped = null;
            }
            
            if (!_recursing)
                OnEquipChanged?.Invoke();
        }
        
        public override string ToString()
        {
            return $"Buyable Manager {Id}";
        }
    }
}