using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace SpriteGame.Rendering
{
    /// <summary>
    /// Abstract base class for all OpenGL texture object wrappers.
    /// </summary>
    public abstract class Texture
    {
        /// <summary>
        /// The native OpenGL handle of the managed texture object.
        /// </summary>
        public int Handle { get; protected set; }

        /// <summary>
        /// Activate and bind this texture to given texture unit, making it ready to be used
        /// in a render pass.
        /// </summary>
        /// <param name="unit">Texture unit to bind texture to</param>
        public abstract void Use(TextureUnit unit);
    }
}
