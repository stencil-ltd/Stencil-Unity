using Plugins.Networking.Local.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.Networking
{
    public class NetworkBroadcastItem : MonoBehaviour
    {
        public Text Title;
        
        public Broadcast Broadcast;

        public void Refresh()
        {
            Title.text = Broadcast.Name;
        }
    }
}