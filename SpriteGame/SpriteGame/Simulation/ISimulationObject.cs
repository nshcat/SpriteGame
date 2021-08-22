using System;
using System.Collections.Generic;
using System.Text;

namespace SpriteGame.Simulation
{
    /// <summary>
    /// Interface for game objects that have state that is periodically
    /// updated based on elapsed game time. Deriving game objects from this interface
    /// causes their logic to become decoupled from the frame rate.
    /// </summary>
    /// <remarks>
    /// Examples for this could be the player, which has to react to gravity
    /// appropiately based on the amount of seconds elapsed since the last update.
    /// </remarks>
    interface ISimulationObject
    {
        /// <summary>
        /// Update internal state using given elapsed seconds since last
        /// update.
        /// </summary>
        /// <param name="deltaTime">Seconds elapsed since last update.</param>
        void Update(float deltaTime);
    }
}
