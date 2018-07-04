using Plugins.Networking.Local.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.Networking.Local.UI
{
    public class LobbyClientEntry : MonoBehaviour
    {
        public Text Title;
        public Button Button;

        private LobbyClient _client;
        public LobbyClient Client
        {
            get { return _client; }
            set
            {
                _client = value;
                Refresh();   
            }
        }

        private void Refresh()
        {
            Title.text = Client.Broadcast.Name;
        }
    }
}