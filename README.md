# LifeOnMars - 3D Graphics

The project presents a simplified planetary system with the sun, Mars orbiting around the sun, and a satellite.
It uses it's own custom 3D graphics engine called Jednosc.

## Requirements
The solution uses .NET version 6.0 and runs on Windows machines.

## Objects

The sun is located in the center of the scene.
Mars orbits around it. Both objects are smooth and were created using the SphereUV class.

The third object is a satellite. The satellite orbits around the moving Mars and decreases and increases its height relative to Mars.

The sun has a high ambient coefficient, Mars has a diffuse coefficient, and the satellite has a specular coefficient.

## Lights

The scene has three light sources:
 - a point light located in the center of the scene in the sun,
 - a satellite spotlight with a power of 50 and a blue color,
 - a satellite spotlight with a power of 100 and a green color.

Both spotlights are animated (changing the direction of light) to "scan" the planet Mars.

## Shading

There are three available shading modes:
 - flat,
 - Gouraud,
 - Phong.
They can be selected from the menu on the right side of the application window.

## Cameras

There are three available cameras:
 - a fixed camera focused on the sun,
 - a camera following Mars,
 - a camera "attached" to the satellite.

They can be selected from the menu on the right side of the application window.

## Technical Notes

The solution consists of two projects:
 1. Jednosc - an engine for rendering 3D graphics.
 2. LifeOnMars - an application displaying 3D graphics and animating the scene.

The rendering pipeline can be found in the RendererMultiThread class.
We pass to it an object of the Scene class, which contains all objects, lights, and the camera.

RendererMultiThread has:
 - backface culling,
 - fog effect (especially visible in the camera following Mars),
 - z-buffer,
 - projection from the cube to screen coordinates,
 - clipping triangles outside the rendering cube (on the recommendation of Mr. Kotowski from the lecture, we remove the entire triangle that lies outside the cube),
 - drawing triangles on multiple threads using barycentric coordinates.

RendererMultiThread uses IShader, which calculates the position of the triangle in the rendering cube and sets the appropriate color of pixels. To allow for changing the shader during rendering, RendererMultiThread accepts an IShaderFactory factory.

This pipeline is inspired by the way OpenGL works.

Due to the creation of large objects at the beginning of the program, the appearance of the window may take some time.
