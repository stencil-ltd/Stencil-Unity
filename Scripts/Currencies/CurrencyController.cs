using System;
using System.Collections.Generic;
using Plugins.UI;
using UI;
using UnityEngine;
using Util;

namespace Currencies
{
    public class CurrencyController : Permanent<CurrencyController>
    {
        public Currency[] Types = { };
        public bool SaveOnWrite;
        
        private readonly Dictionary<string, Currency> _types = new Dictionary<string, Currency>();

        public Currency Get(string name)
        {
            return _types[name];
        }

        private void Start()
        {
            if (!Valid) return;
            foreach (var type in Types)
                _types[type.Name] = type;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Save();
        }

        public CurrencyController Save()
        {
            foreach (var type in Types)
                type.Save();
            PlayerPrefs.Save();
            return this;
        }

        public CurrencyController Clear()
        {
            foreach (var type in Types)
                type.Clear();
            return this;
        }
    }
}