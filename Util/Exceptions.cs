using System;
using System.Linq;
using UnityEngine;

namespace Plugins.Util
{
    public static class Exceptions
    {
        public static string[] AggregateMessages(this AggregateException ex)
        {
            return ex.InnerExceptions.Select(it => it.Message).ToArray();
        }

        public static void LogMessages(this AggregateException ex)
        {
            foreach (var msg in ex.AggregateMessages())
                Debug.LogError(msg);
        }
        
    }
}