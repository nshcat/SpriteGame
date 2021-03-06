using System;
using System.Collections.Generic;
using System.Text;

namespace SpriteGame.Rendering
{
    /// <summary>
    /// Base interface for vertex structs
    /// </summary>
    /// <typeparam name="T">Derived type, has to be a struct</typeparam>
    interface IVertex<T> where T : struct
    {
        /// <summary>
        /// Size of a vertex, in bytes
        /// </summary>
        abstract int Size { get; }

        /// <summary>
        /// Descriptors for all attributes
        /// </summary>
        abstract VertexAttributeDescription[] Attributes { get; }
    }
}
