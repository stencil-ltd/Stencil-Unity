namespace Plugins.Networking.Local.Data
{
    public struct LobbyClient
    {
        public readonly short PlayerId;
        public readonly Broadcast Broadcast;

        public LobbyClient(short playerId, Broadcast broadcast)
        {
            PlayerId = playerId;
            Broadcast = broadcast;
        }
    }
}