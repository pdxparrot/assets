using pdxpartyparrot.Game.Players.Input;
using pdxpartyparrot.Example.Data.Players;

using UnityEngine.Assertions;

namespace pdxpartyparrot.Example.Players
{
    public sealed class PlayerInputHandler_25D : ThirdPersonPlayerInputHandler
    {
        private Player_3D GamePlayer => (Player_25D)Player;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(PlayerInputData is PlayerInputData);
            Assert.IsTrue(Player is Player_25D);
        }

        #endregion
    }
}
