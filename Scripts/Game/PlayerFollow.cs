using System;
using UI;

namespace Game
{
    public class PlayerFollow : Controller<PlayerFollow>
    {
        [Serializable]
        public enum UpdateStage
        {
            Update, FixedUpdate, LateUpdate
        }

        public UpdateStage Stage = UpdateStage.LateUpdate;

        private void Update()
        {
            if (Stage == UpdateStage.Update)
                Follow();
        }

        private void FixedUpdate()
        {
            if (Stage == UpdateStage.FixedUpdate)
                Follow();
        }

        private void LateUpdate()
        {
            if (Stage == UpdateStage.LateUpdate)
                Follow();
        }

        private void Follow()
        {
            var player = Player.Instance;
            if (player != null)
                transform.position = player.transform.position;
        }
    }
}