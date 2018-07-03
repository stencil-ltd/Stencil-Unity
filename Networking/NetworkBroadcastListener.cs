using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Plugins.Networking
{
    [Serializable]
    public class NetworkBroadcastChange : UnityEvent<List<Broadcast>>
    {}

    public class NetworkBroadcastListener : MonoBehaviour
    {
        public NetworkDiscovery Network;
        public float Timeout;
        public NetworkBroadcastChange OnChangeEvent;
        public event EventHandler<List<Broadcast>> OnChange;

        private List<NetworkBroadcastResult> _list 
            = new List<NetworkBroadcastResult>();
        public List<Broadcast> Broadcasts { get; private set; }
            = new List<Broadcast>();
        
        private HashSet<string> _keys = new HashSet<string>();
        
        private void Update()
        {
            CheckBroadcasts();
        }

        private void CheckBroadcasts()
        {
            var received = Network.broadcastsReceived;
            if (received == null) return;
            var list = received.Values.ToList();
            Process(list);
            if (list.SequenceEqual(_list)) return;
            _list = list;
            Broadcasts = list.Select(result => new Broadcast(result)).ToList();
            OnChange?.Invoke(this, Broadcasts);
            OnChangeEvent?.Invoke(Broadcasts);
        }

        private void Process(List<NetworkBroadcastResult> list)
        {
            var now = DateTime.Now;
            list.RemoveAll(result => (now - result.seen).TotalSeconds > Timeout);
            list.Sort();
        }
    }
}