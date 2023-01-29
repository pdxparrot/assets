# TODO

* Discord integration
  * https://discord.com/developers/docs/game-sdk/sdk-starter-guide
* Create prefabs through code
  * https://docs.unity3d.com/ScriptReference/PrefabUtility.html
* UI Builder
  * https://docs.unity3d.com/Packages/com.unity.ui.builder@1.0/manual/index.html
* Tear these apart for ideas
  * https://www.youtube.com/watch?v=qsU4nM0L_n0
    * https://learn.unity.com/project/3d-game-kit
    * https://learn.unity.com/project/2d-game-kit
* Netcode for GameObjects for networking
  * https://docs-multiplayer.unity3d.com/
* Fix assets.csproj to enable OmniSharp on this project
* Show a progress bar when initializing a project
* Add a button to the project settings window to run initialization
* Use the settings manager package
  * https://docs.unity3d.com/Packages/com.unity.settings-manager@1.0/manual/index.html
* Use addressables
  * https://docs.unity3d.com/Packages/com.unity.addressables@0.4/manual/index.html
* Add feature selection for things like 2D vs 3D, XR, Mobile, ECS, etc
  * This would add and remove the required package bundles as necessary
* Add features for ENABLE_VR and ENABLE_GVR
* Need to create InputSystem settings asset
* Need to import TextMesh Pro Essentials
* Create Data/Animation/empty.controller Animation Controller in project setup
  * Add InputX and InputZ float parameters
* Create Data/Audio/main.mixer Mixer in project setup
* Create the EventSystem prefab at project setup ?
* Setup a generic "alert" message box

# Engine Update

* **TODO:** Copy the last game jam's engine code back to the common GitHub repo
  * **TODO:** Would all of this make more sense as forks of a common engine base repo?
* **TODO:** Copy any changes to editorconfig, etc back to the common GitHub repo
* **TODO:** Update 2D and 3D example projects as necessary

# Project Creation

* Create a new Unity project
  * Close the project and do not save it
* Create the GitHub repo for the project
* Copy gitignore.project from common GitHub repo to .gitignore
  * https://raw.githubusercontent.com/pdxparrot/assets/master/.gitignore
* Copy LICENSE from common GitHub repo
  * https://raw.githubusercontent.com/pdxparrot/assets/master/LICENSE
* Copy .editorconfig from common GitHub repo
  * https://raw.githubusercontent.com/pdxparrot/assets/master/.editorconfig
* Create README.md
* Delete the Assets/Scenes directory and its .meta file
* git init, add, commit, and push

# Pre-Setup

* Copy engine setup from common GitHub repo
  * https://raw.githubusercontent.com/pdxparrot/assets/master/Assets/Scripts/Core/Network/UnityWebRequesetExtensions.cs
    * **TODO:** this requirement sucks, get rid of it (just copy paste the extension method contents)
  * https://raw.githubusercontent.com/pdxparrot/assets/master/Assets/Scripts/Core/Editor/Project/
  * https://raw.githubusercontent.com/pdxparrot/assets/master/Assets/Scripts/Core/Editor/Window/
  * https://raw.githubusercontent.com/pdxparrot/assets/master/Assets/Scripts/Core/Editor/Util.cs
  * https://raw.githubusercontent.com/pdxparrot/assets/master/Assets/Scripts/Core/Editor/ScriptingDefineSymbols.cs
  * **TODO:** simplify / script this
* Copy engine editor resources from common GitHubu repo
  * https://raw.githubusercontent.com/pdxparrot/assets/master/Assets/Editor
  * **TODO:** simplify / script this
* Open and close the project once for the build process to setup
  * **TODO:** this shouldn't be necessary but even when it isn't it still is to avoid something going weird
* Open the new Unity Project and the project should automatically initialize
  * This process can take a while and currently is not very responsive
  * Say **No** to enabling the new Input System backend (initializing will set this up instead)
    * This seems to have gone away in 2022
* ~~Add Keijiro Kino~~ **TODO:** this required the HDRP so we probably don't want to always use it
  * https://github.com/keijiro/Kino
    * Add registry to scopedRegistries in Packages/manifest.json
    * Add "jp.keijiro.kino.post-processing": "2.1.15" to dependencies in Packages/manifest.json
* Update any out of date packages
* Copy the rest of the core engine scripts
  * https://raw.githubusercontent.com/pdxparrot/assets/master/Assets/Scripts/Core
  * Do **not** copy the core game scripts yet
  * **TODO:** simplify / script this
* Create the Assembly Definitions
  * Assets/Scripts/Core/com.pdxpartyparrot.Core.asmdef
    * References: Unity.InputSystem, com.unity.cinemachine, Unity.Postprocessing.Runtime, Unity.TextMeshPro, Unity.VisualScripting.Core, Unity.VisualScripting.Flow, ~~Kino.Postprocessing~~
    * Uncheck Auto Referenced
    * Add com.unity.netcode.runtime and com.unity.netcode.components if using networking
  * Assets/Scripts/Core/Editor/com.pdxpartyparrot.Core.Editor.asmdef
    * Editor platform only
    * References: com.pdxpartyparrot.Core.asmdef, com.unity.cinemachine, Unity.TextMeshPro
    * Uncheck Auto Referenced
  * **TODO:** simplify this
* Clean up TODOs as necessary
* Remove any FormerlySerializedAs attributes
* Add the Core assembly to the Visual Scripting Node Library (Project Settings)

# Project Settings

* Graphics Settings
  * Set the Render Pipeline Asset if desired (not usually needed)
    * This will require creating the asset first, which itself may be configured as desired
* Input System Package
  * Create the Input System Settings asset if not already done
    * Process events in Fixed Update
* Netcode for GameObjects
  * Check Auto-Add Network Objects
* Package Manager
  * Enable Pre-release Packages
  * Show Dependencies
    * This seems to be gone in 2022
