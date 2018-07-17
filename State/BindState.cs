using System;
namespace Plugins.State
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class BindState : Attribute
    {
        public readonly string Name;
        public BindState(string name = null)
        {
            Name = name;
        }
    }
}
