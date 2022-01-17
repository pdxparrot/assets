using pdxpartyparrot.Game.Players;
using pdxpartyparrot.Example.Data.Players;

namespace pdxpartyparrot.Example.Players
{
    public sealed class PlayerManager : PlayerManager<PlayerManager>
    {
        public PlayerData GamePlayerData => (PlayerData)PlayerData;
    }
}
