﻿#pragma warning disable 0618    // disable obsolete warning for now

using System;

using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Characters.Players;
using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.Assertions;

#if USE_NETWORKING
using Unity.Netcode;
using Unity.Netcode.Components;
#endif

namespace pdxpartyparrot.Game.Players
{
#if USE_NETWORKING
    [RequireComponent(typeof(NetworkAnimator))]
#endif
    public class NetworkPlayer : NetworkActor
    {
        public IPlayer Player => (IPlayer)Actor;

        [SerializeField]
        [ReadOnly]
        private ulong _clientId;

        public ulong ClientId => _clientId;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Actor is IPlayer);
        }

        #endregion

        public void Initialize(ulong clientId)
        {
            _clientId = clientId;
        }

        public void Init2D()
        {
#if USE_NETWORKING
            NetworkTransform.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody2D;
            NetworkTransform.syncRotationAxis = NetworkTransform.AxisSyncMode.AxisZ;
#endif
        }

        public void Init3D()
        {
#if USE_NETWORKING
            NetworkTransform.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
            NetworkTransform.syncRotationAxis = NetworkTransform.AxisSyncMode.AxisY;
#endif
        }

        // TODO: we could make better use of NetworkBehaviour callbacks in here (and in other NetworkBehaviour types)

        #region Callbacks

#if USE_NETWORKING
        [ClientRpc]
#endif
        public virtual void SpawnRpc(string id)
        {
            Debug.Log($"Network player {id} spawn");

            // TODO: these are already called locally ... but this causes them to be called again locally :(
            /*Actor.Initialize(Guid.Parse(id));
            Actor.Behavior.Initialize(GameStateManager.Instance.PlayerManager.PlayerBehaviorData);*/
        }

        #endregion
    }
}
