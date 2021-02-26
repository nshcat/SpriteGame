using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace SpriteGame.Rendering
{
    /// <summary>
    /// Represents a two-dimensional OpenGL texture object.
    /// </summary>
    public class Texture2D : Texture
    {
        /// <summary>
        /// Create new 2D texture from given source image.
        /// </summary>
        /// <param name="source"></param>
        public Texture2D(Image<Rgba32> source)
        {
            // Create empty texture object
            this.Handle = GL.GenTexture();
            if (this.Handle == 0)
                throw new OpenGlException("Failed to create texture object");

            // Mark empty texture object as active, so we can work on it and initialize it
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, this.Handle);

            // For some fucking reason the image is flipped if we dont do this
            source.Mutate(new FlipProcessor(FlipMode.Vertical));

            // Retrieve all pixels in a single span
            Span<Rgba32> pixels;
            var success = source.TryGetSinglePixelSpan(out pixels);

            if (!success)
                throw new OpenGlException("Failed to retrieve source image pixels");

            // Convert to raw bytes
            var pixelBytes = MemoryMarshal.AsBytes(pixels).ToArray();

            // Upload image bytes to the GPU, and set them as the textures data buffer
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, source.Width, source.Height,
                0, PixelFormat.Rgba, PixelType.UnsignedByte, pixelBytes);

            // Generate all mipmaps
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            // Disable texture filtering since we want a retro, pixelated look
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        }

        /// <summary>
        /// Destroy the OpenGL objects associated with this instance.
        /// </summary>
        public void Destroy()
        {
            GL.DeleteTexture(this.Handle);
        }

        /// <summary>
        /// Activate and bind this texture to given texture unit for use in rendering.
        /// </summary>
        /// <param name="unit">Texture unit to bind texture to</param>
        public override void Use(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, this.Handle);
        }
    }
}
