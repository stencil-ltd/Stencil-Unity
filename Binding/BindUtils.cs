﻿using System;
using System.Linq;
using System.Reflection;
using Plugins.State;
using UnityEngine;

namespace Binding
{
    public static class BindUtils
    {
        public static void Bind(this MonoBehaviour obj)
        {
            var attr = typeof(Bind);
            var type = obj.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(field => field.IsDefined(attr, true));
            foreach (var field in fields)
                field.SetValue(obj, obj.GetComponent(field.FieldType));
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(prop => prop.IsDefined(attr, true));
            foreach (var prop in props)
                prop.SetValue(obj, obj.GetComponent(prop.PropertyType));
        }

        public static void BindStates(this MonoBehaviour obj)
        {
            var attr = typeof(BindState);
            var type = obj.GetType();
            var states = Resources.FindObjectsOfTypeAll<State>();

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(field => field.IsDefined(attr, true));
            foreach (var field in fields)
            {
                var bstate = field.GetCustomAttribute<BindState>();
                field.SetValue(obj, Find(bstate.Name ?? field.Name, states));
            }

            var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(prop => prop.IsDefined(attr, true));
            foreach (var prop in props)
            {
                var bstate = prop.GetCustomAttribute<BindState>();
                prop.SetValue(obj, Find(bstate.Name ?? prop.Name, states));
            }
        }

        private static State Find(string name, State[] states)
        {
            var find = states.FirstOrDefault((arg) => arg.Name.Replace(" ", "") == name);
            if (find == null)
                throw new Exception($"Could not find state {name}");
            return find;
        }
    }
}