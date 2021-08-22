using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;
using SpriteGame.Rendering;


namespace SpriteGame.Simulation
{
    // TODO implement rotation => We just need a different way to calculate AABB

    /// <summary>
    /// Object that manages both the position and bounding info for a game object.
    /// </summary>
    class TransformedCollider
    {
        /// <summary>
        /// The bounding box used for collision detection
        /// </summary>
        public AABB BoundingBox { get; protected set; }

        /// <summary>
        /// The current center position of the collider
        /// </summary>
        public Vector2 Position { get; protected set; }

        /// <summary>
        /// The size of the collider (width and height)
        /// </summary>
        public Vector2 Size { get; protected set; }

        /// <summary>
        /// The sprite layer this collider exists on. 
        /// </summary>
        /// <remarks>
        /// Layer 0 is the lowest layer, and sprites will be rendered in order of their layer association.
        /// Note that this does not affect collision calculations.
        /// </remarks>
        public uint Layer { get; protected set; }

        /// <summary>
        /// Create new transformed collider
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="layer"></param>
        public TransformedCollider(Vector2 position, Vector2 size, uint layer)
        {
            this.Position = position;
            this.Size = size;
            this.Layer = layer;
        }

        /// <summary>
        /// Update bounding box to correspond to the current object position and size
        /// </summary>
        protected void UpdateBoundingBox()
        {
            this.BoundingBox = AABB.FromObjectExtents(this.Position, this.Size);
        }

        /// <summary>
        /// Apply transformation to given rendering parameters for rendering an object
        /// that this collider is attached to
        /// </summary>
        /// <param name="rp">Rendering parameters to modify</param>
        public void ApplyTransform(RenderParams rp)
        {
            rp.Translate(new Vector3(this.Position.X, this.Position.Y, (float)this.Layer * 0.15f));
        }

        /// <summary>
        /// Translate collider by given direction vector
        /// </summary>
        /// <param name="direction">Direction to translate by</param>
        public void Translate(Vector2 direction)
        {
            this.Position += direction;
            this.UpdateBoundingBox();
        }
    }
}
