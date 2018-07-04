using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.Networking.Local.Data;

namespace Plugins.Networking.Local
{
    public partial class Lobby
    {
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