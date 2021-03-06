using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace SpriteGame.Rendering.Sprites
{
    /// <summary>
    /// Shader material used to render a sprite
    /// </summary>
    partial class SpriteMaterial
    {

        /// <summary>
        /// The shader program used to render a sprite
        /// </summary>
        private ShaderProgram _program;

        /// <summary>
        /// The associated sprite sheet
        /// </summary>
        private SpriteSheet _sheet;

        /// <summary>
        /// The index of the currently active sprite in the sprite sheet
        /// </summary>
        public int SpriteIndex { get; set; } = 0;

        /// <summary>
        /// Create a new sprite material based on given sprite sheets attributes
        /// </summary>
        /// <param name="sheet"></param>
        public SpriteMaterial(SpriteSheet sheet)
        {
            this._sheet = sheet;
            this._program = new ShaderProgram(
                Shader.FromText(ShaderType.VertexShader, _vertexShaderSource),
                Shader.FromText(ShaderType.FragmentShader, _fragmentShaderSource)
            );
        }

        /// <summary>
        /// Use this sprite material to render a sprite
        /// </summary>
        /// <param name="rp">Current rendering parameters</param>
        public void Use(RenderParams rp)
        {
            rp.PushMatrix();
            //rp.RotateZ((float)Math.PI / 4.0f);
            rp.Translate(100, 100, 0);

            this._program.Use();
            this._program.LoadMatrices(rp);
            this._program.SetInt("sprite_sheet", 0);
            this._program.SetInt("sprites_in_x", this._sheet.SpritesInX);
            this._program.SetInt("sprites_in_y", this._sheet.SpritesInY);
            this._program.SetInt("sprite_index", this.SpriteIndex);

            rp.PopMatrix();
        }
    }
}
