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
        
        public BuyableManager Manager { get; internal set; }

        public string Key => $"buyable_{Manager.Id}_{Id}";
        
        public event EventHandler<Buyable> OnAcquired;

        private bool _blockEvents;
 
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
                if (value && !_blockEvents) OnAcquired?.Invoke(this, this);
            }
        }
        
        public override string ToString()
        {
            return $"[{Id}] - Buyable {Title} ({Manager})";
        }

        private void Awake()
        {
            if (!Application.isPlaying || !InitialGrant || Acquired) return;
            _blockEvents = true;
            Acquired = true;
            _blockEvents = false;
        }
    }
}