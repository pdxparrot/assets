using System;

using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters.Players;
using pdxpartyparrot.Example.Camera;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Example.Players
{
    public sealed class Player_3D : Player3D
    {
        public PlayerBehavior GamePlayerBehavior => (PlayerBehavior)PlayerBehavior;

        private GameViewer PlayerGameViewer => (GameViewer)Viewer;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(PlayerInputHandler is PlayerInputHandler);

            Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        #endregion

        public override void Initialize(Guid id)
        {
            base.Initialize(id);

            Assert.IsTrue(PlayerBehavior is PlayerBehavior);
        }

        protected override bool InitializeLocalPlayer(Guid id)
        {
            if(!base.InitializeLocalPlayer(id)) {
                return false;
            }

            PlayerViewer = GameManager.Instance.Viewer;

            return true;
        }

        #region Spawn

        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            PlayerGameViewer.FollowTarget(GamePlayerBehavior.LookTarget);

            return true;
        }

        public override void OnDeSpawn()
        {
            PlayerGameViewer.FollowTarget(null);

            base.OnDeSpawn();
        }

        #endregion
    }
}
