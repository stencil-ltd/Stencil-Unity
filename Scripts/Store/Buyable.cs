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
 
        public DateTime? DateAcquired => PlayerPrefsX.GetDateTime(Key);
        public bool Acquired
        {
            get { return DateAcquired != null; }
            set
            {
                if (Acquired == value) return;
                if (value)
                    PlayerPrefsX.SetDateTime(Key, DateTime.Now);
                else PlayerPrefs.DeleteKey(Key);
                PlayerPrefs.Save();
                OnAcquireChanged?.Invoke(this, this);
            }
        }

        public bool Equipped
        {
            get { return PlayerPrefsX.GetBool($"{Key}_equip"); }
            set
            {
                if (value == Equipped) return;
                if (!Equippable) throw new Exception("This cannot be equipped");
                PlayerPrefsX.SetBool($"{Key}_equip", value);
                PlayerPrefs.Save();
                OnEquipChanged?.Invoke(this, this);
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

        internal void Init(BuyableManager manager)
        {
            if (!Application.isPlaying) return;
            if (Manager != null) throw new Exception($"Already initialized by {Manager}");
            Manager = manager;
            if (InitialGrant && !Acquired)
                Acquired = true;
            if (InitialEquip && Equippable && !Equipped)
                Equipped = true;
        }
    }
}