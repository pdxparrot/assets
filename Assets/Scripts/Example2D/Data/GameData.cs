using System;

using UnityEngine;

using pdxpartyparrot.Example2D.Camera;

namespace pdxpartyparrot.Example2D.Data
{
    [CreateAssetMenu(fileName = "GameData", menuName = "pdxpartyparrot/Example2D/Data/Game Data")]
    [Serializable]
    public sealed class GameData : Game.Data.GameData
    {
        public GameViewer GameViewerPrefab => (GameViewer)ViewerPrefab;

        #region Project Game States

        //[Header("Project Game States")]

        #endregion
    }
}
