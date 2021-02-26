using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace SpriteGame.Rendering.Sprites
{
    /// <summary>
    /// A sprite sheet that contains a number of sprite images.
    /// </summary>
    /// <remarks>
    /// A single sprite is identified by its (x,y) coordinates in the sprite
    /// sheet The (0,0) origin is the top left of the sprite sheet.
    /// The sprite next to it to the right has coordinates (1,0).
    /// </remarks>
    class SpriteSheet
    {
        /// <summary>
        /// Width of a single sprite, in pixels
        /// </summary>
        public int SpriteWidth { get; protected set; }

        /// <summary>
        /// Height of a single sprite, in pixels
        /// </summary>
        public int SpriteHeight { get; protected set; }

        /// <summary>
        /// How many sprites this sheet contains in X direction
        /// </summary>
        public int SpritesInX { get; protected set; }

        /// <summary>
        /// How many sprites this sheet contains in Y direction
        /// </summary>
        public int SpritesInY { get; protected set; }

        /// <summary>
        /// The internal OpenGL texture object containing the sprite sheet
        /// </summary>
        private Texture2D _tex;

        /// <summary>
        /// Create a new sprite sheet based on a texture file and sprite dimensions.
        /// </summary>
        /// <param name="texturePath">Path to texture image file</param>
        /// <param name="spriteWidth">Width of a single sprite, in pixels</param>
        /// <param name="spriteHeight">Height of a single sprite, in pixels</param>
        public SpriteSheet(string texturePath, int spriteWidth, int spriteHeight)
        {
            this.SpriteHeight = spriteHeight;
            this.SpriteWidth = spriteWidth;

            var texImage = Image.Load<Rgba32>(texturePath);

            this._tex = new Texture2D(texImage);
            this.SpritesInX = texImage.Width / this.SpriteWidth;
            this.SpritesInY = texImage.Height / this.SpriteHeight;
        }

        /// <summary>
        /// Bind this sprite sheet to be used for rendering
        /// </summary>
        /// <param name="unit"></param>
        public void Use(TextureUnit unit)
        {
            this._tex.Use(unit);
        }
    }
}
