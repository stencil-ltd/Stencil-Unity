using Binding;
using UnityEngine;

namespace Plugins.Networking.Local
{
    [AddComponentMenu("Network/Lobby")]
    [RequireComponent(
        typeof(EventNetworkManager), 
        typeof(NetworkDiscovery),
        typeof(NetworkBroadcastListener))]
    
    public partial class Lobby : MonoBehaviour
    {
        public static Lobby Instance { get; private set; }
        
        [Bind] private EventNetworkManager _manager;
        [Bind] private NetworkDiscovery _discovery;
        [Bind] private NetworkBroadcastListener _listener;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            this.Bind();
            _listener.Network = _discovery;
            DontDestroyOnLoad(gameObject);
            InitLobby();
            LobbyStates.OnChange += StateChange;
        }

        private void OnDestroy()
        {
            DestroyLobby();
            LobbyStates.OnChange -= StateChange;
            Instance = null;
        }

        private void StateChange(object sender, LobbyState e)
        {
            
        }
    }
}