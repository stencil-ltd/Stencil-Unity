using Plugins.Networking.Local.Data;
using UnityEngine.Networking;

namespace Plugins.Networking.Local
{
    public partial class Lobby
    {
        private Broadcast LookUp(NetworkConnection conn)
        {
            return new Broadcast(conn);
        }
    }
}