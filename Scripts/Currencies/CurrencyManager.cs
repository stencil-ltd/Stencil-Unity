﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Currencies
{
    [CreateAssetMenu(menuName = "Managers/Currency")]
    public class CurrencyManager : Singleton<CurrencyManager>
    {
        public Currency[] Types;
        public bool SaveOnWrite;
        
        private readonly Dictionary<string, Currency> _types = new Dictionary<string, Currency>();

        public Currency Get(string name)
        {
            return _types[name];
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            foreach (var type in Types)
                _types[type.Name] = type;
        }

        protected override void OnDisable()
        {
            Save();
            base.OnDisable();
        }

        public CurrencyManager Save()
        {
            foreach (var type in Types)
                type.Save();
            PlayerPrefs.Save();
            return this;
        }

        public CurrencyManager Clear()
        {
            foreach (var type in Types)
                type.Clear();
            return this;
        }
    }
}