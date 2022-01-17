using System;

using UnityEngine;

namespace pdxpartyparrot.Example.Data.Players
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "pdxpartyparrot/Example/Data/Players/Player Data")]
    [Serializable]
    public sealed class PlayerData : Game.Data.Players.PlayerData
    {
    }
}
