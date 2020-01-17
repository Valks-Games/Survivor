# Contributing
We are currently on `Unity 2019.3.05f`, please notify the team if a [newer version](https://unity3d.com/unity/beta/2019.3#downloads) available.

## Unity Good Practices
- Try to avoid putting assets in the Resources folder as much as possible as it increases game startup time and ram usage.
- When comparing gameObject tags use `.CompareTag` instead of `==` to avoid garbage collection.
- Objects at the very bottom of the game hierarchy will be layered over objects above. (e.g. If a panel is above a button, the panel will block the raycast to the button)

## Setting up Visual Studio Code
Throughout my development career, I've worked with several different editors and IDE's. JetBrains, Atom, Notepad++, and Eclipse to name a few. With experience I can now say that Visual Studio Code is my favorite editor and it's what I will be recommending you to use for contributing to this project.

### Installing VSC
1. Download and Install [Visual Studio Code](https://code.visualstudio.com)
2. Follow all steps in [Unity Development with VS Code](https://code.visualstudio.com/docs/other/unity)

### Extensions
Not all extensions are required for development but I highly recommend you at least read through them all.
- [Debugger for Unity](https://marketplace.visualstudio.com/items?itemName=Unity.unity-debug) (Debug Unity in the editor and the compiled build)
- [Unity Meta Files Watcher](https://marketplace.visualstudio.com/items?itemName=PTD.vscode-unitymeta) (Watches Unity all meta files and changes them accordingly)
- [Unity Tools](https://marketplace.visualstudio.com/items?itemName=Tobiah.unity-tools) (View the Unity Scripting Reference through a keybind)
- [Unity Code Snippets](https://marketplace.visualstudio.com/items?itemName=kleber-swf.unity-code-snippets) (Provides you with useful Unity Code Snippets on the fly)
- [Live Share](https://marketplace.visualstudio.com/items?itemName=MS-vsliveshare.vsliveshare) (Real-time collaboration through the VSC editor)
- [Discord Presence](https://marketplace.visualstudio.com/items?itemName=icrawl.discord-vscode) (Shows others what your working on)
- [Material Theme](https://marketplace.visualstudio.com/items?itemName=Equinusocio.vsc-material-theme) (Legacy material themes work the best in my opinion)

## Opening an Issue
1. Gather as much information as you can about the topic (excl. questions)
2. Read the General Guidelines ([#54](https://github.com/valkyrienyanko/Survivor/issues/54))
3. Open an issue with a predefined template
4. Provice as much context as possible in your issue!

## Creating a Pull Request
1. Always test the application to see if it works as intended with no additional bugs you may be adding!
2. State all the changes you made in the PR, not everyone will understand what you've done!

## New Concepts
Always consult the leader of the project before introducing new concepts.

## C# Style & Guidelines
- Private variables should be camelCase.
- If a public variable has 2 or so characters, keep everything UPPERCASE (ID (not Id or id))
- If a private variable has 2 or so characters, keep everything LOWERCASE (id)
- Public variables should follow the PascalFormat
- Methods should follow PascalFormat
- Do not remove informative comments.
- Keep the indentation as is, do not expand something if it's compressed.
- Try to follow the general style the owner has laid down throughout all of the code.

## Sprites
We will be using [Gimp](https://www.gimp.org) to create Sprites. Make sure when finished creating a Sprite, export both the `.xcf` and image file into the Unity assets folder.

## Animations
### Mixamo Humanoids
1. Keep the format .FBX for Unity at 30 Frames with no keyframe reduction and check off "In Place" if applicable. 
2. Import the .FBX into Blender, then delete the imported armature. 
3. Apply the new animation to the original armature. Make sure your applying the animation to the "Armature" and not the "Cube". 
4. Export the Blender file to .FBX, import into Unity, set the scale factor to 0.1, set the animation type to Humanoid, go into configure, sample Bind-Pose, then enforce T-Pose, and finally enable Loop Time for all animations.

## Useful Links
- [Unity Scripting Reference](https://docs.unity3d.com/ScriptReference/)
