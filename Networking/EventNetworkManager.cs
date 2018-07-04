using System;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Plugins.Networking
{
    [Serializable]
    public class NetworkConnectionEvent : UnityEvent<NetworkConnection> {}
    
    [Serializable]
    public class NetworkClientEvent : UnityEvent<NetworkClient> {}
    
    [Serializable]
    public class NetworkConnectionError : UnityEvent<NetworkConnection, int> {}
    
    [Serializable]
    public class ServerAddPlayerEvent : UnityEvent<NetworkConnection, short> {}
    
    [Serializable]
    public class ServerRemovePlayerEvent : UnityEvent<NetworkConnection, PlayerController> {}

    public class EventNetworkManager : NetworkManager
    {
        public NetworkConnectionEvent EventOnServerConnect;
        public override void OnServerConnect(NetworkConnection conn)
        {
            base.OnServerConnect(conn);
            EventOnServerConnect?.Invoke(conn);
        }
        
        public NetworkConnectionEvent EventOnServerDisconnect;
        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);
            EventOnServerDisconnect?.Invoke(conn);
        }
        
        public NetworkConnectionEvent EventOnServerReady;
        public override void OnServerReady(NetworkConnection conn)
        {
            base.OnServerReady(conn);
            EventOnServerReady?.Invoke(conn);
        }

        public NetworkConnectionEvent EventOnClientConnect;
        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            EventOnClientConnect?.Invoke(conn);
        }

        public NetworkConnectionEvent EventOnClientDisconnect;
        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);
            EventOnClientDisconnect?.Invoke(conn);
        }

        public UnityEvent EventOnStartHost;
        public override void OnStartHost()
        {
            base.OnStartHost();
            EventOnStartHost?.Invoke();
        }

        public UnityEvent EventOnStartServer;
        public override void OnStartServer()
        {
            base.OnStartServer();
            EventOnStartServer?.Invoke();
        }

        public NetworkClientEvent EventOnStartClient;
        public override void OnStartClient(NetworkClient client)
        {
            base.OnStartClient(client);
            EventOnStartClient?.Invoke(client);
        }

        public NetworkConnectionError EventOnClientError;
        public override void OnClientError(NetworkConnection conn, int errorCode)
        {
            base.OnClientError(conn, errorCode);
            EventOnClientError?.Invoke(conn, errorCode);
        }

        public ServerAddPlayerEvent EventOnServerAddPlayer;
        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            base.OnServerAddPlayer(conn, playerControllerId);
            EventOnServerAddPlayer?.Invoke(conn, playerControllerId);
        }

        public ServerRemovePlayerEvent EventOnServerRemovePlayer;
        public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
        {
            base.OnServerRemovePlayer(conn, player);
            EventOnServerRemovePlayer?.Invoke(conn, player);
        }
    }
}