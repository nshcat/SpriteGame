using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

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
        private Mesh<TexturedVertex> _mesh
            = new Mesh<TexturedVertex>(MeshPrimitiveMode.Triangles, MeshRenderMode.Indexed);

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
            var width = (float)this.Sheet.SpriteWidth;
            var height = (float)this.Sheet.SpriteHeight;

            // We want the model space coordinates of the quad to be centered
            // around the origin, in order to allow easier roation
            var hwidth = width / 2f;
            var hheight = height / 2f;

            // The four vertices of the quad
            this._mesh.AddVertex(new TexturedVertex(new Vector3(-hwidth, hheight, 0.0f), new Vector2(0.0f, 1.0f)));
            this._mesh.AddVertex(new TexturedVertex(new Vector3(hwidth, -hheight, 0.0f), new Vector2(1.0f, 0.0f)));
            this._mesh.AddVertex(new TexturedVertex(new Vector3(-hwidth, -hheight, 0.0f), new Vector2(0.0f, 0.0f)));
            this._mesh.AddVertex(new TexturedVertex(new Vector3(hwidth, hheight, 0.0f), new Vector2(1.0f, 1.0f)));

            //   0
            //   +
            //   | \
            // 2 +--+ 1
            this._mesh.AddIndex(0);
            this._mesh.AddIndex(1);
            this._mesh.AddIndex(2);

            //   0   3
            //    +--+
            //     \ |
            //       + 1
            this._mesh.AddIndex(0);
            this._mesh.AddIndex(3);
            this._mesh.AddIndex(1);

            this._mesh.Build();
        }

        /// <summary>
        /// Render sprite to screen
        /// </summary>
        /// <param name="rp">Current rendering parameters</param>
        public void Render(RenderParams rp)
        {
            this.Sheet.Use(TextureUnit.Texture0);
            this._material.SpriteIndex = this.SpriteIndex;
            this._material.Use(rp);
            this._mesh.Render();
        }
    }
}
