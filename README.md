# 3D Graphics & Animation
This consists of my stripped down work for my report on my 3D Graphics & Animation programming module. Only c# code, shader files, and reports are maintained.
## 1. PGA 1 - First Scene
This doesn't inlcude any meaningful work code-wise so I only kept the report. Just a lit scene.
## 2. [PGA 2 - Physics Scene](https://hristostantchev.itch.io/absolutely-realistic-physics-bowling)
A 2-day project that consisted of a physics bowling game you can find on my itch.io (or through the link in the title for convenience). 
Apart from my scripts, this features research, report, FBX files & their blender origin files. First time I modeled for fun
## 3. [PGA 3 - Key Frame Animation](https://hristostantchev.itch.io/slime-slayer)
This features my C# code, my blender file, FBX exports, and PDF report. It is again, a 2-day project I decided to expand in a little game you can find on my itch.io (or through the title link).
## 4. First GPU Shader
Features CG/HLSL shader code & report. It is an animated shader for Unity URP projects:
- Vertex Shader: Converges from original size into the center of the object, using an animated function made by using time inside a sine function.
- Fragment Shader: Tiling & Offset, has diagonal animation by incrementing offset with time.
## 5. Lit Shader
Features CG/HLSL shader that implements Lambertian lighting in the fragment shader. Not particularly proud of this one, it was later refined in _6. Texture Shader_
## 6. Texture Shader
Once again, CG/HLSL shader code and report. The task was to implement normal mapping & some texture-based animation.
1. Researched more into PBR lighting techniques and found materials and examples to incorporate into my code for a better Lambertian model.
2. Implemented normal mapping & albedo texture sampling in fragment shader.
3. Implemented a dissolve animation using a Perlin noise texture.
4. Added height mapping in the vertex shader for fun.
