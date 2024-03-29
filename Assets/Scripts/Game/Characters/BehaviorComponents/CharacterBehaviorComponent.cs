﻿using JetBrains.Annotations;

using pdxpartyparrot.Core.World;

using UnityEngine;

namespace pdxpartyparrot.Game.Characters.BehaviorComponents
{
    public abstract class CharacterBehaviorComponent : MonoBehaviour
    {
        // TODO: if subclasses could register for specific action types (and we keep a dictionary ActionType => Listener)
        // then that would work out a lot faster and cleaner than how this is currently done

        [CanBeNull]
        protected CharacterBehavior Behavior { get; private set; }

        #region Actions

        public abstract class CharacterBehaviorAction
        {
        }

        #endregion

        #region Unity Lifecycle

        protected virtual void Awake()
        {
        }

        protected virtual void OnDestroy()
        {
        }

        #endregion

        public virtual void Initialize(CharacterBehavior behavior)
        {
            Behavior = behavior;
        }

        public virtual bool OnAnimationUpdate(float dt)
        {
            return false;
        }

        public virtual bool OnPhysicsUpdate(float dt)
        {
            return false;
        }

        #region Actions

        public virtual bool OnStarted(CharacterBehaviorAction action)
        {
            return false;
        }

        public virtual bool OnPerformed(CharacterBehaviorAction action)
        {
            return false;
        }

        public virtual bool OnCancelled(CharacterBehaviorAction action)
        {
            return false;
        }

        #endregion

        #region Events

        // NOTE: overriding this should always return false
        public virtual bool OnSpawn(SpawnPoint spawnpoint)
        {
            return false;
        }

        // NOTE: overriding this should always return false
        public virtual bool OnReSpawn(SpawnPoint spawnpoint)
        {
            return false;
        }

        // NOTE: overriding this should always return false
        public virtual bool OnDeSpawn()
        {
            return false;
        }

        #endregion
    }
}
