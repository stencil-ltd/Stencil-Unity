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
            _manager.EventOnServerReady.AddListener(AddPlayer);
            _manager.EventOnServerDisconnect.AddListener(RemovePlayer);
            InitLobbyClient();
        }

        private void DestroyLobby()
        {
            _manager.EventOnServerReady.RemoveListener(AddPlayer);
            _manager.EventOnServerDisconnect.RemoveListener(RemovePlayer);
        }

        public void ShutDown()
        {
            if (_discovery.running)
                _discovery.StopBroadcast();
            UnityNetworkClient?.Disconnect();
            UnityNetworkClient?.Shutdown();
            UnityNetworkClient = null;
            _manager.StopServer();
            _manager.StopClient();
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
                UnityNetworkClient?.Send(MsgTypeName, new StringMessage(Name));
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