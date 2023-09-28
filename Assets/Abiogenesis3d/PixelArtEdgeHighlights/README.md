# UPixelator - Pixel Art Edge Highlights

Thank you for purchasing Pixel Art Edge Highlights!
If you like the asset I would appreciate if you would consider rating it!

## Contact
If you have any questions or feedback, please contact me at reslav.hollos@gmail.com.  
You can also join the [Discord server](https://discord.gg/uFEDDpS8ad)  

## Description
Pixel Art Edge Highlights is a post process effect that draws thin edges based on the depth and normal textures screen output.

It is intended to be used with the [Unity Pixelator](https://assetstore.unity.com/packages/slug/243562) asset that pixelates whole 3d scenes in realtime and handles camera movement smoothing and pixel creep reduction.

(You can also use it as a standalone effect to add detail on the edges that would otherwise be indistinguishable like flat colors where normals are equal, see screenshots with the effect off).

## Edge Types
- `Convex Highlights` lightens outward edges
- `Outline Shadow` darkens objects outline
- `Concave Shadow` darkens inward edges

## Supported Pipelines
- Built-in ✓
- URP ✓

## Tested versions
- Unity 2021.3.x: Builtin, URP 12.x
- Unity 2022.3.x: Builtin, URP 14.x

## Setup
- Scene
  - Add `PixelArtEdgeHighlights` component to the hierarchy and you should see the effect working
- On camera
  - Set `Projection: Orthographic`
  - Set `Clipping Planes > Far` to values from 40 to 100
  - Have the camera distanced about 10 meters from the target (internal shader values for depth map are adjusted for it)
  - Disable MSAA or any other anti aliasing (for URP set `Anti Aliasing: Disabled` in the renderer asset)
- Game window
  - Change `Free Aspect` to a fixed resolution eg. `1920x1080`
  - Set `Scale: 1` to have pixels render 1 on 1
  - Aspect ratio of width to height should not be close to or over `2:1`

## Example scene
- Double click `PixelArtEdgeHighlights_URP.unitypackage` to update example materials

## Recommended usage
- Lower values from `0.2` to `0.5` are suggested to get nice colored edges just enough to make the highlights differentiate geometry.

## Excluding objects from the effect
The effect is set to execute before transparents, to exclude an object set its render queue to 3000+.
If an opaque material becomes transparent it could mean it is not outputing alpha values correctly.
To change that you can hardcode the alpha value to 1 (eg. `return float4(some_output.rgb, 1);`).

- [Shader Graph] `Advanced Options > Queue Control: UserOverride` then `Render Queue` will appear
- [Builtin: Standard] Set Rendering Mode to `Fade` (otherwise you wont be able to set the queue higher)
- [URP: Lit] `Surface Type: Transparent`
- [Custom] If there isn't a `Render Queue` visible try setting it through inspector Debug (triple dots right of the inspector lock icon)

## Known Issues
- [Unity 2022; Built-in] The effect is around twice as bright, for now just lower the values please until I find the root cause
- [Unity 2022; URP] There are two obsolete warnings, I will address them when I find replacement code that works
- [URP] If you delete the folder and reimport, you might get "Shader not found", please re-enable the `PixelArtEdgeHighlight` script
