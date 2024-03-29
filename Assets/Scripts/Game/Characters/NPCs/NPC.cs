﻿using System;

using pdxpartyparrot.Core.Actors.Components;

using UnityEngine;

namespace pdxpartyparrot.Game.Characters.NPCs
{
    public interface INPC
    {
        GameObject GameObject { get; }

        Guid Id { get; }

        bool IsLocalActor { get; }

        ActorBehaviorComponent Behavior { get; }

        ActorMovementComponent Movement { get; }

        NPCBehavior NPCBehavior { get; }

        bool IsMoving { get; }

        #region Actor

        void DeSpawn(bool destroy);

        #endregion

        #region Pathing

        bool HasPath { get; }

        Vector3 NextPosition { get; }

        Vector3 MoveDirection { get; }

        bool UpdatePath(Vector3 target, float range);

        void ResetPath(bool idle);

        #endregion

        void Stop(bool resetPath, bool idle);

        void Recycle();

        void OnBehaviorInitialized();
    }
}
