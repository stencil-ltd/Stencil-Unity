namespace Plugins.Networking.Local
{
    public struct LocalNetworkClient
    {
        public readonly Data.Broadcast Broadcast;

        public LocalNetworkClient(Data.Broadcast broadcast)
        {
            Broadcast = broadcast;
        }
    }
}