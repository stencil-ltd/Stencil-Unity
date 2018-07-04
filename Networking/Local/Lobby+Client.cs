using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Plugins.Networking.Local.Data;
using UnityEngine.Networking;

namespace Plugins.Networking.Local
{
    public partial class Lobby
    {
        [CanBeNull] public NetworkClient UnityNetworkClient;
        
        public List<LobbyServer> Servers = new List<LobbyServer>();
        public event EventHandler<List<LobbyServer>> OnServersChanged;

        private void InitLobbyClient()
        {
            _listener.OnChangeEvent.AddListener(OnBroadcast);
        }

        private void OnBroadcast(List<Broadcast> arg0)
        {
            Servers = arg0.Select(broadcast => new LobbyServer(broadcast)).ToList();
            OnServersChanged?.Invoke(this, Servers);
        }
    }
}