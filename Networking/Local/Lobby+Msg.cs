using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

namespace Plugins.Networking.Local
{
    public partial class Lobby
    {
        public const short MsgTypeName = MsgType.Highest + 16;

        private void NameUpdated()
        {
            UnityNetworkClient?.Send(MsgTypeName, new StringMessage(Name));
        }
    }
}