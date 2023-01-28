# Assets

Minimum Unity Version: **2021.2**
Last Sync: **2/19/2022** from **ggj2022**

* Commonly Used Game Jam Assets
* Core engine scripts
* Example projects

# Important Notes

* `OnDestroy()` is not called for disabled objects and isn't reliable for cleanup
  * `OnEnable()` / `OnDisable()` are safer

# VSCode on Linux

* Install .net Core SDK - https://docs.microsoft.com/en-us/dotnet/core/install/linux
  * Run all of the apt instructions, don't use snap
  * `dotnet [--list-sdks | --list-runtimes]`
* Install Mono - https://www.mono-project.com/download/stable/
* Need the C# extension installed
  * `"omnisharp.enableRoslynAnalyzers": true`
  * `"omnisharp.enableEditorConfigSupport": true`
  * `"omnisharp.useGlobalMono": "always"`

# Game Jam Notes

* Production
  * Asset tracking
  * Task tracking (spreadsheet, trello, whatever makes sense)
  * Game design doc
  * Storyboarding the major loops, levels, etc
    * Where possible, cut this up and drop it in as placeholder art
* PIGSquad bumper
* Get the credits rolling early
  * At the very least, track contributors early
* Produce builds early and frequently
  * A lot of things can end up breaking in actual builds and it's good to find those things early
* Tutorialize
  * Either through gameplay or intro slides
  * Having a card at the presentation table can help
  * Show controls in UI
* Physical feedback - rumble, etc
* Aural feedback - sound effects, stingers, etc
* Visual feedback - animations, tweens, camera shake, etc
* Setup Effect Triggers for events early on, even if they do nothing
  * Hooking everything up with these in mind makes it easier to slot in effects later
* Get art / animations / audio / UI placeholders in early to start working with triggers and events
  * Art, like code, can be refined as we go but late jam hookups are expensive and likely to be dropped
* Fading levels in and out could be better than showing a transition screen
  * Just in general, making more use of fades would be good
* Fullscreen effects is something we never use that could be really cool
* Producing WebGL builds instead of native builds would make it easier for a larger audience to play the game
* Add the Global Game Jam game site and itch.io game site to the README

## WebGL

* https://itch.io/docs/creators/html5
* **NOTE:** The last time I tried this, the game crashed when Unity tried to render its own splash screen that can't be disabled in the free editor
* Set Platform to WebGL
  * DXT compression
* Build
* Create build zip
  * `$ cd Build/`
  * `$ zip -r {filename}.zip *`
* Set itch.io project type to HTML
  * Other platform builds can still be uploaded for direct download
* Upload zip to itch.io and check `This file will be played in the browser`
  * Set whatever Embed options make sense

# Scene Tester

* Create and save a new Basic (URP) scene
  * Remove the camera from the scene
* Attach the desired lighting settings
* Add the scene to the Build Settings
* Add the scene, by name, to the SceneTester
* Create an empty GameObject (Level) in the scene and add the TestSceneHelper component to it
  * This script can be extended for more advanced functionality
* Test levels may be loaded through the debug menu
  * `Game.GameStateManager.TestScenes`
* **TODO:** finish this
* **TODO:** Test Levels require at least one SpawnPoint tagged with the player spawn tag in order for a player to spawn if using a player

# Level Enter Dialogue

* Useful for tutorializing
* gg2021 had a UI that was enabled on the first level's IntroEffect. Now a DialogueEffectTriggerComponent can be used instead.
  * **TODO:** This currently cannot chain dialogues but it really needs to be able to

# Audio Importing

* Music
  * Load In Background: False
  * Load Type: Streaming
  * Compression: Vorbis
  * Quality: 100
  * These should not be MP3s as Unity can't loop MP3s correctly (Ogg works better)
* Stinger / Dialogue
  * Load In Background: False
  * Load Type: Decompress On Load
  * Preload Audio Data: True
  * Compression: Vorbis
  * Quality: 100
* Effects (this includes UI effects)
  * Load In Background: False
  * Load Type: Decompress On Load
  * Preload Audio Data: True
  * Compression: PCM
  * These should be WAVs if possible

# Viewers

* SideScroller25D
  * 2.5D side scoller view
  * Camera Distance on the

# Actor Behaviors

# Character Behaviors

* **TODO:** behavior components that use Animator parameters need the expected parameters and triggers to exist or they will spam warnings when set

# NPCs

## Prefab setup

* Create an empty Prefab and add the NPC type component to it
  * This will require a collider to be added first
    * Adjust the size and position of the collider
  * Layer: NPC
  * Disable the NavMesh Agent and Obstacle components
  * Configure the NavMesh components as desired
