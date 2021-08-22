using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace SpriteGame.Rendering
{
    /// <summary>
    /// Vertex data structure for simple vertices that contain texture
    /// coordinates
    /// </summary>
    struct TexturedVertex
    {
        // Vertex position
        [VertexAttribute]
        private readonly Vector3 _position;

        // Texture coordinates
        [VertexAttribute]
        private readonly Vector2 _texCoords;

        /// <summary>
        /// The vertex position
        /// </summary>
        public Vector3 Position => _position;

        /// <summary>
        /// The texture coordinates
        /// </summary>
        public Vector2 TexCoords => _texCoords;

        /// <summary>
        /// Create new textured vertex
        /// </summary>
        /// <param name="position">Vertex position</param>
        /// <param name="texCoords">Texture coordinates</param>
        public TexturedVertex(Vector3 position, Vector2 texCoords)
        {
            _position = position;
            _texCoords = texCoords;
        }
    }
}
