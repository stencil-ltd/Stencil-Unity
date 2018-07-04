using System.Collections.Generic;
using Plugins.Networking.Local.Data;
using UnityEngine;
using Util;

namespace Plugins.Networking.Local.UI
{
    public abstract class LobbyAbstractList<P, L> : MonoBehaviour where P : MonoBehaviour
    {
        public P EntryPrefab;
        public RectTransform Content;

        private readonly LobbyState _visibleState;
        protected Lobby Lobby { get; private set; }

        protected LobbyAbstractList(LobbyState visibleState)
        {
            _visibleState = visibleState;
        }

        private void Awake()
        {
            LobbyStates.OnChange += OnStateChanged;
        }

        private void Start()
        {
            enabled = LobbyStates.State == _visibleState;
        }

        private void OnDestroy()
        {
            LobbyStates.OnChange -= OnStateChanged;
        }

        protected virtual void OnEnable()
        {
            Lobby = Lobby.Instance;
        }

        protected virtual void OnDisable()
        {
        }

        private void OnStateChanged(object sender, LobbyState e)
        {
            enabled = e == _visibleState;
        }
    }
}