using System;

using UnityEngine;

namespace pdxpartyparrot.Example.Data.Players
{
    [CreateAssetMenu(fileName = "PlayerBehaviorData", menuName = "pdxpartyparrot/Example/Data/Players/PlayerBehavior Data")]
    [Serializable]
    public sealed class PlayerBehaviorData : Game.Data.Characters.PlayerBehaviorData
    {
    }
}
