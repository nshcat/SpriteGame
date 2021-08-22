using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;
using SpriteGame.Rendering;


namespace SpriteGame.Simulation
{
    // TODO implement rotation => We just need a different way to calculate AABB
    // TODO velocity, Update, ApplyForce()

    /// <summary>
    /// Object that manages both the position and bounding info for a game object.
    /// </summary>
    class TransformedCollider : ISimulationObject
    {
        private Vector2 _position;

        /// <summary>
        /// The bounding box used for collision detection
        /// </summary>
        public AABB BoundingBox { get; protected set; }

        /// <summary>
        /// The current center position of the collider
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
                this.UpdateBoundingBox();
            }
        }

        /// <summary>
        /// The size of the collider (width and height)
        /// </summary>
        public Vector2 Size { get; protected set; }

        /// <summary>
        /// The sprite layer this collider exists on. 
        /// </summary>
        public WorldLayer Layer { get; protected set; }

        /// <summary>
        /// The world layers this object will not collide with
        /// </summary>
        public HashSet<WorldLayer> IgnoredLayers { get; set; }
            = new HashSet<WorldLayer> { WorldLayer.Background };

        /// <summary>
        /// Create new transformed collider
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="layer"></param>
        public TransformedCollider(Vector2 position, Vector2 size, WorldLayer layer)
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

        public void ApplyForce(Vector2 force)
        {

        }

        public void StopMovement()
        {

        }

        public void Update(float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
