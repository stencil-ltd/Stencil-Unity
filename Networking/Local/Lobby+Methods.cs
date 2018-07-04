using System;
using System.Collections.Generic;
using UnityEngine;

namespace Plugins.Networking.Local
{
    public partial class Lobby
    {
        private void InitLobby()
        {
            
        }

        private void DestroyLobby()
        {
            
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