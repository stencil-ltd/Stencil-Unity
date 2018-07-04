using System;
using Plugins.Networking.Local.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace Plugins.Networking.Local
{
    public partial class Lobby
    {
        private void InitLobby()
        {
            _manager.EventOnServerAddPlayer.AddListener(AddPlayer);
            _manager.EventOnServerRemovePlayer.AddListener(RemovePlayer);
            InitLobbyClient();
        }

        private void DestroyLobby()
        {
            _manager.EventOnServerAddPlayer.RemoveListener(AddPlayer);
            _manager.EventOnServerRemovePlayer.RemoveListener(RemovePlayer);
        }

        private void RemovePlayer(NetworkConnection arg0, PlayerController arg1)
        {
            Players.RemoveAll(client => client.Broadcast.Address.Equals(arg0.address));
            OnPlayersChanged?.Invoke(this, Players);
        }

        private void AddPlayer(NetworkConnection arg0, short arg1)
        {
            Players.Add(new LobbyClient(arg1, LookUp(arg0.address)));
            OnPlayersChanged?.Invoke(this, Players);
        }

        public void ShutDown()
        {
            if (_discovery.running)
                _discovery.StopBroadcast();
            _manager.StopServer();
            LobbyStates.RequestState(LobbyState.None);
        }

        public void StartServer()
        {
            ShutDown();
            _discovery.Initialize();
            _discovery.StartAsServer();
            if (!_manager.StartServer())
                Debug.LogError("Something went wrong.");
            else LobbyStates.RequestState(LobbyState.Server);
        }

        public void StartClient()
        {
            ShutDown();
            _discovery.Initialize();
            _discovery.StartAsClient();
            LobbyStates.RequestState(LobbyState.Client);
        }
    }
}