using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace SpriteGame.Simulation
{
    /// <summary>
    /// Simple axis-aligned bounding box
    /// </summary>
    class AABB
    {
        #region Properties

        /// <summary>
        /// The top left corner of the bounding box
        /// </summary>
        public Vector2 TopLeft { get; protected set; }

        /// <summary>
        /// The bottom right corner of the bounding box
        /// </summary>
        public Vector2 BottomRight { get; protected set; }

        /// <summary>
        /// The center position of the bounding box
        /// </summary>
        public Vector2 Center { get; protected set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Create new AABB from given boundary points
        /// </summary>
        /// <param name="topLeft">The top left corner of the box</param>
        /// <param name="bottomRight">The bottom right corner of the box</param>
        public AABB(Vector2 topLeft, Vector2 bottomRight)
        {
            if (!this.CheckOrdering(topLeft, bottomRight))
                throw new ArgumentException("Given boundary points define an invalid AABB");

            this.TopLeft = topLeft;
            this.BottomRight = bottomRight;
            this.UpdateCenter();
        }
        #endregion

        #region Factory methods
        /// <summary>
        /// Create new AABB from object center and dimensions
        /// </summary>
        /// <param name="center">The center point of the object</param>
        /// <param name="size">Object size</param>
        /// <returns></returns>
        public static AABB FromObjectExtents(Vector2 center, Vector2 size)
        {
            var halfDiagonal = size * 0.5f;

            return new AABB(center - halfDiagonal, center + halfDiagonal);
        }
        #endregion

        #region Internal methods

        /// <summary>
        /// Update center position from current boundaries
        /// </summary>
        protected void UpdateCenter()
        {
            this.Center = (this.TopLeft - this.BottomRight) * 0.5f;
        }

        /// <summary>
        /// Checks whether an AABB defined by the given corner positions would be valid
        /// </summary>
        protected bool CheckOrdering(Vector2 topLeft, Vector2 bottomRight)
        {
            return (topLeft.X < bottomRight.X) && (topLeft.Y < bottomRight.Y);
        }

        #endregion

    }
}
