using System;
using System.Collections.Generic;
using System.Text;

namespace SpriteGame.Rendering.Particles
{
    /// <summary>
    /// Fragment shader source for particle system
    /// </summary>
    partial class ParticleSystem
    {
        private static string _fragmentShaderSource =
            @"
            #version 450

            in float lifetime;
            out vec4 color;

            void main()
            {
	            color = vec4(1.0f);
            }
            ";
    }
}
