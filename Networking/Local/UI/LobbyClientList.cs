using System.Collections.Generic;
using Plugins.Networking.Local.Data;
using Util;

namespace Plugins.Networking.Local.UI
{
    public class LobbyClientList : LobbyAbstractList<LobbyClientEntry, LobbyClient>
    {
        public LobbyClientList() : base(LobbyState.Server)
        {}

        protected override void OnEnable()
        {
            base.OnEnable();
            Lobby.OnPlayersChanged += OnPlayersChanged;
            OnPlayersChanged(null, Lobby.Players);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Lobby.OnPlayersChanged -= OnPlayersChanged;
        }

        private void OnPlayersChanged(object sender, List<LobbyClient> e)
        {
            Content.DestroyAllChildren();
            foreach (var result in e)
            {
                var item = Instantiate(EntryPrefab, Content, false);
                item.Client = result;
                item.Button.onClick.AddListener(() => ItemClicked(item));
            }
        }

        private void ItemClicked(LobbyClientEntry item)
        {
            // TODO
        }
    }
}