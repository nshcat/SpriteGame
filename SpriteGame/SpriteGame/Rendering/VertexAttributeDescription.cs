using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace SpriteGame.Rendering
{
    /// <summary>
    /// Class describing a single vertex attribute
    /// </summary>
    class VertexAttributeDescription
    {
        /// <summary>
        /// The underlying element type of this attribute
        /// </summary>
        public VertexAttribType AttributeType { get; set; } = VertexAttribType.Float;

        /// <summary>
        /// Length of attribute, in elements
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Length of attribute, in bytes
        /// </summary>
        public int LengthInBytes => this.CalculateLengthInBytes();

        /// <summary>
        /// Create new attribute descriptor
        /// </summary>
        /// <param name="attributeType">Type of element used in attribute</param>
        /// <param name="length">Number of elements in this attribute</param>
        public VertexAttributeDescription(VertexAttribType attributeType, int length)
        {
            AttributeType = attributeType;
            Length = length;
        }

        /// <summary>
        /// Calculate the total length in bytes
        /// </summary>
        /// <returns></returns>
        protected int CalculateLengthInBytes()
        {
            // Maps attribute types to their size in bytes
            var lengthMap = new Dictionary<VertexAttribType, int>
            {
                [VertexAttribType.Byte] = 1,
                [VertexAttribType.Float] = sizeof(float),
                [VertexAttribType.Int] = sizeof(int),
                [VertexAttribType.Short] = sizeof(short),
                [VertexAttribType.UnsignedInt] = sizeof(uint),
                [VertexAttribType.UnsignedShort] = sizeof(ushort)
            };

            if (!lengthMap.ContainsKey(this.AttributeType))
                throw new InvalidOperationException("Unknown attribute type");

            return lengthMap[this.AttributeType] * this.Length;
        }
    }
}
