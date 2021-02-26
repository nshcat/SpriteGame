using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace SpriteGame.Rendering
{
    /// <summary>
    /// An abstraction around OpenGL shader program objects
    /// </summary>
    public class ShaderProgram
    {
        /// <summary>
        /// All shader objects corresponding to this program
        /// </summary>
        protected List<Shader> shaders = new List<Shader>();

        /// <summary>
        /// The native OpenGL handle of this program object
        /// </summary>
        public int Handle { get; private set; }

        /// <summary>
        /// A piece of text containing information about the program linking status
        /// </summary>
        public string Log { get; private set; }

        /// <summary>
        /// Set the projection matrix for this program.
        /// In this scenario, all shaders are required to accept both a projection
        /// aswell as a view matrix.
        /// </summary>
        public Matrix4 Projection
        {
            set => SetMatrix("projection", ref value);
        }

        /// <summary>
        /// Set the view matrix for this program.
        /// In this scenario, all shaders are required to accept both a projection
        /// aswell as a view matrix.
        /// </summary>
        public Matrix4 View
        {
            set => SetMatrix("view", ref value);
        }

        /// <summary>
        /// Set the model matrix for this program.
        /// </summary>
        public Matrix4 Model
        {
            set => SetMatrix("model", ref value);
        }

        /// <summary>
        /// Create new shader program object by linking given shader objects
        /// </summary>
        /// <param name="s">Shader objects to link together</param>
        /// <exception cref="ShaderException">If fatal errors occur while creating or linking shader program</exception>
        public ShaderProgram(params Shader[] s)
        {
            if (s.Length == 0)
                throw new ArgumentException("At least one shader object is required");

            // Create new OpenGL shader program object
            Handle = GL.CreateProgram();

            if (Handle == 0)
            {
                throw new ShaderException("Failed to create shader program object");
            }

            // Process given shader objects
            shaders.AddRange(s);

            foreach (var shader in shaders)
            {
                GL.AttachShader(Handle, shader.Handle);
            }

            // Link shader program
            GL.LinkProgram(Handle);

            // Retrieve log
            Log = GL.GetProgramInfoLog(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out var result);

            if (result != 1)
            {
                throw new ShaderException("Failed to link shader program", Log);
            }
        }

        /// <summary>
        /// Set this program object as the current active one for rendering
        /// </summary>
        public void Use()
        {
            GL.UseProgram(Handle);
        }

        /// <summary>
        /// Load matrices from rendering parameters
        /// </summary>
        /// <param name="rp">Current rendering parameters</param>
        public void LoadMatrices(RenderParams rp)
        {
            this.Projection = rp.Projection;
            this.Model = rp.Model;
            this.View = rp.View;
        }

        /// <summary>
        /// Sets a matrix uniform with given name to given matrix value
        /// </summary>
        /// <param name="name">Uniform name</param>
        /// <param name="mat">New uniform value</param>
        public void SetMatrix(string name, ref Matrix4 mat)
        {
            GL.UniformMatrix4(this.GetLocation(name), false, ref mat);
        }

        /// <summary>
        /// Sets a integer uniform with given name to given value.
        /// </summary>
        /// <param name="name">Uniform name</param>
        /// <param name="value">New uniform value</param>
        public void SetInt(string name, int value)
        {
            GL.Uniform1(this.GetLocation(name), value);
        }

        /// <summary>
        /// Sets a floating point uniform with given name to given value.
        /// </summary>
        /// <param name="name">Uniform name</param>
        /// <param name="value">New uniform value</param>
        public void SetFloat(string name, float value)
        {
            GL.Uniform1(this.GetLocation(name), value);
        }

        /// <summary>
        /// Sets a Vec4 uniform with given name to given vector.
        /// </summary>
        /// <param name="name">Uniform name</param>
        /// <param name="vec">New uniform value</param>
        public void SetVec4F(string name, Vector4 vec)
        {
            GL.Uniform4(this.GetLocation(name), vec);
        }

        /// <summary>
        /// Retrieve location for given uniform name.
        /// </summary>
        /// <param name="name">Uniform name</param>
        /// <returns>Location of uniform with given name</returns>
        protected int GetLocation(string name)
        {
            var location = GL.GetUniformLocation(this.Handle, name);

            if (location == -1)
                throw new InvalidOperationException($"Shader program does not contain uniform with name \"{name}\"");

            return location;
        }
    }
}
