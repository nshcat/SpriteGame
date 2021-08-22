using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace SpriteGame.Rendering
{
    /// <summary>
    /// Vertex data structure for simple vertices that contain color
    /// data
    /// </summary>
    struct ColoredVertex
    {
        // Vertex position
        [VertexAttribute]
        private readonly Vector3 _position;

        // Vertex color
        [VertexAttribute]
        private readonly Color4 _color;

        /// <summary>
        /// The vertex position
        /// </summary>
        public Vector3 Position => _position;

        /// <summary>
        /// The vertex color
        /// </summary>
        public Color4 Color => _color;

        /// <summary>
        /// Create new colored vertex
        /// </summary>
        /// <param name="position">Vertex position</param>
        /// <param name="color">VertexColor</param>
        public ColoredVertex(Vector3 position, Color4 color)
        {
            _position = position;
            _color = color;
        }
    }
}