* Add a new empty GameObject under the NPC prefab (Model)
  * Attach this to the Model on the NPC type component
  * The actual model for the NPC should go under this container
    * Placeholder models should have their collider removed
  * It can be useful to create a script for this to handle model-related behavior
* Add a new empty GameObject under the NPC prefab (Behavior) and add the NPC type behavior component to it
  * Attach the NPC Behavior to the Actor Components of the NPC component
* Add a new empty GameObject under the NPC prefab (Movement) and add one of the CharacterMovement components to it
  * Attach the Rigidbody on the NPC to the Movement Rigidbody
  * Attach the NPC Movement to the Actor Components of the NPC component
  * **TODO:** Animator on the NPC Behavior ???
* **TODO:** NPCs main collider gets forced to be a trigger so they need a child object with another collider to act as their physics collider
  * So the top level GameObject needs to be an NPC type trigger layer, not the main NPC layer, everything else goes on the NPC layer
    * Players should be set to collide with the NPC type trigger layer but not the NPC physics layer (the NPC physics layer should collide with the World layer)
  * This needs an NPCPhysics sub-class on it
  * This is also where collision listening happens
  * **TODO:** should we also be doing this with Player prefabs?

# AI (NavMesh)

* Configure the NavMeshSurface attached to the level
  * Agent Type: Humanoid
  * Collect Objects: All
  * Include Layers: World
  * Use Geometry: Physics Colliders
  * Default Area: Walkable
* NavMeshModifier can be used to ignore colliders that shouldn't be part of the mesh
* All walkable surfaces and obstacles need to be marked as Navigation Static under the Object tab of the Navigation inspector
* Bake settings should be adjusted to match the agent size
* NavMesh baking uses 2x the agent radius plus some amount of padding, so small surfaces will often fail to be recognized as walkable. Either make the agent less round or make walkable surfaces bigger to fix that.
* NavAgent rotation can be disabled by setting Angular Move Speed to 0 in the behavior data
* NavMeshModifier can be used to exclude areas from the NavMesh bake

# Effects (Triggers)

* **TODO:** Deprecate this in favor of Visual Scripting
* Create an empty GameObject and add an EffectTrigger to it
* Add desired EffectTriggerComponent's to the GameObject and attach to the EffectTrigger's Components list
* EffectTrigger's can be chained through the Trigger On Complete list
* RumbleEffectTriggerComponent's need to have their PlayerInput initialized before they'll work correctly

# Visual Scripting

* Assembly definitions need to be added to the Node Library
* Types for variables need to be added to the Type Library
* Regenerate Nodes needs to be run any time a node type is added or updated
* Object variables can be used to attach local objects to script variables
  * For example, to attach the PlayerInputHelper to the Rumble node, an Object variable can be used to bring the component into the script
* Subgraphs can be used to organize functionality
  * These typically need a Trigger Input (Invoke) and Output (Exit) added to get into and out of them
* Currently, if builds fail with AotStubs.cs not being able to find a type, the Assembly Definition containing that type needs to have Auto Reference checked to work around it

# Interactables

* Interactable objects implement IInteractable
* Interactables*D component required for things that want to interact with Interactables

# World Boundary

* Attach one of the WorldBoundary scripts to the world boundary object
  * This will set the collider to be a trigger
* Implement IWorldBoundaryCollisionListener on the object that will listen for collision triggers from the WorldBoundary

# Floating Text

* Create empty Prefabs/UI/FloatingText and add the FloatingText component to it
  * **TODO:** what else goes into this (ggj2019 uses it)?
  * Attach to the GameData

# Cinematics

* **TODO:**

# Dialogues

## Prefab setup

* Create an empty prefab in Prefabs/Dialogues and add the Dialogue component to it
  * Layer: UI
  * Set the UIObject Id to something unique
  * Add a new Canvas object below the Dialogue
    * Render Mode: Screen Space - Overlay
    * UI Scale Mode: Scale With Screen Size
    * Reference Resolution: 1280x720
    * Match Width Or Height: 0.5
    * Remove the Graphic Raycaster
    * Remove the EventSystem object that gets added (or turn it into a prefab if that hasn't been created yet)
  * Add a Panel below the Canvas
  * Add whatever UI elements make sense for the dialogue
  * Attach the prefab to the DialogueData
* NextDialogue can be set to chain dialogues together
* Allow Cancel can be checked to allow a dialogue to be cancelled in addition to being advanced

## Usage

* Call DialogueManager.Instance.ShowDialogue() with the prefab
* Alternatively DialogueEffectTriggerComponent can be used
* Dialogue UIObject can be used for interactions

# Misc Notes

* SpawnPoint can be overriden to pass additional context to actors as they're spawned
* In general for things that need to trigger but also have physics collisions, putting the physics collider on a child object and the trigger on the main object solves the problem fairly well
