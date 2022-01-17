using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Example.UI;

namespace pdxpartyparrot.Example.State
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
