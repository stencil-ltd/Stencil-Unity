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
        public Buyable[] Buyables;
        public EventHandler<Buyable> OnAcquired;

        private void Awake()
        {
            Registry[Id] = this;
            foreach (var b in Buyables)
            {
                if (b.Manager != null) throw new Exception("each buyable can have one manager.");
                b.Manager = this;
                b.OnAcquired += _OnAcquired;
            }
        }

        public static BuyableManager GetManager(string id) => Registry[id];
        public Buyable Find(string id) => Buyables.First(b => b.Id == id);
        
        private void _OnAcquired(object sender, Buyable e) => OnAcquired?.Invoke(sender, e);
        
        public override string ToString()
        {
            return $"Buyable Manager {Id}";
        }
    }
}
