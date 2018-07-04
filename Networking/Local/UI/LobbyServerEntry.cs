using Plugins.Networking.Local.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.Networking.Local.UI
{
    public class LobbyServerEntry : MonoBehaviour
    {
        public Text Title;

        private LobbyServer _server;
        public LobbyServer Server
        {
            get { return _server; }
            set
            {
                _server = value;
                Refresh();   
            }
        }

        private void Refresh()
        {
            Title.text = Server.Broadcast.Name;
        }
    }
}