* Tags and Layers
  * Add the following layers if they don't exist:
    * PostProcessing
    * NoPhysics
    * Vfx
    * Viewer
    * Player
    * NPC
    * World
    * Weather
* Physics Settings
  * Only enable the minimum necessary collisions
    * **TODO:** Water?
    * Vfx -> Vfx
    * Viewer -> Weather, World
    * Player -> Weather, World, NPC (and Player if that's the desired behavior)
    * NPC -> Weather, World (and NPC if that's the desired behavior)
    * World -> Weather
* Physics 2D Settings
  * Only enable the minimum necessary collisions
    * **TODO:** Water?
    * Vfx -> Vfx
    * Viewer -> Weather, World
    * Player -> Weather, World, NPC (and Player if that's the desired behavior)
    * NPC -> Weather, World (and NPC if that's the desired behavior)
    * World -> Weather
* Player Settings
  * Default icon needs to be set here for now
  * Set any desired Splash Images/Logos
  * Color Space: Linear (or Gamma if targeting old mobile/console platforms)
    * Fix up any Grahics API issues that this might cause (generally this means disabling Auto Graphics APIs on certain platforms)
      * This seems to be unnecessary in 2022
  * Enable Static and Dynamic Batching if they aren't already
  * Set Dedicated Server to use IL2CPP if desired
  * Verify that the Bundle Identifer is set correctly
    * com.pdxpartyparrot.{project}
  * Ensure that Graphics Jobs are enabled
    * WebGL often has this disabled by default
  * WebGL builds should be Gzip compressed
* TextMesh Pro
  * Import TMP Essentials if not already done
  * Optionally import TMP Examples & Extras if desired
* Visual Scripting
  * Generate Custom Inspector Properties

# Packages

* Add preview packages
  * Android Logcat if desired
  * HD Render Pipeline if desired
  * Burst/Jobs (if using ECS)
    * Unity Netcode if doing multiplayer

# Asset Store Assets

* DOTween (not Pro)
  * Make sure to run Setup DOTween
  * Make sure to run Create ASMDEF
  * Make sure to enable DOTween in the PDX Party Parrot Project Settings
* If using Spine, download the latest Spine-Unity package (currently 3.8+) and import it
  * Assets/Spine* must be added to the .gitignore to prevent committing this
    * **TODO:** this should already be done in the common .gitignore
  * The Assembly Definition will need to be force added to source control
    * If the Assembly Definition does not exist, your version is too old!
  * Make sure to enable Spine in the PDX Party Parrot Project Settings
* **TODO:** If using NavMesh, ...
  * Add com.unity.ai.navigation package
    * Project should add this but if it fails it needs to be added by name until it stops being hidden
      * This seems to be fixed in 2022
  * Add Unity.AI.Navigation to asmdefs
  * Enable NavMesh in the PDX Party Parrot Project Settings

# Engine Source

## Game Scripts

* Copy engine game scripts
  * https://raw.githubusercontent.com/pdxparrot/assets/master/Assets/Scripts/Game
  * **TODO:** simplify / script this
* Create the Assembly Definitions
  * Scripts/Game/com.pdxpartyparrot.Game.asmdef
    * References: com.pdxpartyparrot.Core.asmdef, Unity.InputSystem, com.unity.cinemachine, Unity.TextMeshPro, Unity.VisualScripting.Core, Unity.VisualScripting.Flow
    * Uncheck Auto Referenced
    * Add com.unity.netcode.runtime and com.unity.netcode.components if using networking
  * Scripts/Game/Editor/com.pdxpartyparrot.Game.Editor.asmdef
    * Editor platform only
    * References: com.pdxpartyparrot.Core.asmdef, com.pdxpartyparrot.Core.Editor.asmdef, com.pdxpartyparrot.Game.asmdef
    * Uncheck Auto Referenced
* Clean up TODOs as necessary
* Remove any FormerlySerializedAs attributes
* Add the Game assembly to the Visual Scripting Node Library (Project Settings)

## TODO: working on Example project that can just be copied over

* Copy example game scripts
  * https://raw.githubusercontent.com/pdxparrot/assets/master/Assets/Scripts/Example
  * **TODO:** simplify / script this
  * **TODO:** remove any steps later on that require creation of the files this brings in
* Update namespaces (`pdxpartyparrot.Example`) in code to match project name (`pdxpartyparrot.{project}`)
* Update paths (`pdxpartyparrot/Example/`) in code to match project name (`pdxpartyparrot/{project}/`)
* Update to use the desired GameViewer type
* Update to use the desired Player / PlayerInputHandler types

## Initial Project Scripts

* Create the Assembly Definitions
  * Scripts/{project}/com.pdxpartyparrot.{project}.asmdef
    * References: com.pdxpartyparrot.Core.asmdef, com.pdxpartyparrot.Game.asmdef
    * Reference Unity.InputSystem, Unity.TextMeshPro, Unity.VisualScripting.*, etc as required
    * Uncheck Auto Referenced
* Add the project assembly to the Visual Scripting Node Library (Project Settings)

## Set Script Execution Order

* Unity EventSystem
* TextMeshPro
* InputSystem PlayerInput
* pdxpartyparrot.{project}.Loading.LoadingManager
* pdxpartyparrot.Core.Time.TimeManager
* pdxpartyparrot.Game.State.GameStateManager
* Cinemachine PixelPerfect
* Default Time
* Unity ToggleGroup
* Cinemachine Brain / ImpulseListener
* pdxpartyparrot.Core.Debug.DebugMenuManager
  * This must be run last

# Engine Asset Setup

* **TODO:** can any of this be automated? maybe creating the directory structure at least?
* Create Data/Animation/empty.controller Animator Controller
* Create Data/Audio/main.mixer Mixer
  * 3 Master child groups
    * Music
      * Expose the Volume parameter and set it to -5db
        * Rename it to MusicVolume
    * SFX
      * Expose the Volume parameter and set it to 0db
        * Rename it to SFXVolume
    * Dialogue
      * Expose the Volume parameter and set it to 5db
        * Rename it to DialogueVolume
    * Ambient
      * Expose the Volume parameter and set it to -10db
        * Rename it to AmbientVolume
  * Expose the Master Volume parameter and set it to 0db
    * Rename it to MasterVolume
  * Add a Lowpass effect to the Master group, after the Attenuation effect
  * Rename the default Snapshot to Unpaused
  * Create a new Snapshot and name it to Paused
    * Set the Lowpass filter cutoff to 350Hz
* Data/Prefabs/Input/EventSystem.prefab
  * Create using default EventSystem that gets added automatically when adding a UI object
  * Replace Standalone Input Module with InputSystemUIInputModule
  * Add EventSystemHelper script to this
  * Copy the DefaultInputActions asset (linked to the Actions Asset by default) to Assets/Data/Input
    * Rename the asset to Player.inputactions avoid confusion with the default
    * Replace the EventSystem Actions Asset with this copy
    * UI actions may need to be reset to Pass Through to fix warnings
    * Pause action usually has to be added to this
      * Action Type: Button
      * Start [Gamepad] (Gamepad scheme)
      * Escape [Keyboard] (Keyboard & Mouse scheme)
    * Adding an Invert Vector2 Processor to the Look action can allow for inverting the look axes
      * **TODO:** There doesn't appear to be a good way to modify this from code yet
    * Unused actions may be removed

## Server Spectator (Multiplayer Only)

* Enable Server Spectator in the project settings
* Duplicate the Player input actions asset as ServerSpectator.inputactions
  * Rename the asset to ServerSpectator.inputactions
  * Setup the action maps as necessary for moving and looking
* Create an empty Prefab and add the ServerSpectator script to it
  * Layer: Default
  * Add a new empty GameObject under the ServerSpectator Prefab (Input) and add the ServerSpectatorInputHandler component to it
    * Attach the ServerSpectatorInputHandler to the ServerSpectator component
    * Attach the ServerSpectator input actions asset to the Unity PlayerInput
    * Default Map should be set to Player
    * Change Behavior to Invoke Unity Events
    * Hook up the Move and Look actions
* Attach the ServerSpectator prefab to the GameStateManager
* Create an empty Prefab and add the ServerSpectatorViewer script to it
  * Layer: Viewer
  * Add a camera under the prefab (Camera)
    * Clear Flags: Solid Color (or Skybox for a skybox)
    * Background: Opaque Black (or Default for a skybox)
    * Remove the Audio Listener
    * Add CinemachineBrain to Camera
  * Attach the Camera to the ServerSpectatorViewer component
  * Add another camera under the prefab (UI Camera)
    * Clear Flags: Solid Color
    * Background: Opaque Black
    * Remove the AudioListener
    * Add the UICameraAspectRatio component to the UI Camera
  * Attach the UI Camera to the ServerSpectatorViewer component
  * Set the CinemachineVirtualCamera settings as necessary
    * Set the Body to 3rd Person Follow
  * Configure any additional settings as required
* Attach the ServerSpectatorViewer prefab to the GameStateManager

## Engine Managers

* Managers go in Data/Prefabs/Managers
* LoadingManager
  * Create an empty Prefab and add the project LoadingManager component to it
  * Create a LoadingTipData in Data/Data and attach it to the manager
* ActorManager
  * Create an empty Prefab and add the ActorManager component to it
* AudioManager
  * Create an empty Prefab and add the AudioManager component to it
  * Create an AudioData in Data/Data and attach it to the manager
    * Attach the main mixer to the data
    * Ensure all of the Parameters look correct
  * Add 6 Audio Sources to the manager prefab
    * Ensure Spatial Blend is set to 0 (2D)
    * Disable Play on Awake
  * Attach each audio source to an audio source on the AudioManager component
* CinematicsManager
  * Create an empty Prefab and add the CinematicsManager component to it
* DebugMenuManager
  * Create an empty Prefab and add the DebugMenuManager component to it
* DialogueManager
  * Create an empty Prefab and add the DialogueManager component to it
  * Create a DialogueData in Data/Data and attach it to the manager
* EffectsManager
  * Create an empty Prefab and add the EffectsManager component to it
* EngineManager
  * Create an empty Prefab and add the PartyParrotManager component to it
  * Create an EngineData in Data/Data and attach it to the manager
  * Attach the frictionless physics materials
    * **TODO:** Why are these attaching backwards?
* GameStateManager
  * Create an empty Prefab and add the GameStateManager component to it
  * Create an empty Prefab in Data/Prefabs/State and add the MainMenuState component to it
    * Set the Initial Scene Name to main_menu
    * Set the MainMenuState as the Main Menu State Prefab in the GameStateManager
  * Create an empty Prefab in Data/Prefabs/State and add the NetworkConnectState component to it
    * Set the NetworkConnectState as the Network Connect State Prefab in the GameStateManager
  * Create an empty Prefab in Data/Prefabs/State and add the project SceneTester component to it
    * Check Make Initial Scene Active
    * Set the SceneTester as the Scene Tester Prefab in the GameStateManager
* InputManager
  * Create an empty Prefab and add the InputManager component to it
  * Create an InputData in Data/Data and attach it to the manager
  * Attach the EventSystem prefab
* LocalizationManager
  * Create an empty Prefab and add the LocalizationManager component to it
  * Create a LocalizationData in Data/Data and attach it to the manager
* NetworkManager
  * Create an empty Prefab and add the (not Unity / MLAPI) NetworkManager component to it
  * Configure Netcode for GameObjects settings if networking is enabled
    * Uncheck Don't Destroy
    * Run In Background should be checked
    * Set the Transport to UnetTransport
    * **TODO:** Can this not add every scene to its list of networked scenes?
* ObjectPoolManager
  * Create an empty Prefab and add the ObjectPoolManager component to it
* SaveGameManager
  * Create an empty Prefab and add the SaveGameManager component to it
  * Set the Save File Name to {project}
* SceneManager
  * Create an empty Prefab and add the SceneManager component to it
* SpawnManager
  * Create an empty Prefab and add the SpawnManager component to it
  * Create a SpawnData in Data/Data and attach it to the manager
    * Add a 'player' spawn tag
* TimeManager
  * Create an empty Prefab and add the TimeManager component to it
* UIManager
  * Create an empty Prefab and add the UIManager component to it
  * Create a UIData in Data/Data and attach it to the manager
    * Set the UI layer to UI
    * Attach a TMP_Font Asset to the Default font
      * LiberationSans SDF is currently the default TMP font
    * Create empty Prefabs/UI/DefaultButtonHoverEffect and add the EffectTrigger component to it
      * Add an AudioEffectTriggerComponent and add it to the EffectTrigger components
        * Set the Audio Clip to button-hover
      * Attach to the UIData
    * Create empty Prefabs/UI/DefaultButtonSubmitEffect and add the EffectTrigger component to it
      * Add an AudioEffectTriggerComponent and add it to the EffectTrigger components
        * Set the Audio Clip to button-click
      * Attach to the UIData
    * Create empty Prefabs/UI/DefaultButtonBackEffect and add the EffectTrigger component to it
      * Add an AudioEffectTriggerComponent and add it to the EffectTrigger components
        * Set the Audio Clip to button-click
      * Attach to the UIData
* ViewerManager
  * Create an empty Prefab and add the ViewerManager component to it

## GameManager

* Create a new GameData script that overrides the Game GameData and implement the required interface
  * Add the CreateAssetMenu and Serializable attributes
* Create a new GameManager script that overrides the Game GameManager and implement the required interface
* Add a connection to the project GameManager in the project LoadingManager
  * Override CreateManagers() in the loading manager to create the GameManager prefab
* Create an empty Prefab and add the GameManager component to it
* Create a GameData in Data/Data and attach it to the manager
  * Set the World Layer to World
  * Configure as necessary
    * Checking "Gamepads Are Players" is often a good idea
* Create a CreditsData in Data/Data and attach it to the GameManager

# Initial Scene Setup

* Create a new Lighting Settings Assets/Data/Rendering/ui.lighting
  * Disable Auto Generate
  * Disable Realtime Global Illumination
  * Disable Baked Global Illumination

## Splash Scene Setup

* **NOTE:** Anything that happens in the splash screen needs to be aware that the CultureInfo is not setup to be invariant
* Create and save a new Basic (URP) scene (Assets/Scenes/splash.unity)
  * The only object in the scene should be the Main Camera
* Setup the camera in the scene
  * Set the Tag to Untagged
  * Clear Flags: Solid Color
  * Background: Opaque Black
  * Culling Mask: Everything
  * Projection: Perspective
  * Uncheck Occlusion Culling
  * Disable HDR
  * Disable MSAA
  * Leave the Audio Listener attached to the camera for audio to work
  * Add the UICameraAspectRatio component to the camera
* Attach the UI lighting settings
  * Remove the Environment Skybox Material
  * Environment Lighting Source: Color
* Add the scene to the Build Settings and ensure that it is Scene 0
* Add a new GameObject to the scene (SplashScreen) and add the SplashScreen component to it
* Attach the camera to the Camera field of the SplashScreen component
* Add whatever splash screen videos to the list of Splash Screens on the SplashScreen component
* Set the Main Scene Name to match whatever the name of your main scene is
  * The main scene should also have been added (or will need to be added) to the Build Settings along with any other required scenes

## Main Scene Setup

* Create and save a new Basic (URP) scene (Scenes/main.unity)
  * The only object in the scene should be the Main Camera
* Setup the camera in the scene
  * Set the Tag to Untagged
  * Clear Flags: Solid Color
  * Background: Opaque Black
  * Culling Mask: Nothing
  * Projection: Orthographic
  * Uncheck Occlusion Culling
  * Disable HDR
  * Disable MSAA
  * Leave the Audio Listener attached to the camera for audio to work
  * Add the UICameraAspectRatio component to the camera
* Attach the UI lighting settings
  * Remove the Environment Skybox Material
  * Environment Lighting Source: Color
* Add the scene to the Build Settings

### Loading Screen Setup

* Add a new LoadingScreen object to the scene with the LoadingScreen component
  * Layer: UI
  * Add a new Canvas object below the LoadingScreen
    * Render Mode: Screen Space - Overlay
    * UI Scale Mode: Scale With Screen Size
    * Reference Resolution: 1280x720
    * Match Width Or Height: 0.5
    * Remove the Graphic Raycaster
    * Set the Canvas on the LoadingScreen object
    * Remove the EventSystem object that gets added (or turn it into a prefab if that hasn't been created yet)
* Add a Panel under the Canvas
  * Clear the Source Image
  * Color: (0, 0, 0, 255)
  * Disable Raycast Target
* Add an Image (Title) under the Panel
  * Stretch the Rect Transform
  * Color: (255, 0, 255, 255)
  * Disable Raycast Target
  * Eventually this can be replaced with the actual title screen
* Add a Text - TextMeshPro (Name) under the Panel
  * Text: "Placeholder"
  * Center the text
  * Disable Raycast Target
* Add an Empty GameObject (Progress) under the Panel and add the ProgressBar component to it
  * Pos Y: -75
* Attach the ProgressBar component to the LoadingScreen component
* Add an Image under the Progress Bar (Background)
  * Position: (0, -200, 0)
  * Size: (500, 25)
  * Color: (0, 0, 0, 255)
  * Source Image: Core Progress Image
  * Disable Raycast Target
* And an Image under the Background Image (Foreground)
  * Position: (0, 0, 0)
  * Size: (500, 25)
  * Color: (255, 255, 255, 255)
  * Source Image: Core Progress Image
  * Disable Raycast Target
  * Image Type: Filled
  * Fill Method: Horizontal
  * Fill Origin: Left
  * Fill Amount: 0.25
* Attach the images to the ProgressBar component
* Add a Text - TextMeshPro (Status) under the Progress Bar
  * Position: (0, -150, 0)
  * Size: (750, 50)
  * Text: "Loading..."
  * Center the text
  * Disable Raycast Target
* Attach the Text to the LoadingScreen component
* Optionally, add loading tips (these can oftentimes not show up if loading levels is too fast)
  * Add a Text - TextMeshPro (LoadingTips) under the Progress Bar
    * Position: (0, -250, 0)
    * Size: (750, 50)
    * Text: "Tip of the Day"
    * Center the text
    * Disable Raycast Target
  * Attach the Text to the LoadingScreen component

### Network Connection UI (Multiplayer Only)

* Create a NetworkConnectUI prefab in Prefabs/UI and add the NetworkConnectUI component to it
  * Layer: UI
  * Add a Canvas under the prefab
    * Render Mode: Screen Space - Overlay
    * UI Scale Mode: Scale With Screen Size
    * Reference Resolution: 1280x720
    * Match Width Or Height: 0.5
    * Set the Canvas on the Menu object
    * Remove the EventSystem object that gets added (or turn it into a prefab if that hasn't been created yet)
  * Add a Panel under the Canvas (Main)
    * Disable Raycast Target
    * Color: (255, 0, 0, 255)
  * Add a Text - TextMeshPro (Status) under the Panel
    * Position: (0, -150, 0)
    * Size: (750, 50)
    * Text: "Network status..."
    * Center the text
    * Disable Raycast Target
  * Attach the Text to the NetworkConnectUI component
  * Add an empty GameObject under the Panel (Container)
    * Stretch the container
    * Add a Vertical Layout Group
      * Spacing: 10
      * Alignment: Lower Center
      * Child Controls Width / Height
      * No Child Force Expand
    * Add the MenuButton under the container (Cancel)
      * **TODO:** this is created later in the process ...
      * Text: "Cancel"
      * Add an On Click handler that calls the NetworkConnectUI OnCancel method
      * Check Is Back Button
* Attach the NetworkConnectUI prefab to the NetworkConnectState prefab

### Loader Setup

* Add the LoadingManager prefab to the scene
* Attach the Main Camera
* Attach the LoadingScreen to the Loader
* Attach all of the manager prefabs to the scene LoadingManager

At this point, the main scene should be runnable but will error out until the main_menu scene is completed.

# Main Menu Setup

## Menu Button

* Create an empty Prefab at Prefabs/UI/MenuButton
  * Layer: UI
  * Add a Layout Element
    * Preferred Width: 200
    * Preferred Height: 50
  * Add a Button child - TextMeshPro (Button)
    * Remove the Canvas and EventSystem that get added
    * Reset the Rect Transform
    * Disable Raycast Target on the Text
    * Stretch the container (Button)
    * Center the text
  * Normal Color: (255, 0, 255, 255)
  * Highlight Color: (0, 255, 0, 255)
  * Select Color: (0, 255, 0, 255)
  * Add a Button Helper to the button

## Main Menu

* Create a new Menu/MainMenu script that overrides the Game MainMenu and implement the required interface
* Create a MainMenu Prefab in Prefabs/Menus and add the Game Menu component to it
  * Layer: UI
  * Add a Canvas under the prefab
    * Render Mode: Screen Space - Overlay
    * UI Scale Mode: Scale With Screen Size
    * Reference Resolution: 1280x720
    * Match Width Or Height: 0.5
    * Set the Canvas on the Menu object
    * Remove the EventSystem object that gets added (or turn it into a prefab if that hasn't been created yet)
  * Add a Panel under the Canvas (Main)
    * Remove the Image component
    * Add the MainMenu script to the panel
      * Set Owner to the Menu object
      * Set the Main Panel on the Menu object to the Main panel
    * Optionally, add credits text
      * Add a Text - Text Mesh Pro under the Panel (Credit)
        * Text: "A PDX PartyParrot Game"
          * Pos Y: 200
          * Width: 500
          * Height: 50
          * Center the text
          * Disable Raycast Target
        * Attach to the Credit Title Text
    * Add an empty GameObject under the Panel (Container)
      * Stretch the container
      * Add a Vertical Layout Group
        * Spacing: 10
        * Alignment: Middle Center
        * Child Controls Width / Height
        * No Child Force Expand
      * Add a UIObject component to the container
        * Id: main_menu_buttons
      * Add the MenuButton under the container (Start)
        * Text: "Start"
        * Add an On Click handler that calls the MainMenu OnStart method
      * Set the Main Menu Initial Selection to the Start Button
      * **TODO:** Multiplayer if networking
      * Add the MenuButton under the container (High Scores) if desired
        * Text: "High Scores"
        * Add an On Click handler that calls the MainMenu OnHighScores method
      * Add the MenuButton under the container (Credits) if desired
        * Text: "Credits"
        * Add an On Click handler that calls the MainMenu OnCredits method
      * Add the MenuButton under the container (Quit)
        * Text: "Quit"
        * Add an On Click handler that calls the MainMenu OnQuitGame method
        * Check Is Back Button
* Attach the MainMenu prefab to the MainMenuState Menu Prefab

### Multiplayer (optional)

* **TODO:** host local (optional)
* **TODO:** dedicated server (optional)
* **TODO:** connect client (optional)
  * **TODO:** this will need a way to find / input a host

### High Scores (optional)

* Add a Panel under the Canvas (High Scores)
  * Remove the Image component
  * Add the High Scores Menu component to the panel
    * Set Owner to the Menu object
    * Set the High Scores Panel on the Menu object to the Main panel
  * Add an empty GameObject under the Panel (Container)
    * Stretch the container
    * Add a Vertical Layout Group
      * Spacing: 0
      * Alignment: Upper Center
      * Child Controls Width / Height
      * No Child Force Expand
  * **TODO**: finish this

### Credits (optional)

* Add a Panel under the Canvas (Credits)
  * Remove the Image component
  * Stretch the Panel
  * Add the Credits Menu component to the panel
    * Set Owner to the Menu object
    * Set the Credits Panel on the Menu object to the Main panel
  * Add an empty GameObject under the Panel (Container)
    * Stretch the container
    * Add a Text - Text Mesh Pro under the container
      * Text: "Credits"
        * Center the text
        * Disable Raycast Target
    * Add a Scroll View under the container
      * Remove the Image
      * Uncheck Horizontal
      * Movement Type: Clamped
      * Delete the Scroll Bar objects
    * Stretch the Scroll View
    * Add a Scroll Rect Auto Scroll to the Scroll View
      * Delay: 2
      * Scroll Rate: 125
    * Stretch the Viewport Content
    * Add a Text - TextMeshPro under the Scroll View Viewport Content
      * Stretch the text
      * Top Center the text
      * Text: "Credits..."
      * Disable Raycast Target
    * Attach the text to the Credits Menu component
    * Add the MenuButton under the container (Back)
      * Text: "Back"
      * Add an On Click handler that calls the CreditsMenu OnBack method
      * Check Is Back Button
    * Set the Back button as the Initial Selection of the Credits Menu
* **TODO:** this has to be manually sized in order to get the credits to scroll correctly. I'm not sure exactly how to describe what to do for this.

### Character Select

* **TODO:** Not sure what game jam actually used this ? ssj2018?

## Title Screen

* Create a TitleScreen prefab in Prefabs/UI and add the TitleScreen Component to it
  * Layer: UI
  * Add a new Canvas object below the TitleScreen
    * Render Mode: Screen Space - Overlay
    * UI Scale Mode: Scale With Screen Size
    * Reference Resolution: 1280x720
    * Match Width Or Height: 0.5
    * Remove the Graphic Raycaster
    * Remove the EventSystem object that gets added (or turn it into a prefab if that hasn't been created yet)
  * Add a Panel under the Canvas
    * Disable Raycast Target
    * Color: (255, 0, 0, 255)
  * Add a TextMeshPro - Text (Title) under the Panel
    * Pos Y: 256
    * Text: "Placeholder"
    * Center the text
    * Disable Raycast Target
    * Attach the Title to the TitleScreen Component
  * Optionally add a TextMeshPro - Text (SubTitle)
    * Pos Y: 128
    * Text: "Placeholder"
    * Center the text
    * Disable Raycast Target
    * Attach the SubTitle to the TitleScreen Component
* Attach the TitleScreen prefab to the MainMenuState prefab

## Main Menu Scene Setup

* Create and save a new Basic (URP) scene (Scenes/main_menu.unity)
  * The only object in the scene should be the Main Camera
* Setup the camera in the scene
  * Set the Tag to Untagged
  * Clear Flags: Solid Color
  * Background: Opaque Black
  * Culling Mask: Everything
  * Projection: Orthographic
  * Uncheck Occlusion Culling
  * Disable HDR
  * Disable MSAA
  * Remove the Audio Listener
  * Add the UICameraAspectRatio component to the camera
* Attach the UI lighting settings
  * Remove the Environment Skybox Material
  * Environment Lighting Source: Color
* Add the scene to the Build Settings
* The scene should now load when the main scene is run as long as the name of the scene matches what was set in the MainMenuState prefab
  * **NOTE:** The main menu won't initialize until the Game UI step is completed

# Game UI

* Create a new UI/GameUI script that overrides the Game GameUI and implement the required interface
* Create a GameUI Prefab in Prefabs/UI and add the GameUI component to it
  * Layer: UI
  * Add a Canvas under the prefab
    * Render Mode: Screen Space - Overlay
    * UI Scale Mode: Scale With Screen Size
    * Reference Resolution: 1280x720
    * Match Width Or Height: 0.5
    * Remove the Graphic Raycaster
    * Remove the EventSystem object that gets added (or turn it into a prefab if that hasn't been created yet)
* Set the Canvas on the GameUI object
* **TODO:** Port the ggj2020 "IntroUI" concept to something more generic
  * This is essentially a set of timed slides shown before the game starts to explain how to play
* Create a new UI/GameUIManager script that overrides the Game GameUIManager
  * Implement the required interface
* Add a connection to the project GameUIManager in the project LoadingManager
  * Create the GameUIManager prefab in the overloaded CreateManagers() in the project LoadingManager
* Create an empty Prefab and add the GameUIManager component to it
* Attach the GameUI prefab to the manager
* The game should now load to the main menu

## Simple Player HUD (optional)

* Create a new UI/PlayerHUD script that overrides the Game HUD
* Add a Panel (HUD) under the GameUI Canvas
  * Remove the Image component
  * Add the PlayerHUD component to it
  * Set the UIObject Id to "hud"
* Create a new prefab from the PlayerHUD object
* Add a connection to the PlayerHUD to the project GameUI and expose it as a field

## Pause Menu (optional)

* Create a PauseMenu Prefab in Prefabs/Menus and add the Game Menu component to it
  * Layer: UI
  * Add a Canvas under the prefab
    * Render Mode: Screen Space - Overlay
    * UI Scale Mode: Scale With Screen Size
    * Reference Resolution: 1280x720
    * Match Width Or Height: 0.5
    * Set the Canvas on the Menu object
    * Remove the EventSystem object that gets added (or turn it into a prefab if that hasn't been created yet)
  * Add a Panel under the Canvas (Main)
    * Remove the Image component
    * Add the PauseMenu script to the panel
      * Set Owner to the Menu object
      * Set the Main Panel on the Menu object to the Main panel
    * Add a Text - TextMeshPro (Pause)
      * Text: "Pause"
      * Pos Y: 200
      * Center the text
      * Disable Raycast Target
    * Add an empty GameObject under the Panel (Container)
      * Stretch the container
      * Add a Vertical Layout Group
        * Spacing: 10
        * Alignment: Middle Center
        * Child Controls Width / Height
        * No Child Force Expand
      * Add a UIObject component to the container
        * Id: pause_menu_buttons
      * Add the MenuButton under the container (Settings) if desired
        * Text: "Settings"
        * Add an On Click handler that calls the PauseMenu OnSettings method
      * Add the MenuButton under the container (Resume)
        * Text: "Resume"
        * Add an On Click handler that calls the PauseMenu OnResume method
      * Add the MenuButton under the container (Main Menu)
        * Text: "Main Menu"
        * Add an On Click handler that calls the PauseMenu OnExitMainMenu method
        * Check Is Back Button
      * Add the MenuButton under the container (Quit)
        * Text: "Quit"
        * Add an On Click handler that calls the PauseMenu OnQuitGame method
        * Check Is Back Button
    * Set the Pause Menu Initial Selection to the Settings Button if used, otherwise set it to the Resume button
* Attach the PauseMenu prefab to the GameUIManager Prefab

## Settings (optional)

* Create a new Menu/SettingsMenu script that overrides the Game SettingsMenu and implement the required interface
* Add a Panel under the Canvas (Settings)
  * Remove the Image component
  * Add the Settings Menu component to the panel
    * Set Owner to the Menu object
    * Set the Settings Panel on the Pause Menu
  * Add an empty GameObject under the Panel (Container)
    * Stretch the container
    * Add a Vertical Layout Group
      * Spacing: 0
      * Alignment: Upper Center
      * Child Controls Width / Height
      * No Child Force Expand
    * Add a Text - Text Mesh Pro under the container
      * Text: "Settings"
        * Center the text
        * Disable Raycast Target
    * **TODO:** Setup the available options (ssjJune2021 had this for inverting the Y-axis)
    * **TODO:** Audio
      * **TODO:** Master volume
      * **TODO:** SFX volume
      * **TODO:** Dialogue volume
      * **TODO:** Music volume
    * **TODO:** Controls
      * **TODO:** Show keyboard and gamepad controls (just a replacable image here is fine)
    * Add a Spacer under the container
      * Flexible Height
    * Add the MenuButton under the container (Back)
      * Text: "Back"
      * Add an On Click handler that calls the SettingsMenu OnBack method
      * Check Is Back Button
    * Set the Back button as the Initial Selection of the Settings Menu

## Game Over UI (optional)

* Create a new UI/GameOverUI script that overrides the Game GameOverUI and implement the required interface
* Create a GameOverUI Prefab in Prefabs/UI and add the GameOverUI component to it
  * Layer: UI
  * Add a Canvas under the prefab
    * Render Mode: Screen Space - Overlay
    * UI Scale Mode: Scale With Screen Size
    * Reference Resolution: 1280x720
    * Match Width Or Height: 0.5
    * Remove the Graphic Raycaster
    * Remove the EventSystem object that gets added (or turn it into a prefab if that hasn't been created yet)
* Add a Panel under the Canvas (Main)
  * Set the Image Color Alpha to 25
  * Add an empty GameObject under the Panel (Container)
    * Stretch the container
    * Add a Vertical Layout Group
      * Spacing: 0
      * Alignment: Middle Center
      * Child Controls Width / Height
      * No Child Force Expand
    * Add a Text - Text Mesh Pro under the container
      * Text: "Game Over"
        * Center the text
        * Disable Raycast Target

## Game Over Menu (optional)

* **TODO:** what jam had this?

# Game States

## MainGameState

* Create a new State/MainGameState script that overrides the Game MainGameState
  * Implement the required interface
* Create an empty Prefab and add the MainGameState component to it
* **TODO:** setup the initial level and set the intial scene name
  * Remove the camera
  * Add to build settings
  * Check Make Initial Scene Active
* Attach to the GameData
* For a simple local game, adding ```GameStateManager.Instance.StartLocal(GameManager.Instance.GameData.MainGameStatePrefab);``` to the MainMenu override OnStart() will start the game in the main game state

## GameOverState (optional)

* Create a new State/GameOverState script that overrides the Game GameOverState
  * Implement the required interface
* Create an empty Prefab and add the GameOverState component to it
* Attach the Game Over UI or Game Over Menu prefabs as desired
* Attach to the MainGameState
* Attach to the SceneTester

# Viewer

* Create a new Camera/{Player / GameViewer} script that overrides one of the Game Viewers that implements the IPlayerViewer interface
* Create an empty Prefab and add the project Viewer script to it
  * Layer: Viewer
  * Add a camera under the prefab (Camera)
    * Clear Flags: Solid Color (or Skybox for a skybox)
    * Background: Opaque Black (or Default for a skybox)
    * Remove the Audio Listener
    * Add CinemachineBrain to Camera
    * Add a Post Process Layer component to the Camera object
      * Set the Layer to PostProcessing
      * Make sure Directly to Camera Target is unchecked
  * Attach the Camera to the Viewer component
  * Add another camera under the prefab (UICamera)
    * Clear Flags: Solid Color
    * Background: Opaque Black
    * Remove the AudioListener
    * Add the UICameraAspectRatio component to the UI Camera
  * Attach the UI Camera to the Viewer component
  * Add an empty GameObject under the prefab (PostProcessingVolume) and add a Post Process Volume to it
  * Attach the Post Process Volume to the Viewer component
  * Create the Post Process Layer (one per-viewer, Viewer{N}_PostProcess)
  * **TODO:** wtf is this stuff:
    * Create a new layer for each potential viewer
    * **TODO:** Need to make sure we put each viewer on its own layer
  * Set the CinemachineVirtualCamera settings as necessary
    * The Body is probably the most useful setting to adjust
      * 3rd Person Follow is good for follow cameras (Transposer is a lesser version of this)
        * Collision filters can be setup here
      * Framing Transposer is good for keeping multiple objects in a fixed view
  * If necessary, add a CinemachineTargetGroup to a subobject for group targeting (ggj2020)
  * Configure any additional settings as required
* Attach the Viewer prefab to the GameData
* Vieweport Size (2D) and FoV (3D) can be adjusted on the GameData

## Viewer Initialization

* It can be useful to have the project GameManager and GameData expose the project viewer prefab
* The MainGameState's InitializeServer() method should call GameManager.Instance.StartGameServer();
* The MainGameState's InitializeClient() method should call GameManager.Instance.StartGameClient();
  * This is also a good place to Allocate(), Acquire(), and Initialize() the viewer in a simple game
  * This is also a good place to initialize the game UI

# Player

## Player

* Create a new Data/Players/PlayerData script that overrides the Game PlayerData
* Create a new Players/Player script that overrides one of the Game Players
  * Implement the required interface

## PlayerBehavior

* Create a new Data/Players/PlayerBehaviorData script that overrides one of the Game PlayerBehaviorData
* Create a new Players/PlayerBehavior script that overrides one of the Game PlayerBehavior
  * Implement the required interface

## PlayerInput

* Create a new Data/Players/PlayerInputData script that overrides the Game PlayerInputData
* Create a new Players/PlayerInputHandler script that overrides one of the Game PlayerInputHandlers
  * Implement the required interface
* Gamepads Are Players can be set on GameData to automatically assign players to gamepads

## PlayerControls

* The input actions file created earlier has two Action Maps by default
  * Player
    * In-game player controls go here
  * UI
    * UI related controls go here

## Player Prefab

* Create an empty Prefab and add the Player component to it
  * This will require a collider to be added first
    * Adjust the size and position of the collider
  * Layer: Player
  * Tag: Player
    * **TODO:** Not sure if we need to do some adjustments here for multiplayer?
    * This tag is needed for 3rd Person Follow camera collision detection
  * Connect the NetworkPlayer component to the Player component
    * This is required even if not using networking
  * Setup networking if using it (**TODO:** this is all probably wrong now)
    * Check the Local Player Authority box in the Network Identity
    * Attach the empty animator controller to the Animator
      * This will stop potential animator error spam
    * Attach the Animator to the Network Animator
* Add a new empty GameObject under the Player prefab (Model)
  * Attach this to the Model on the Player component
  * The actual model for the player should go under this container
    * Placeholder models should have their collider removed
  * It can be useful to create a script for this to handle model-related behavior
* Add a new empty GameObject under the Player prefab (Behavior) and add the PlayerBehavior component to it
  * Attach the Player Behavior to the Actor Components of the Player component
* Add a new empty GameObject under the Player prefab (Movement) and add one of the PlayerMovement components to it
  * Attach the Rigidbody on the Player to the Movement Rigidbody
  * Attach the Player Movement to the Actor Components of the Player component
  * **TODO:** Animator on the Player Behavior ???
* Add a new empty GameObject under the Player Prefab (Input) and add the project PlayerInputHandler component to it
  * Attach the PlayerInputHandler to the Player component
  * Attach the Player input actions asset to the Unity PlayerInput
    * Default Map should be set to Player
    * Change Behavior to Invoke Unity Events
    * Hook up the main device events (lost, regained, changed)
      * **TODO:** where are these event handlers?? ggj2021 had them?
    * Hook up any events as necessary to the input handler
      * For example OnMoveAction is necessary to move, OnPauseAction is necessary to pause, etc
  * Attach the Player to the Owner on the PlayerInputHandler component
  * Create a PlayerInputData in Data/Data/Players and attach it to the PlayerInput component

## PlayerManager

* Create a new PlayerManager script that overrides the Game PlayerManager
  * Implement the required interface
* Add a connection to the project PlayerManager in the project LoadingManager
  * Create the PlayerManager prefab in the overloaded CreateManagers() in the project LoadingManager
* Create an empty Prefab and add the PlayerManager component to it
* Attach the Player prefab to the Player Actor Prefab on the PlayerManager
* Create a PlayerData in Data/Data and attach it to the PlayerManager component
* Create a PlayerBehaviorData in Data/Data and attach it to the PlayerManager component
  * Set the Actor Layer to Player
* Attach the manager to the LoadingManager prefab

# Levels

## Lighting

* Create new Lighting Settings Assets/Rendering/{name}.lighting as needed
  * Configure as required for the level

## Spawn Points

* Create a new empty GameObject and attach the SpawnPoint component to it
  * Player spawns must be tagged with one of the Player Spawn Point Tags in the Spawn Data

## Levels

* Create and save a new Basic (URP) scene
  * Remove the camera from the scene
* Attach the desired lighting settings
* Add the scene to the Build Settings
* Create a new level helper script that overrides the Game LevelHelper
  * Implement the required interface
  * Levels can share a single helper or it can be per-level as needed
* Create an empty Prefab and add the new level helper component to it
* Add a level helper prefab to each level as they're created
* If using players, create new empty GameObjects and attach Spawnpoint components to them
  * Set the tag to 'Player' on these
* The first level should be set, by name, as the MainGameState Initial Scene
* World objects should be on the World layer and any static objects should be marked as static
* **TODO:** finish this
* **TODO:** Document how to transition levels

# TODO

* **TODO:** More GameStates

# Performance Notes

* Mark all static objects as Static in their prefab editor
