using UnityEngine.Assertions;

using pdxpartyparrot.Core.Data.Actors.Components;
using pdxpartyparrot.Example.Data.Players;

namespace pdxpartyparrot.Example.Players
{
    public sealed class PlayerBehavior : Game.Characters.Players.PlayerBehavior
    {
        public PlayerBehaviorData GamePlayerBehaviorData => (PlayerBehaviorData)PlayerBehaviorData;

        public override void Initialize(ActorBehaviorComponentData behaviorData)
        {
            Assert.IsTrue(Owner is Player);
            Assert.IsTrue(behaviorData is PlayerBehaviorData);

            base.Initialize(behaviorData);
        }
    }
}
