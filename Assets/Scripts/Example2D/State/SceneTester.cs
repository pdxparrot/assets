using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Example2D.UI;

namespace pdxpartyparrot.Example2D.State
{
    public class SceneTester : Game.State.SceneTester
    {
        public override void InitViewers()
        {
            ViewerManager.Instance.AllocateViewers(1, GameManager.Instance.GameGameData.GameViewerPrefab);
            GameManager.Instance.InitViewer();

            // need this before players spawn
            GameUIManager.Instance.InitializeGameUI(GameManager.Instance.Viewer.UICamera);
        }
    }
}
