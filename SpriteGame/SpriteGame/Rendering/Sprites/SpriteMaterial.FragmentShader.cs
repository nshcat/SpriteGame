using System;
using System.Collections.Generic;
using System.Text;

namespace SpriteGame.Rendering.Sprites
{
    /// <summary>
    /// Fragment shader source code for the sprite material
    /// </summary>
    partial class SpriteMaterial
    {
        private static string _fragmentShaderSource =
            @"
            #version 450

            in vec2 out_tex_coords;
            
            uniform sampler2D sprite_sheet;

            out vec4 color;

            void main()
            {
	            color = texture(sprite_sheet, out_tex_coords);
            }
            ";
    }
}
