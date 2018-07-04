namespace Plugins.Networking.Local.Data
{
    public struct LobbyServer
    {
        public readonly Broadcast Broadcast;

        public LobbyServer(Broadcast broadcast)
        {
            Broadcast = broadcast;
        }
    }
}