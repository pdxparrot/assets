# Assets

Minimum Unity Version: **2021.2**
Last Sync: **7/3/2021** from **ssjjune2021**

* Commonly Used Game Jam Assets
* Core engine scripts
* Example projects

# Import Notes

* OnDestroy() is not called for disabled objects and isn't reliable for cleanup
  * OnEnable() / OnDisable() are safer

# VSCode on Linux

* Install .net Core SDK - https://docs.microsoft.com/en-us/dotnet/core/install/linux
* Install Mono - https://www.mono-project.com/download/stable/
* Need the C# extension installed
  * "omnisharp.enableRoslynAnalyzers": true
  * "omnisharp.enableEditorConfigSupport": true
  * "omnisharp.useGlobalMono": "always"

# Scene Tester

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

# NPC Prefabs

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

# Floating Text

* Create empty Prefabs/UI/FloatingText and add the FloatingText component to it
  * **TODO:** what else goes into this (ggj2019 uses it)?
  * Attach to the GameData

# Cinematics

* **TODO:**

# Dialogues

* **TODO:**

# Misc Notes

* SpawnPoint can be overriden to pass additional context to actors as they're spawned
* In general for things that need to trigger but also have physics collisions, putting the physics collider on a child object and the trigger on the main object solves the problem fairly well
