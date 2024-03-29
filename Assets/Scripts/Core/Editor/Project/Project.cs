﻿using System.IO;
using System.Text;

using UnityEditor;
using UnityEditor.Build;

using UnityEngine;

namespace pdxpartyparrot.Core.Editor.Project
{
    // TODO: this is missing UWP settings

    [InitializeOnLoad]
    public static class Project
    {
        public const int Version = 1;

        public static readonly NamedBuildTarget[] SupportedBuildTargets = {
            NamedBuildTarget.Standalone,
            NamedBuildTarget.WebGL,
            NamedBuildTarget.Server,
            NamedBuildTarget.Unknown,
        };

        private const string CscFileName = "Assets/csc.rsp";

        #region Core Assets

        private const string GitHubBaseUrl = "https://raw.githubusercontent.com/pdxparrot/assets/master";
        private const string GitHubAssetsBaseUrl = GitHubBaseUrl + "/Assets";

        #region Art

        private const string DefaultIconFileName = "pdxparrot.png";
        private const string DefaultIconPath = "Assets/Art/Core/" + DefaultIconFileName;
        private const string DefaultIconUrl = GitHubAssetsBaseUrl + "/Art/Core/" + DefaultIconFileName;

        private const string ProgressSpriteFileName = "progress.png";
        private const string ProgressSpriteUrl = GitHubAssetsBaseUrl + "/Art/Core/" + ProgressSpriteFileName;

        #endregion

        #region Audio

        private const string ButtonClickFileName = "button-click.mp3";
        private const string ButtonClickUrl = GitHubAssetsBaseUrl + "/Audio/UI/" + ButtonClickFileName;

        private const string ButtonHoverFileName = "button-hover.mp3";
        private const string ButtonHoverUrl = GitHubAssetsBaseUrl + "/Audio/UI/" + ButtonHoverFileName;

        #endregion

        #endregion

        static Project()
        {
            EditorApplication.delayCall += InitializeProject;
        }

        private static void InitializeProject()
        {
            EditorApplication.delayCall -= InitializeProject;

            ProjectManifest manifest = new ProjectManifest();

            // TODO: use a try / catch here instead of pre-checking for the file
            if(File.Exists(ProjectManifest.FileName)) {
                Debug.Log("Reading existing project manifest...");

                manifest.Read();
            } else {
                Debug.Log("Creating new project manifest...");

                // for whatever reason if we initialize on the first load of a fresh project,
                // something will come along and overwrite it all back to defaults
                // so create a v0 manifest that will get picked up on the second load
                manifest.Version = 0;
                manifest.Write();
                return;
            }

            if(UpdateFromVersion(manifest.Version)) {
                Debug.Log("Project updated, saving manifest");

                manifest.Version = Version;
                manifest.Write();
            }
        }

        private static bool UpdateFromVersion(int version)
        {
            if(version == Version) {
                Debug.Log("Project is up to date");
                return false;
            }

#if true
            if(!EditorUtility.DisplayDialog(version == 0 ? "Initialize Project" : "Update Project", version == 0 ? "Project must be initialized" : "Project must be updated", "Ok", "Cancel")) {
                Debug.LogWarning("Skipping project initialization!");
                return false;
            }
#else
            EditorUtility.DisplayDialog(version == 0 ? "Initialize Project" : "Update Project", version == 0 ? "Project must be initialized" : "Project must be updated", "Ok");
#endif

            Debug.Log(version == 0 ? $"Initializing project to {Version}..." : $"Updating project from {version} to {Version}...");

            InitializeFileSystem(version);

            if(!DownloadAssets(version)) {
                return false;
            }

            InitializeCompilerOptions(version);

            InitializePackages(version);

            InitializeProjectSettings(version);

            InitializeAssets(version);

            AssetDatabase.SaveAssets();

            return true;
        }

        #region File System

        private static void InitializeFileSystemV1()
        {
            // art content
            Util.CreateAssetFolder("Assets", "Art");
            Util.CreateAssetFolder("Assets/Art", "Core");

            // game data
            Util.CreateAssetFolder("Assets", "Data");
        }

        private static void InitializeFileSystem(int version)
        {
            if(version < 1) {
                InitializeFileSystemV1();
            }
        }

        #endregion

        #region Asset Downloads

        private static bool DownloadAssets(int version)
        {

            if(!DownloadArt(version)) {
                return false;
            }

            if(!DownloadAudio(version)) {
                return false;
            }

            return true;
        }

        #region Art

        private static bool DownloadArtV1()
        {
            if(!Util.DownloadTextureToFile(DefaultIconUrl, DefaultIconPath, TextureImporterType.Sprite)) {
                return false;
            }

            if(!Util.DownloadTextureToFile(ProgressSpriteUrl, $"Assets/Art/Core/{ProgressSpriteFileName}", TextureImporterType.Sprite)) {
                return false;
            }

            return true;
        }

