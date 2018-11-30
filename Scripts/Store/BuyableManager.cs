using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Lifecycle;
using Plugins.Data;
using Scripts.Prefs;
using UnityEngine;
using Util;

namespace Store
{

    [CreateAssetMenu(menuName = "Buyables/Manager")]
    public class BuyableManager : AlwaysScriptableObject
    {
        private static readonly Dictionary<string, BuyableManager> Registry 
            = new Dictionary<string, BuyableManager>();
        public static BuyableManager GetManager(string id) => Registry[id];

        public string Id;
        public bool SingleEquip;

        public Buyable[] Buyables = {};

        public event EventHandler OnAcquireChanged;
        public event EventHandler OnEquipChanged;

        [Header("Debug")]
        [CanBeNull] public Buyable SingleEquipped;
        [CanBeNull] public Buyable[] TagSingleEquipped;
        
        [HideInInspector]
        public StencilPrefs Prefs = StencilPrefs.Default;

        private Dictionary<BuyableTag, Buyable> _tagEquipMap = new Dictionary<BuyableTag, Buyable>();

        private static bool _init;
        public static void Init()
        {
            if (_init) return;
            _init = true;
            Debug.Log("Buyables Loading");
            foreach (var r in Resources.FindObjectsOfTypeAll<BuyableManager>())
                r._Init();
        }

        [CanBeNull]
        public Buyable GetForTag(BuyableTag tag)
        {
            Buyable ret;
            _tagEquipMap.TryGetValue(tag, out ret);
            return ret;
        }

        private void _Init()
        {
            Debug.Log($"Init {this}");
            if (!Application.isPlaying) return;
            Registry[Id] = this;
            
            var ids = new HashSet<string>();
            foreach (var b in Buyables)
            {
                if (ids.Contains(b.Id)) throw new Exception($"Duplicate id {b.Id}");
                ids.Add(b.Id);
                if (b.Init(this))
                    b.OnAcquireChanged += (sender, args) => OnAcquireChanged?.Invoke(sender, null);
            }
            ConfigureSingleEquipped();
            ResetButton.OnGlobalReset += OnReset;
        }

        private void ConfigureSingleEquipped()
        {
            _tagEquipMap.Clear();
            TagSingleEquipped = null;
            SingleEquipped = null;
            foreach (var b in Buyables)
            {
                if (SingleEquip)
                {
                    if (b.Equipped)
                    {
                        if (SingleEquipped == null || SingleEquipped == b)
                            SingleEquipped = b;
                        else b.Equipped = false;
                    }
                } 
                else if (b.Tag?.SingleEquip == true) // pray this shit works.
                {
                    var tag = b.Tag;
                    if (b.Equipped)
                    {
                        Buyable b1;
                        var foundTag = _tagEquipMap.TryGetValue(tag, out b1);
                        if (!foundTag || b1 == b)
                        {
                            _tagEquipMap[tag] = b;
                        }
                        else b.Equipped = false;
                    }
                }
            }
            RebuildSingleTags();
        }

        private void OnReset(object sender, EventArgs eventArgs)
        {
            foreach (var b in Buyables)
                b.ConfigureDefaults();
            ConfigureSingleEquipped();
            OnAcquireChanged?.Invoke();
            OnEquipChanged?.Invoke();
        }

        private bool _recursing;
        internal void _OnEquip(Buyable e)
        {
            if (SingleEquip)
                HandleSingleEquipped(e);
            else if (e.Tag?.SingleEquip == true)
                HandleSingleEquippedTag(e);
            RebuildSingleTags();
            if (!_recursing) OnEquipChanged?.Invoke();
        }

        private void RebuildSingleTags()
        {
            TagSingleEquipped = _tagEquipMap.Values.ToArray();
        }

        private void HandleSingleEquippedTag(Buyable e)
        {
            var tag = e.Tag;
            Buyable old;
            var found = _tagEquipMap.TryGetValue(tag, out old);
            
            if (e.Equipped)
            {
                _recursing = true;
                _tagEquipMap[tag] = e;
                if (found)
                    old.Equipped = false;
                _recursing = false;
            }
            else if (old == e)
            {
                _tagEquipMap.Remove(tag);
            }
        }

        private void HandleSingleEquipped(Buyable e)
        {
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
        }

        public override string ToString()
        {
            return $"Buyable Manager {Id}";
        }
    }
}