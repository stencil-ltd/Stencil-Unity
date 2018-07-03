using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Util;

namespace Plugins.Networking
{
    public class NetworkBroadcastList : MonoBehaviour
    {
        public NetworkBroadcastItem ItemPrefab;
        public RectTransform Content;

        public event EventHandler<Broadcast> OnClick;
        
        public void Event_UpdateBroadcasts(List<Broadcast> broadcastResults)
        {
            Content.DestroyAllChildren();
            foreach (var result in broadcastResults)
            {
                var item = Instantiate(ItemPrefab, Content, false);
                item.Broadcast = result;
                item.Refresh();
                item.GetComponent<Button>().onClick.AddListener(() => ItemClicked(item));
            }
        }

        private void ItemClicked(NetworkBroadcastItem item)
        {
            var result = item.Broadcast;
            NetworkManager.singleton.networkAddress = result.Address;
            NetworkManager.singleton.networkPort = Convert.ToInt32(result.Port);
            NetworkManager.singleton.StartClient();
            OnClick?.Invoke(this, result);
        }
    }
}