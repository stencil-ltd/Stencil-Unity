using System;
using System.Collections;
using System.Collections.Generic;
using Plugins.Networking.Local.Data;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
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
                item.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(ItemClicked(item)));
            }
        }

        private IEnumerator ItemClicked(NetworkBroadcastItem item)
        {
            var result = item.Broadcast;
            NetworkManager.singleton.networkAddress = result.Address;
            NetworkManager.singleton.networkPort = Convert.ToInt32(result.Port);
            var client = NetworkManager.singleton.StartClient();
            yield return null;
            client.Send(MsgType.Ready, new ReadyMessage());
            OnClick?.Invoke(this, result);
        }
    }
}