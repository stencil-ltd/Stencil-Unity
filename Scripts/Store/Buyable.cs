using System;
using UnityEngine;
using Currencies;
using Util;

namespace Store
{
    [CreateAssetMenu(menuName = "Buyables/Buyable")]
    public class Buyable : ScriptableObject
    {
        public string Id;
        public string Title;
        
        [Header("Assets")]
        public Sprite Icon;
        public Texture Skin;
        public GameObject Prefab;
        
        [Header("Money")]
        public Currency Currency;
        public long Price;
        public bool InitialGrant;
        
        [Header("Equip")]
        public bool Equippable;
        public bool InitialEquip;
        
        public BuyableManager Manager { get; internal set; }

        public string Key => $"buyable_{Manager.Id}_{Id}";
        
        public event EventHandler<Buyable> OnAcquireChanged;
        public event EventHandler<Buyable> OnEquipChanged;

        private bool _unsafe;
 
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
                if (!_unsafe) OnAcquireChanged?.Invoke(this, this);
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
                if (!_unsafe) OnEquipChanged?.Invoke(this, this);
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
            return $"[{Id}] - Buyable {Title} ({Manager})";
        }

        private bool _init;
        internal void Init(BuyableManager manager)
        {
            if (!Application.isPlaying) return;
            if (_init) throw new Exception($"Double initialized {this}");
            _init = true;
            Manager = manager;
            Reconfigure();
        }

        public void Reconfigure()
        {
            _unsafe = true;
            Acquired = InitialGrant;
            Equipped = InitialEquip && Equippable;
            _unsafe = false;
        }
    }
}