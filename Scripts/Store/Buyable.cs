using System;
using System.Collections.Generic;
using UnityEngine;
using Currencies;
using Util;

namespace Store
{
    [CreateAssetMenu(menuName = "Buyables/Buyable")]
    public class Buyable : ScriptableObject
    {
        private static string GetKey(BuyableManager mgr, string id) => $"buy_{mgr.Id}_{id}";

        public string Id;
        public string Title;
        
        [Header("Assets")]
        public Sprite Icon;
        public Texture Texture;
        public Mesh Mesh;
        public GameObject Prefab;
        
        [Header("Money")]
        public Currency Currency;
        public long Price;
        public bool InitialGrant;
        
        [Header("Equip")]
        public bool Equippable;
        public bool InitialEquip;
        
        public BuyableManager Manager { get; internal set; }

        public string Key => GetKey(Manager, Id);
        
        public event EventHandler<Buyable> OnAcquireChanged;
        public event EventHandler<Buyable> OnEquipChanged;

        internal bool _unsafe;
 
        public DateTime? DateAcquired => PlayerPrefsX.GetDateTime(Key);
        public bool Acquired
        {
            get { return DateAcquired != null; }
            set
            {
                if (!_unsafe && Acquired == value) return;
                if (value)
                    PlayerPrefsX.SetDateTime(Key, DateTime.Now);
                else PlayerPrefs.DeleteKey(Key);
                PlayerPrefs.Save();
                if (!_unsafe) OnAcquireChanged?.Invoke(this);
            }
        }

        public bool Equipped
        {
            get { return PlayerPrefsX.GetBool($"{Key}_equip"); }
            set
            {
                if (!_unsafe && value == Equipped) return;
                if (!Equippable) throw new Exception("This cannot be equipped");
                PlayerPrefsX.SetBool($"{Key}_equip", value);
                PlayerPrefs.Save();
                if (!_unsafe)
                {
                    Manager._OnEquip(this);
                    OnEquipChanged?.Invoke(this);
                }
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

        internal void Init(BuyableManager manager)
        {
            if (!Application.isPlaying) return;
            Manager = manager;
            ConfigureDefaults();
            Debug.Log($"Init {this}");
        }

        public void ConfigureDefaults()
        {
            if (PlayerPrefsX.GetBool($"{Key}_configured")) return;
            _unsafe = true;
            if (InitialGrant)
                Acquired = true;
            if (InitialEquip && Equippable)
                Equipped = true;
            _unsafe = false;
            PlayerPrefsX.SetBool($"{Key}_configured", true);
            PlayerPrefs.Save();
        }
    }
}