using System;
using System.Collections.Generic;
using UnityEngine;
using Currencies;
using JetBrains.Annotations;
using Scripts.Prefs;
using Util;

namespace Store
{
    [CreateAssetMenu(menuName = "Buyables/Buyable")]
    public class Buyable : ScriptableObject
    {
        private static string GetKey(BuyableManager mgr, string id) => $"buy_{mgr.Id}_{id}";

        public string Id;
        public string Title;
        [CanBeNull] public BuyableTag Tag;

        [Header("Assets")]
        public Sprite Icon;
        public Texture Texture;
        public Material Material;
        public Mesh Mesh;
        public GameObject Prefab;
        
        [Header("Money")]
        public Currency Currency;
        public ulong Price;
        public bool InitialGrant;
        
        [Header("Equip")]
        public bool Equippable = true;
        public bool InitialEquip;
        public bool Unlockable;

        [Header("Debug")]
        public bool DebugAcquired;
        public bool DebugEquipped;
        public bool DebugUnlocked;
        
        public BuyableManager Manager { get; internal set; }
        public StencilPrefs Prefs => Manager.Prefs;

        public string Key => GetKey(Manager, Id);
        
        public event EventHandler<Buyable> OnAcquireChanged;
        public event EventHandler<Buyable> OnEquipChanged;
        public event EventHandler<Buyable> OnUnlockChanged;

        internal bool _unsafe;
 
        public DateTime? DateAcquired => Prefs.GetDateTime(Key);
        public bool Acquired
        {
            get { return DateAcquired != null; }
            set
            {
                if (!_unsafe && Acquired == value) return;
                if (value)
                    Prefs.SetDateTime(Key, DateTime.Now);
                else Prefs.DeleteKey(Key);
                Prefs.Save();
                UpdateDebug();
                if (!_unsafe) OnAcquireChanged?.Invoke(this);
            }
        }

        public bool Equipped
        {
            get { return Prefs.GetBool($"{Key}_equip"); }
            set
            {
                if (!_unsafe && value == Equipped) return;
                if (!Equippable) throw new Exception("This cannot be equipped");
                Prefs.SetBool($"{Key}_equip", value);
                Prefs.Save();
                UpdateDebug();
                if (!_unsafe)
                {
                    Manager._OnEquip(this);
                    OnEquipChanged?.Invoke(this);
                }
            }
        }
        
        public DateTime? DateUnlocked => Prefs.GetDateTime($"{Key}_unlock");
        public bool Unlocked
        {
            get { return !Unlockable || DateUnlocked != null; }
            set
            {
                if (!_unsafe && Unlocked == value) return;
                if (value)
                    Prefs.SetDateTime($"{Key}_unlock", DateTime.Now);
                else Prefs.DeleteKey($"{Key}_unlock");
                Prefs.Save();
                UpdateDebug();
                if (!_unsafe) OnUnlockChanged?.Invoke(this);
            }
        }
        
        public bool AttemptToBuy()
        {
            if (Currency.Spend(Price).AndSave())
            {
                Acquired = true;
                return true;
            }
            return false;
        }
        
        public override string ToString()
        {
            return Key;
        }

        private bool _init;
        internal bool Init(BuyableManager manager)
        {
//            if (_init) return false;
            if (!Application.isPlaying) return false;
            _init = true;
            Manager = manager;
            ConfigureDefaults();
            Debug.Log($"Init {this}");
            UpdateDebug();
            return true;
        }

        public void ConfigureDefaults()
        {
            if (Prefs.GetBool($"{Key}_configured")) return;
            _unsafe = true;
            if (InitialGrant)
                Acquired = true;
            if (InitialEquip && Equippable)
                Equipped = true;
            _unsafe = false;
            Prefs.SetBool($"{Key}_configured", true);
            Prefs.Save();
        }

        private void UpdateDebug()
        {
            #if UNITY_EDITOR
            DebugAcquired = Acquired;
            DebugEquipped = Equipped;
            DebugUnlocked = Unlocked;
            #endif
        }
    }
}