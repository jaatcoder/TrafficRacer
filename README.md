# Traffic Racer (Unity)

A Unity traffic racer project with scene flow:
MainMenu -> Garage -> SampleScene

## Unity Version

- Unity Editor: 6000.3.9f1

## What To Commit To Git

To let anyone clone and open this project in Unity, commit these folders/files:

- Assets/
- Packages/
- ProjectSettings/
- Any docs (README, guides)

Do not commit generated cache/build folders:

- Library/
- Temp/
- Logs/
- UserSettings/
- obj/
- Build/ or Builds/

(These are already covered by .gitignore.)

## Open The Project (For Developers)

1. Clone the repository.
2. Open Unity Hub.
3. Add project folder.
4. Open with Unity 6000.3.9f1.
5. Let Unity reimport.
6. Open scene [Assets/Scenes/MainMenu.unity](Assets/Scenes/MainMenu.unity).
7. Press Play.

## Controls

- W / Up Arrow: accelerate
- S / Down Arrow: reverse/brake
- A / Left Arrow: steer left
- D / Right Arrow: steer right
- Space: brake

## Build A Playable Game (For Players Without Unity)

1. Open File -> Build Profiles in Unity.
2. Select target platform (Windows/Mac/Linux).
3. Ensure scenes are included in this order:
   - Assets/Scenes/MainMenu.unity
   - Assets/Scenes/Garage.unity
   - Assets/Scenes/SampleScene.unity
4. Click Build and choose an output folder (for example: Builds/Windows).
5. Zip the build folder and upload it to GitHub Releases.

After that, players can download the release zip and run the game directly without Unity.

## Quick Git Push Checklist

1. Check status:
   - git status
2. Stage all project source files:
   - git add Assets Packages ProjectSettings README.md .gitignore
3. Commit:
   - git commit -m "Prepare full Unity project for sharing"
4. Push:
   - git push origin <your-branch>
