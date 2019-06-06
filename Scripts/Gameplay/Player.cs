using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Gameplay
{
    public static class Player
    {
        [CanBeNull] public static GameObject Instance { get; private set; }
        [CanBeNull] public static T Get<T>() where T : Component => Instance?.GetComponent<T>();

        public static event EventHandler<GameObject> OnPlayerSet;

        public static void SetPlayer([CanBeNull] GameObject player)
        {
            Debug.Log($"Changing player from {Instance} to {player}");
            Instance = player;
            OnPlayerSet?.Invoke(null, Instance);
        }

        public static void ClearPlayer([CanBeNull] GameObject player)
        {
            if (Instance != player) return;
            SetPlayer(null);
        }
    }
}