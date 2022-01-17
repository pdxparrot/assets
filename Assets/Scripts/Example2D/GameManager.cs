using UnityEngine;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game;
using pdxpartyparrot.Example2D.Camera;
using pdxpartyparrot.Example2D.Data;

namespace pdxpartyparrot.Example2D
{
    public sealed class GameManager : GameManager<GameManager>
    {
        public GameData GameGameData => (GameData)GameData;

        public GameViewer Viewer { get; private set; }

        public void InitViewer()
        {
            Viewer = ViewerManager.Instance.AcquireViewer<GameViewer>();
            if(null == Viewer) {
                Debug.LogWarning("Unable to acquire game viewer!");
                return;
            }
            Viewer.Initialize(GameGameData);
        }
    }
}
