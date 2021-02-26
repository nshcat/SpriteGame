using System;
using System.Collections.Generic;
using System.Text;

namespace SpriteGame.Rendering
{
    /// <summary>
    /// Interface for types that can be rendered to the screen
    /// </summary>
    interface IRenderable
    {
        /// <summary>
        /// Render object to screen using given rendering parameters
        /// </summary>
        /// <param name="rp">Current rendering parameters</param>
        void Render(RenderParams rp);
    }
}
