using System;
using System.Collections.Generic;
using System.Text;

namespace SpriteGame.Rendering.Particles
{
    /// <summary>
    /// Vertex shader source for particle system
    /// </summary>
    partial class ParticleSystem
    {
        private static string _vertexShaderSource =
            @"
            #version 450

            layout(location = 0) in vec3 vertex_position;
            layout(location = 4) in vec4 particle_data;
            
            uniform float particle_size;

            uniform mat4 projection;
            uniform mat4 view;

            out float lifetime;

            void main()
            {
	            vec4 position_viewspace = view * vec4(particle_data.xyz, 1.0f);
                position_viewspace.xy += particle_size * (vertex_position.xy - vec2(0.5));
                gl_Position = projection * position_viewspace;
                lifetime = particle_data.w;
            }
            ";
    }
}
