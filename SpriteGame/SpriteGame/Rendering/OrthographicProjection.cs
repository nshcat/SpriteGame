using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace SpriteGame.Rendering
{
    /// <summary>
    /// Represents a simple orthographic projection with fixed view matrix for use with OpenGL.
    /// </summary>
    public class OrthographicProjection
    {
        /// <summary>
        /// The view matrix. This always is the identity matrix.
        /// </summary>
        public Matrix4 View => Matrix4.Identity;

        /// <summary>
        /// The projection matrix corresponding to this orthographic projection.
        /// </summary>
        public Matrix4 Projection { get; protected set; }

        /// <summary>
        /// Retrieve <see cref="RenderParams"/> instance based on this projection.
        /// </summary>
        public RenderParams Params => new RenderParams(this.View, this.Projection);

        /// <summary>
        /// Refresh this projection based on given screen dimensions
        /// </summary>
        /// <param name="screenWidth">Width of screen, in pixels</param>
        /// <param name="screenHeight">Height of screen, in pixels</param>
        public void Refresh(int screenWidth, int screenHeight)
        {
            // (0,0) is bottom left of the window
            this.Projection = Matrix4.CreateOrthographicOffCenter(0.0f, (float)screenWidth,
                0.0f, (float)screenHeight, 0.0f, 1.0f);
        }
    }
}
