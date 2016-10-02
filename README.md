Distance Field Raymarcher for Unity
-----------------------------------

This is an implementation of Distance Field Raymarching in the Unity game engine.  It was created to be used as a reference implementation in future projects.  For a comprehensive technical writeup about how Distance Field Raymarching works, see [this article on my blog](http://flafla2.github.io/2016/10/01/raymarching.html).

Below are a few of the major features of this implementation:

1. **Complete Implementation of Distance Field Raymarching**.  This repository demonstrates how to construct a fully working implementation of Distance Field Raymarching.  Using this repository as a base, you can focus on implementation details instead of the more technical aspects of raymarching.  Additionally, I include a number of useful Distance Field primitives (sphere, box, torus, cylinder, etc) and Combine Operations (Union, Intersection, Subtraction) - see ``DistanceFunc.cginc`` for details.  Below is a gif of the included demo scene (click for high res):
   ![](https://thumbs.gfycat.com/VainFaintGermanspaniel-size_restricted.gif)
2. **Heatmap-based Performance Testing**.  The system has support for heatmap-based performance testing using a color ramp.  Below is an example of this output (red = many samples per pixel, blue = fewer samples):
   ![](http://flafla2.github.io/img/2016-10-01-raymarching/perftest2.png)
3. **Open Source and License**.  This implementation is released under the liberal MIT License, so it is easy use it in your projects.