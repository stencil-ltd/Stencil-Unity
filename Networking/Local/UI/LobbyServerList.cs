﻿using System.Collections.Generic;
using Plugins.Networking.Local.Data;
using UnityEngine;
using Util;

namespace Plugins.Networking.Local.UI
{
    public class LobbyServerList : LobbyAbstractList<LobbyServerEntry, LobbyServer>
    {
        public LobbyServerList() : base(LobbyState.Client)
        {}

        protected override void OnEnable()
        {
            base.OnEnable();
            Lobby.OnServersChanged += OnServersChanged;
            OnServersChanged(null, Lobby.Servers);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Lobby.OnServersChanged -= OnServersChanged;
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