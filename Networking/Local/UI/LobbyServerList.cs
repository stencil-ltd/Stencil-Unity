using System.Collections;
using System.Collections.Generic;
using Plugins.Networking.Local.Data;
using UnityEngine;
using Util;

namespace Plugins.Networking.Local.UI
{
    public class LobbyServerList : MonoBehaviour
    {
        public LobbyServerEntry EntryPrefab;
        public RectTransform Content;

        private Lobby _lobby;

        private void OnEnable()
        {
            _lobby = Lobby.Instance;
            _lobby.OnServersChanged += OnServersChanged;
            LobbyStates.OnChange += OnStateChanged;
        }

        private void OnDisable()
        {
            _lobby.OnServersChanged -= OnServersChanged;
            _lobby = null;
            LobbyStates.OnChange -= OnStateChanged;
        }

        private void OnStateChanged(object sender, LobbyState e)
        {
            enabled = e == LobbyState.Client;
        }

        private void OnServersChanged(object sender, List<LobbyServer> e)
        {
            Content.DestroyAllChildren();
            foreach (var result in e)
            {
                var item = Instantiate(EntryPrefab, Content, false);
                item.Server = result;
                item.Button.onClick.AddListener(() => ItemClicked(item));
            }
        }

        private void ItemClicked(LobbyServerEntry item)
        {
            Lobby.Instance.JoinServer(item.Server);
        }
    }
}