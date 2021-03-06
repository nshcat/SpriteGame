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
        /// Vertex structure for sprites
        /// </summary>
        private struct SpriteVertex : IVertex<SpriteVertex>
        {
            // Vertex position
            private readonly Vector3 _position;

            // Texture coordinates
            private readonly Vector2 _texCoords;

            /// <summary>
            /// Texture coordinates for this vertex
            /// </summary>
            public Vector2 TextureCoordinates => _texCoords;

            /// <summary>
            /// Position of this vertex
            /// </summary>
            public Vector3 Position => _position;

            /// <summary>
            /// Size of a vertex, in bytes
            /// </summary>
            public int Size => sizeof(float) * 5;

            /// <summary>
            /// Vertex attribute descriptors for this type
            /// </summary>
            public VertexAttributeDescription[] Attributes => new VertexAttributeDescription[]
            {
                new VertexAttributeDescription(VertexAttribType.Float, 3),  // Position (Vec3)
                new VertexAttributeDescription(VertexAttribType.Float, 2)   // TexCoords (Vec2)
            };

            /// <summary>
            /// Create new sprite vertex
            /// </summary>
            /// <param name="position">Vertex position</param>
            /// <param name="texCoords">Vertex texture coordinates</param>
            public SpriteVertex(Vector3 position, Vector2 texCoords)
            {
                _position = position;
                _texCoords = texCoords;
            }
        }

        /// <summary>
        /// The sprite sheet this sprite uses to rendering
        /// </summary>
        public SpriteSheet Sheet { get; protected set; }

        /// <summary>
        /// The internal triangle mesh used to render the sprite
        /// </summary>
        private Mesh<SpriteVertex> _mesh
            = new Mesh<SpriteVertex>(MeshPrimitiveMode.Triangles, MeshRenderMode.Indexed);

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
            this._mesh.AddVertex(new SpriteVertex(new Vector3(-hwidth, hheight, 0.0f), new Vector2(0.0f, 1.0f)));
            this._mesh.AddVertex(new SpriteVertex(new Vector3(hwidth, -hheight, 0.0f), new Vector2(1.0f, 0.0f)));
            this._mesh.AddVertex(new SpriteVertex(new Vector3(-hwidth, -hheight, 0.0f), new Vector2(0.0f, 0.0f)));
            this._mesh.AddVertex(new SpriteVertex(new Vector3(hwidth, hheight, 0.0f), new Vector2(1.0f, 1.0f)));

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
