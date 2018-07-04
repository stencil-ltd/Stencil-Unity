using System;
using System.Collections.Generic;
using Plugins.Networking.Local.Data;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

namespace Plugins.Networking.Local
{
    public partial class Lobby
    {
        public List<LobbyClient> Players = new List<LobbyClient>();
        public event EventHandler<List<LobbyClient>> OnPlayersChanged;
        
        private void RemovePlayer(NetworkConnection arg0)
        {
            Players.RemoveAll(client => client.Connection.connectionId == arg0.connectionId);
            OnPlayersChanged?.Invoke(this, Players);
        }

        private void AddPlayer(NetworkConnection arg0)
        {
            Players.Add(new LobbyClient(LookUp(arg0), arg0));
            arg0.RegisterHandler(MsgTypeName, msg =>
            {
                Broadcast.RegisterName(arg0.connectionId, msg.ReadMessage<StringMessage>().value);
                OnPlayersChanged?.Invoke(this, Players);
            });
            OnPlayersChanged?.Invoke(this, Players);
        }
    }
}