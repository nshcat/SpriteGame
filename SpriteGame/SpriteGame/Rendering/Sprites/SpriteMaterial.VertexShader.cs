using System;
using System.Collections.Generic;
using System.Text;

namespace SpriteGame.Rendering.Sprites
{
    /// <summary>
    /// Vertex shader source code for the sprite material
    /// </summary>
    partial class SpriteMaterial
    {
        private static string _vertexShaderSource =
            @"
            #version 450

            layout(location = 0) in vec3 vertexPosition;
            layout(location = 1) in vec2 vertexTexCoords;
            
            uniform int sprites_in_x;
            uniform int sprites_in_y;
            uniform int sprite_index;

            uniform mat4 projection;
            uniform mat4 view;
            uniform mat4 model;

            out vec2 out_tex_coords;

            void main()
            {
	            float tex_sprite_width = 1.0 / float(sprites_in_x);
	            float tex_sprite_height = 1.0 / float(sprites_in_y);
	
	            vec2 tex_sprite_dims = vec2(tex_sprite_width, tex_sprite_height);

	            vec2 tex_coords_in_sprite = tex_sprite_dims * vertexTexCoords;

                tex_coords_in_sprite.y = tex_coords_in_sprite.y + float(sprites_in_y - 1) * tex_sprite_height;

	            uint sprite_index_x = sprite_index % sprites_in_x;
	            uint sprite_index_y = sprite_index / sprites_in_x;
	
	            vec2 vec_sprite_offset = vec2(float(sprite_index_x), float(sprite_index_y)) * tex_sprite_dims;
	
	            vec2 tex_coords_absolute = tex_coords_in_sprite + vec_sprite_offset;
	
	            out_tex_coords = tex_coords_absolute;
	
	            vec4 vPos = vec4(vertexPosition, 1.0f);
	            gl_Position = projection * view * model * vPos;
            }
            ";
    }
}
