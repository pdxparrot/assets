# Assets

Minimum Unity Version: **2021.2**
Last Sync: **7/3/2021** from **ssjjune2021**

* Commonly Used Game Jam Assets
* Core engine scripts
* Example projects

## VSCode on Linux

* Install .net Core SDK - https://docs.microsoft.com/en-us/dotnet/core/install/linux
* Install Mono - https://www.mono-project.com/download/stable/
* Need the C# extension installed
  * "omnisharp.enableRoslynAnalyzers": true
  * "omnisharp.enableEditorConfigSupport": true
  * "omnisharp.useGlobalMono": "always"

## Scene Tester

* Create and save a new Basic (URP) scene
  * Remove the camera from the scene
* Attach the desired lighting settings
* Add the scene to the Build Settings
* Add the scene, by name, to the SceneTester
* Create an empty GameObject (Level) in the scene and add the TestSceneHelper component to it
  * This script can be extended for more advanced functionality
* Test levels may be loaded through the debug menu
  * Game.GameStateManager.TestScenes
* **TODO:** finish this
* **TODO:** Test Levels require at least one SpawnPoint tagged with the player spawn tag in order for a player to spawn if using a player

## Floating Text

* Create empty Prefabs/UI/FloatingText and add the FloatingText component to it
  * **TODO:** what else goes into this (ggj2019 uses it)?
  * Attach to the GameData

## Cinematics

* **TODO:**

## Dialogues

* **TODO:**
