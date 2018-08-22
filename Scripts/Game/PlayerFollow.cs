using UI;
using UnityEngine;

namespace Game
{
    public class PlayerFollow : Controller<PlayerFollow>
    {
        private void LateUpdate()
        {
            var player = Player.Instance;
            if (player != null)
                transform.position = player.transform.position;
        }
    }
}