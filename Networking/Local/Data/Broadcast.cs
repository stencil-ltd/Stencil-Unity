using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Plugins.Networking.Local.Data
{
    public struct Broadcast
    {
        private static Dictionary<int, string> _names 
            = new Dictionary<int, string>();

        public static void RegisterName(int id, string name)
        {
            _names[id] = name;
        }

        public static string LookUp(int id)
        {
            string retval;
            if (!_names.TryGetValue(id, out retval))
                retval = "Unknown";
            return retval;
        }
        
        public readonly string Name;
        public readonly int Port;
        public readonly string Address;

        public Broadcast(NetworkBroadcastResult result)
        {
            string dataString = NetworkDiscovery.BytesToString(result.broadcastData);
            var items = dataString.Split('|');
            Port = Convert.ToInt32(items[2]);
            Name = items.Length == 4 ? items[3].Replace("\\|", "|") : "Unknown";
            Address = result.serverAddress;
        }

        public Broadcast(NetworkConnection conn)
        {
            Name = LookUp(conn.connectionId);
            Port = -1;
            Address = conn.address;
        }
    }
}