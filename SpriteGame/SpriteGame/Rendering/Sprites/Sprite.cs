using System;
using System.Collections.Generic;
using System.Text;

namespace SpriteGame.Rendering.Sprites
{
    /// <summary>
    /// Class managing a single sprite that can be rendered to the screen
    /// </summary>
    class Sprite : IRenderable
    {
        /// <summary>
        /// The sprite sheet this sprite uses to rendering
        /// </summary>
        public SpriteSheet Sheet { get; protected set; }

        /// <summary>
        /// The internal triangle mesh used to render the sprite
        /// </summary>
        private TriangleList _mesh = new TriangleList();

        /// <summary>
        /// The material used to render the sprite
        /// </summary>
        private SpriteMaterial _material;

        /// <summary>
        /// The currently active and drawn sprite in the sprite sheet
        /// </summary>
        public int SpriteIndex { get; set; }

        /// <summary>
        /// Create a new sprite based on given sprite sheet
        /// </summary>
        /// <param name="sheet">The sprite sheet that is used as image source</param>
        public Sprite(SpriteSheet sheet)
        {
            this.Sheet = sheet;
            this._material = new SpriteMaterial(this.Sheet);
            this.BuildMesh();
        }

        /// <summary>
        /// Build the triangle mesh for this sprite
        /// </summary>
        private void BuildMesh()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Render sprite to screen
        /// </summary>
        /// <param name="rp">Current rendering parameters</param>
        public void Render(RenderParams rp)
        {
            this.Sheet.Use(OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
            this._material.SpriteIndex = this.SpriteIndex;
            this._material.Use(rp);
            this._mesh.Render();
        }
    }
}
