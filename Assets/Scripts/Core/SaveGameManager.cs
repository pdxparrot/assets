﻿using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Collections;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core
{
    // TODO: when writing to disk - https://docs.unity3d.com/Manual/JSONSerialization.html
    public class SaveGameManager : SingletonBehavior<SaveGameManager>
    {
        private const string FileNameExtension = ".partyparrot";

        [SerializeField]
        private string _saveFileName = "default";

        private string SaveFileName => $"{_saveFileName}{FileNameExtension}";

        [SerializeField]
        [ReadOnly]
        private bool _isSaving;

        public bool IsSaving => _isSaving;

        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        #region Get / Set Values

        public void SetValue(string key, object value)
        {
            _data[key] = value;
        }

        public void SetValue<T>(string key, T value)
        {
            _data[key] = value;
        }

        [CanBeNull]
        public object GetValue(string key)
        {
            return _data.GetValueOrDefault(key);
        }

        // NOTE: this isn't type checked, so it can crash if an invalid typecast is done
        [CanBeNull]
        public T GetValue<T>(string key)
        {
            return (T)_data.GetValueOrDefault(key);
        }

        #endregion

        public IEnumerator LoadSaveRoutine()
        {
            Debug.Log($"Loading save file '{SaveFileName}'...");

            // TODO: read the save file
            yield break;
        }

        public void SaveAsync()
        {
            StartCoroutine(SaveRoutine());
        }

        private IEnumerator SaveRoutine()
        {
            _isSaving = true;

            // TODO: write the save file

            _isSaving = false;
            yield break;
        }
    }
}
