using System.Linq;
using System.Reflection;
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
    }
}