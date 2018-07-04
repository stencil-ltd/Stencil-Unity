using UnityEngine.Networking;

namespace Plugins.Networking.Local.Data
{
    public struct LobbyClient
    {
        public readonly Broadcast Broadcast;
        public readonly NetworkConnection Connection;
        public string Name => Broadcast.LookUp(Connection.connectionId);

        public LobbyClient(Broadcast broadcast, NetworkConnection conn)
        {
            Broadcast = broadcast;
            Connection = conn;
        }
    }
}