        private static bool DownloadArt(int version)
        {
            if(version < 1) {
                if(!DownloadArtV1()) {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Audio

        private static bool DownloadAudioV1()
        {
            if(!Util.DownloadSFXToFile(ButtonClickUrl, $"Assets/Audio/UI/{ButtonClickFileName}")) {
                return false;
            }

            if(!Util.DownloadSFXToFile(ButtonHoverUrl, $"Assets/Audio/UI/{ButtonHoverFileName}")) {
                return false;
            }

            return true;
        }

        private static bool DownloadAudio(int version)
        {
            if(version < 1) {
                if(!DownloadAudioV1()) {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #endregion

        #region Compiler Options

        private static void InitializeCompilerOptionsV1(StringBuilder builder)
        {
            // disable warning Field 'field' is never assigned to, and will always have its default value 'value'
            builder.AppendLine("-nowarn:0649");
        }

        private static void InitializeCompilerOptions(int version)
        {
            StringBuilder builder = new StringBuilder();

            if(version < 1) {
                InitializeCompilerOptionsV1(builder);
            }

            File.WriteAllText(CscFileName, builder.ToString());
        }

        #endregion

        #region Packages

        private static void InitializePackagesV1()
        {
            Util.AddPackage("com.unity.addressables");
            Util.AddPackage("com.unity.ai.navigation");
            Util.AddPackage("com.unity.assetbundlebrowser");
            Util.AddPackage("com.unity.cinemachine");
            Util.AddPackage("com.unity.editorcoroutines");
            Util.AddPackage("com.unity.inputsystem");
            Util.AddPackage("com.unity.mathematics");
            //Util.AddPackage("com.unity.multiplayer-hlapi");
            Util.AddPackage("com.unity.multiplayer.tools");
            Util.AddPackage("com.unity.netcode.gameobjects");
            Util.AddPackage("com.unity.polybrush");
            Util.AddPackage("com.unity.postprocessing");
            Util.AddPackage("com.unity.probuilder");
            Util.AddPackage("com.unity.progrids");
            Util.AddPackage("com.unity.quicksearch");
            Util.AddPackage("com.unity.render-pipelines.core");
            Util.AddPackage("com.unity.render-pipelines.universal");
            Util.AddPackage("com.unity.scriptablebuildpipeline");
            Util.AddPackage("com.unity.searcher");
            Util.AddPackage("com.unity.settings-manager");
            Util.AddPackage("com.unity.shadergraph");
            Util.AddPackage("com.unity.textmeshpro");
            Util.AddPackage("com.unity.timeline");
            Util.AddPackage("com.unity.ugui");
            Util.AddPackage("com.unity.visualeffectgraph");
            Util.AddPackage("com.unity.visualscripting");
        }

        private static void InitializePackages(int version)
        {
            if(version < 1) {
                InitializePackagesV1();
            }
        }

        #endregion

        #region Project Settings

        private static void InitializeProjectSettings(int version)
        {
            InitializeEditorSettings(version);

            InitializePlayerSettings(version);
        }

        #region Editor Settings

        private static void InitializeEditorSettingsV1()
        {
            // visible meta files
#if UNITY_2020_1_OR_NEWER
            VersionControlSettings.mode = "Visible Meta Files";
#else
            EditorSettings.externalVersionControl = "Visible Meta Files";
#endif

            // force text asset serialization
            EditorSettings.serializationMode = SerializationMode.ForceText;

            // enable the sprite packer at build time
            EditorSettings.spritePackerMode = SpritePackerMode.BuildTimeOnlyAtlas;

            // base c# project namespace
            EditorSettings.projectGenerationRootNamespace = "pdxpartyparrot";

            // OS native line endings
            EditorSettings.lineEndingsForNewScripts = LineEndingsMode.OSNative;
        }

        private static void InitializeEditorSettings(int version)
        {
            if(version < 1) {
                InitializeEditorSettingsV1();
            }
        }

        #endregion

        #region Player Settings

        private static void SetDefaultIcon()
        {
            // TODO: how do we set the damn default icon now??
            /*Texture2D defaultIcon = AssetDatabase.LoadAssetAtPath(DefaultIconPath, typeof(Texture2D)) as Texture2D;
            PlayerSettings.SetIcons(NamedBuildTarget.Unknown, new[] { defaultIcon }, IconKind.Application);*/
            Debug.LogError("Default player icon must be set manually");
        }

        private static void EnableExclusiveInputSystem()
        {
            // no nice interface for this :(

            var projectSettingsAssets = AssetDatabase.LoadAllAssetsAtPath(Util.ProjectSettingsAssetPath);
            if(projectSettingsAssets.Length < 1) {
                Debug.LogWarning("Unable to load ProjectSettings.asset for Input System initialization");
                return;
            }

            SerializedObject projectSettings = new SerializedObject(projectSettingsAssets[0]);

#if UNITY_2020_1_OR_NEWER
            SerializedProperty activeInputHandler = projectSettings.FindProperty("activeInputHandler");
            activeInputHandler.intValue = 1;
#else
            SerializedProperty enableNativePlatformBackendsForNewInputSystem = projectSettings.FindProperty("enableNativePlatformBackendsForNewInputSystem");
            enableNativePlatformBackendsForNewInputSystem.boolValue = true;

            SerializedProperty disableOldInputManagerSupport = projectSettings.FindProperty("disableOldInputManagerSupport");
            disableOldInputManagerSupport.boolValue = true;
#endif

            projectSettings.ApplyModifiedProperties();
        }

        // TODO: break this down into platform specific methods
        private static void InitializePlayerSettingsV1()
        {
            // company name
            PlayerSettings.companyName = "PDX Party Parrot";
            // TODO: do we need to also set the bundle identifier?

            SetDefaultIcon();

            // multithreaded rendering
            PlayerSettings.SetMobileMTRendering(NamedBuildTarget.Android, true);
            PlayerSettings.SetMobileMTRendering(NamedBuildTarget.iOS, true);

            // TODO: need to enable static and dynamic batching but seems like we have to open the asset itself to do it

            // incremental GC
#if UNITY_2020_1_OR_NEWER
            PlayerSettings.gcIncremental = true;
#endif

            // script runtime
#if !UNITY_2019_3_OR_NEWER
            PlayerSettings.scriptingRuntimeVersion = ScriptingRuntimeVersion.Latest;
#endif

            // scripting backend
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone, ScriptingImplementation.IL2CPP);
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.iOS, ScriptingImplementation.IL2CPP);

            // api compatability level
            PlayerSettings.SetApiCompatibilityLevel(NamedBuildTarget.Standalone, ApiCompatibilityLevel.NET_Standard_2_0);
            PlayerSettings.SetApiCompatibilityLevel(NamedBuildTarget.Android, ApiCompatibilityLevel.NET_Standard_2_0);
            PlayerSettings.SetApiCompatibilityLevel(NamedBuildTarget.iOS, ApiCompatibilityLevel.NET_Standard_2_0);
            PlayerSettings.SetApiCompatibilityLevel(NamedBuildTarget.WebGL, ApiCompatibilityLevel.NET_Standard_2_0);

            // IL2CPP compiler config
            PlayerSettings.SetIl2CppCompilerConfiguration(NamedBuildTarget.Standalone, Il2CppCompilerConfiguration.Release);
            PlayerSettings.SetIl2CppCompilerConfiguration(NamedBuildTarget.Android, Il2CppCompilerConfiguration.Release);
            PlayerSettings.SetIl2CppCompilerConfiguration(NamedBuildTarget.iOS, Il2CppCompilerConfiguration.Release);

            EnableExclusiveInputSystem();

            // "Oreo" min Android SDK
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;

            // target all Android architectures
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.All;
        }

        private static void InitializePlayerSettings(int version)
        {
            if(version < 1) {
                InitializePlayerSettingsV1();
            }
        }

        #endregion

        #endregion

        #region Asset Creation

        private static void InitializeAssets(int version)
        {
            InitializePhysicsMaterials(version);
        }

        #region Physics

        private static void InitializePhysicsMaterialsV1()
        {
            Util.CreateAssetFolder("Assets/Data", "Physics");
            Util.CreateAssetFolder("Assets/Data/Physics", "Materials");

            // 2D frictionless material
            PhysicsMaterial2D frictionless2D = new PhysicsMaterial2D {
                friction = 0.0f,
                bounciness = 0.1f,
            };
            Util.CreateAsset(frictionless2D, "Assets/Data/Physics/Materials/frictionless.physicMaterial");

            // 3D frictionless material
            PhysicMaterial frictionless3D = new PhysicMaterial {
                staticFriction = 0.0f,
                dynamicFriction = 0.0f,
                bounciness = 0.1f,
                bounceCombine = PhysicMaterialCombine.Minimum,
                frictionCombine = PhysicMaterialCombine.Minimum
            };
            Util.CreateAsset(frictionless3D, "Assets/Data/Physics/Materials/frictionless2D.physicsMaterial2D");
        }

        private static void InitializePhysicsMaterials(int version)
        {
            if(version < 1) {
                InitializePhysicsMaterialsV1();
            }
        }

        #endregion

        #endregion
    }
}
