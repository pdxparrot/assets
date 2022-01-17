using JetBrains.Annotations;

using pdxpartyparrot.Game.UI;

namespace pdxpartyparrot.Example2D.UI
{
    public sealed class GameUIManager : GameUIManager<GameUIManager>
    {
        [CanBeNull]
        public GameUI GameGameUI => (GameUI)GameUI;
    }
}
