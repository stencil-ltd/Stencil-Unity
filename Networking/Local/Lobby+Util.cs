using Plugins.Networking.Local.Data;

namespace Plugins.Networking.Local
{
    public partial class Lobby
    {
        private Broadcast LookUp(string addr)
        {
            return new Broadcast(_discovery.broadcastsReceived[addr]);
        }
    }
}