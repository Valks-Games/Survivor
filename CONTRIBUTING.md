# Contributing
Make sure you are on the same [Unity Project Version](https://unity3d.com/unity/beta/2019.3#downloads). Here's a reference to the [Unity Scripting API](https://docs.unity3d.com/ScriptReference/).

## Opening an Issue.
1. Gather as much information as you can about the topic (excl. questions)
2. Read the General Guidelines ([#54](https://github.com/valkyrienyanko/Survivor/issues/54))
3. Open an issue with a predefined template
4. Provice as much context as possible in your issue!

## Creating a Pull Request.
1. Always test the application to see if it works as intended with no additional bugs you may be adding!
2. State all the changes you made in the PR, not everyone will understand what you've done!

## New Concepts
Always consult the leader of the project before introducing new concepts.

## C# Style & Guidelines
- Private variables should be camelCase.
- Public variables should follow the PascalFormat
- Methods should follow PascalFormat
- Do not remove informative comments.
- Keep the indentation as is, do not expand something if it's compressed.
- Try to follow the general style the owner has laid down throughout all of the code.

## Animations
### Mixamo Humanoids
Keep the format .FBX for Unity at 30 Frames with no keyframe reduction and check off "In Place" if applicable. Import the .FBX into Blender, then delete the imported armature. Apply the new animation to the original armature. Make sure your applying the animation to the "Armature" and not the "Cube". Export the Blender file to .FBX, import into Unity, set the scale factor to 0.1, set the animation type to Humanoid, go into configure, sample Bind-Pose, then enforce T-Pose, and finally enable Loop Time for all animations.
