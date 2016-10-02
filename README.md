Distance Field Raymarcher for Unity
-----------------------------------

This is an implementation of Distance Field Raymarching in the Unity game engine.  It was created to be used as a reference implementation in future projects.  For a comprehensive technical writeup about how Distance Field Raymarching works, see [this article on my blog](http://flafla2.github.io/2016/10/01/raymarching.html).

Below are a few of the major features of this implementation:

1. **Complete Implementation of Distance Field Raymarching**.  This repository demonstrates how to construct a fully working implementation of Distance Field Raymarching.  Using this repository as a base, you can focus on implementation details instead of the more technical aspects of raymarching.  Additionally, I include a number of useful Distance Field primitives (sphere, box, torus, cylinder, etc) and Combine Operations (Union, Intersection, Subtraction) - see ``DistanceFunc.cginc`` for details.  Below is a gif of the included demo scene:
   ![](https://thumbs.gfycat.com/VainFaintGermanspaniel-size_restricted.gif)
2. **Interaction with mesh-based Objects**.  This implementation takes Unity's depth buffer into account, so your raymarched objects can intersect and interact with traditional mesh-based objects.
   ![](https://thumbs.gfycat.com/SimpleInfiniteBlackandtancoonhound-size_restricted.gif)
3. **Heatmap-based Performance Testing**.  The system has support for heatmap-based performance testing using a color ramp.  Below is an example of this output (red = many samples per pixel, blue = fewer samples):
   <img src="http://flafla2.github.io/img/2016-10-01-raymarching/perftest2.png" width="450">
4. **Open Source and License**.  This implementation is released under the liberal MIT License, so it is easy use it in your projects.
