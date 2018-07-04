using System;
using System.Collections.Generic;
using Plugins.Networking.Local.Data;

namespace Plugins.Networking.Local
{
    public partial class Lobby
    {
        public List<LobbyClient> Players = new List<LobbyClient>();
        public event EventHandler<List<LobbyClient>> OnPlayersChanged;
    }
}