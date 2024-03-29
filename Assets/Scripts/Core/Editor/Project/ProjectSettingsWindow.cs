﻿using System;

using UnityEditor;
using UnityEditor.Build;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

namespace pdxpartyparrot.Core.Editor.Project
{
    // TODO: server spectator should only be an option if networking is enabled
    public class ProjectSettingsWindow : Window.EditorWindow
    {
        private const string MainStyleSheet = "ProjectSettingsWindow/Main";
        private const string WindowLayout = "ProjectSettingsWindow/Window";

        [MenuItem("PDX Party Parrot/Project Settings...")]
        public static void ShowWindow()
        {
            ProjectSettingsWindow window = GetWindow<ProjectSettingsWindow>();
            window.Show();
        }

        public override string Title => "Project Settings";

        private EnumField _behaviorMode;

        private TextField _productName;
        private TextField _productVersion;

        private Toggle _useSpine;
        private Toggle _useDOTween;
        private Toggle _useNetworking;
        private Toggle _enableServerSpectator;
        private Toggle _useNavMesh;

        #region Unity Lifecycle

        protected override void OnDestroy()
        {
            OnSave();

            base.OnDestroy();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            rootVisualElement.styleSheets.Add(Resources.Load<StyleSheet>(MainStyleSheet));

            VisualTreeAsset mainVisualTree = Resources.Load<VisualTreeAsset>(WindowLayout);
            mainVisualTree.CloneTree(rootVisualElement);

            ProjectManifest manifest = new ProjectManifest();
            manifest.Read();

            _behaviorMode = rootVisualElement.Q<EnumField>("enum-behavior-mode");
            _behaviorMode.Init(EditorBehaviorMode.Mode3D);
            _behaviorMode.value = EditorSettings.defaultBehaviorMode;

            _productName = rootVisualElement.Q<TextField>("text-product-name");
            _productName.value = PlayerSettings.productName;

            _productVersion = rootVisualElement.Q<TextField>("text-product-version");
            _productVersion.value = PlayerSettings.bundleVersion;

            _useSpine = rootVisualElement.Q<Toggle>("toggle-feature-spine");
            _useSpine.value = manifest.UseSpine;

            _useDOTween = rootVisualElement.Q<Toggle>("toggle-feature-dotween");
            _useDOTween.value = manifest.UseDOTween;

            _useNetworking = rootVisualElement.Q<Toggle>("toggle-feature-networking");
            _useNetworking.value = manifest.UseNetworking;

            _enableServerSpectator = rootVisualElement.Q<Toggle>("toggle-feature-server-spectator");
            _enableServerSpectator.value = manifest.EnableServerSpectator;

            _useNavMesh = rootVisualElement.Q<Toggle>("toggle-feature-navmesh");
            _useNavMesh.value = manifest.UseNavMesh;
        }

        #endregion

        private void SetScriptingDefineSymbols(NamedBuildTarget buildTarget)
        {
            ScriptingDefineSymbols scriptingDefineSymbols = new ScriptingDefineSymbols(PlayerSettings.GetScriptingDefineSymbols(buildTarget));

            if(_useSpine.value) {
                scriptingDefineSymbols.AddSymbol("USE_SPINE");
            } else {
                scriptingDefineSymbols.RemoveSymbol("USE_SPINE");
            }

            if(_useDOTween.value) {
                scriptingDefineSymbols.AddSymbol("USE_DOTWEEN");
            } else {
                scriptingDefineSymbols.RemoveSymbol("USE_DOTWEEN");
            }

            if(_useNetworking.value) {
                scriptingDefineSymbols.AddSymbol("USE_NETWORKING");
            } else {
                scriptingDefineSymbols.RemoveSymbol("USE_NETWORKING");
            }

            if(_enableServerSpectator.value) {
                scriptingDefineSymbols.AddSymbol("ENABLE_SERVER_SPECTATOR");
            } else {
                scriptingDefineSymbols.RemoveSymbol("ENABLE_SERVER_SPECTATOR");
            }

            if(_useNavMesh.value) {
                scriptingDefineSymbols.AddSymbol("USE_NAVMESH");
            } else {
                scriptingDefineSymbols.RemoveSymbol("USE_NAVMESH");
            }

            PlayerSettings.SetScriptingDefineSymbols(buildTarget, scriptingDefineSymbols.ToString());
        }

        #region Events

        private void OnSave()
        {
            ProjectManifest manifest = new ProjectManifest();
            manifest.Read();

            bool refreshAssetDatabase = false;

            EditorSettings.defaultBehaviorMode = (EditorBehaviorMode)_behaviorMode.value;

            PlayerSettings.productName = _productName.value;
            PlayerSettings.bundleVersion = _productVersion.value;

            refreshAssetDatabase |= manifest.UseSpine != _useSpine.value;
            manifest.UseSpine = _useSpine.value;

            refreshAssetDatabase |= manifest.UseDOTween != _useDOTween.value;
            manifest.UseDOTween = _useDOTween.value;

            refreshAssetDatabase |= manifest.UseNetworking != _useNetworking.value;
            manifest.UseNetworking = _useNetworking.value;

            refreshAssetDatabase |= manifest.EnableServerSpectator != _enableServerSpectator.value;
            manifest.EnableServerSpectator = _enableServerSpectator.value;

            refreshAssetDatabase |= manifest.UseNavMesh != _useNavMesh.value;
            manifest.UseNavMesh = _useNavMesh.value;

            foreach(NamedBuildTarget buildTarget in Project.SupportedBuildTargets) {
                try {
                    SetScriptingDefineSymbols(buildTarget);
                } catch(Exception e) {
                    Debug.LogError($"Failed to set scripting define symbols for build target {buildTarget}: {e}");
                }
            }

            manifest.Write();

            if(refreshAssetDatabase) {
                AssetDatabase.Refresh();
            }
        }

        #endregion
    }
}
