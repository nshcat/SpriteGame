using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace SpriteGame.Simulation
{
    // XXX Bounds etc

    /// <summary>
    /// Class managing and updating a collection of <see cref="TransformedCollider"/> objects, and resolving
    /// collisions between them.
    /// </summary>
    class World : ISimulationObject
    {
        /// <summary>
        /// The dimensions of the simulated game world
        /// </summary>
        public Vector2 Size { get; protected set;}

        /// <summary>
        /// Collection of all transformed colliders participating in this simulated world
        /// </summary>
        protected List<TransformedCollider> Colliders { get; }
            = new List<TransformedCollider>();


        /// <summary>
        /// Update all included objects and resolve collisions between them
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
