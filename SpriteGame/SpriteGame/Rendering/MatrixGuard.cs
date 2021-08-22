using System;
using System.Collections.Generic;
using System.Text;

namespace SpriteGame.Rendering
{
    /// <summary>
    /// Helper class to be used with a using block. Automatically handles pushing
    /// and popping operations on the matrix stack of an <see cref="RenderParams"/>
    /// instance, used for hierachical rendering.
    /// </summary>
    class MatrixGuard : IDisposable
    {
        /// <summary>
        /// The render parameter instance associated with this matrix guard.
        /// </summary>
        protected RenderParams Parameters { get; set; }

        /// <summary>
        /// Construct a new matrix guard instance. This will cause <see cref="RenderParams.PushMatrix"/>
        /// to be called.
        /// </summary>
        /// <param name="rp">Render parameters to use</param>
        public MatrixGuard(RenderParams rp)
        {
            this.Parameters = rp;
            rp.PushMatrix();
        }

        /// <summary>
        /// Disengage matrix guard. Will call <see cref="RenderParams.PopMatrix"/>.
        /// </summary>
        public void Dispose()
        {
            this.Parameters.PopMatrix();
        }
    }
}
