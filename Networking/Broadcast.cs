namespace Plugins.Networking
{
    public struct Broadcast
    {
        public readonly string Name;
        public readonly string Creator;
        public readonly string Host;
        public readonly string Port;
        public readonly string Address;

        public Broadcast(NetworkBroadcastResult result)
        {
            string dataString = NetworkDiscovery.BytesToString(result.broadcastData);
            var items = dataString.Split(':');
            Creator = items[0];
            Host = items[1];
            Port = items[2];
            Name = items.Length == 4 ? items[3].Replace("\\:", ":") : "Unknown";
            Address = result.serverAddress;
        }
    }
}