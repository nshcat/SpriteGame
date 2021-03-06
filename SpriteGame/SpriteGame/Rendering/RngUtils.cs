using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace SpriteGame.Rendering
{
    /// <summary>
    /// Static class containing various RNG utilities
    /// </summary>
    static class RngUtils
    {
        /// <summary>
        /// The game-global RNG instance
        /// </summary>
        public static Random RNG { get; } = new Random();

        /// <summary>
        /// Return a randomly interpolated vector between x and y
        /// </summary>
        /// <param name="x">First vector</param>
        /// <param name="y">Second vector</param>
        /// <returns>Randomly interpolated vector between x and y</returns>
        public static Vector3 RandomBetween(Vector3 x, Vector3 y)
        {
            var t = (float)RNG.NextDouble();
            return (1f - t) * x + t * y;
        }

        /// <summary>
        /// Return a randomly interpolated value between x and y
        /// </summary>
        /// <param name="x">First value</param>
        /// <param name="y">Second value</param>
        /// <returns>Randomly interpolated value between x and y</returns>
        public static float RandomBetween(float x, float y)
        {
            var t = (float)RNG.NextDouble();
            return (1f - t) * x + t * y;
        }
    }
}
