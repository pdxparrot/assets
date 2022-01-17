using System;

using UnityEngine;

using pdxpartyparrot.Example.Camera;

namespace pdxpartyparrot.Example.Data
{
    [CreateAssetMenu(fileName = "GameData", menuName = "pdxpartyparrot/Example/Data/Game Data")]
    [Serializable]
    public sealed class GameData : Game.Data.GameData
    {
        public GameViewer GameViewerPrefab => (GameViewer)ViewerPrefab;

        #region Project Game States

        //[Header("Project Game States")]

        #endregion
    }
}
