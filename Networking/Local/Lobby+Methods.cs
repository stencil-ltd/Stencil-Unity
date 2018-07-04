using System;
using Plugins.Networking.Local.Data;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

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
            _manager.StopClient();
            UnityNetworkClient?.Shutdown();
            UnityNetworkClient = null;
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

        public void JoinServer(LobbyServer server)
        {
            NetworkManager.singleton.networkAddress = server.Broadcast.Address;
            NetworkManager.singleton.networkPort = server.Broadcast.Port;
            UnityNetworkClient = NetworkManager.singleton.StartClient();
            UnityNetworkClient?.RegisterHandler(MsgType.Connect, msg =>
            {
                Debug.Log("Connection Established. Sending ready signal");
                UnityNetworkClient?.Send(MsgType.Ready, new ReadyMessage());  
                LobbyStates.RequestState(LobbyState.ClientReady);
            });
            UnityNetworkClient?.RegisterHandler(MsgType.Error, msg =>
            {
                Debug.LogError($"Connection Error: {msg}");
                ShutDown();    
            });
        }
    }
}