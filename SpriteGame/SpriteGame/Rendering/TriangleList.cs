using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace SpriteGame.Rendering
{
    /// <summary>
    /// A simple class abstracting the concept of a vertex buffer (and an index buffer)
    /// containing triangle data. It allows easy creation of models by adding triangles
    /// one by one.
    /// </summary>
    /// <example>
    /// var list = new TriangleList();
    /// list.AddTriangle(...);
    /// list.Build();
    /// </example>
    public class TriangleList
    {
        /// <summary>
        /// A simple struct encapsulating all data of one vertex
        /// </summary>
        protected struct VertexInformation
        {
            /// <summary>
            /// Vertex position
            /// </summary>
            private readonly Vector3 _position;

            /// <summary>
            /// Texture coordinates
            /// </summary>
            private readonly Vector2 _texCoords;

            /// <summary>
            /// Construct a new vertex with given data.
            /// </summary>
            public VertexInformation(Vector3 position, Vector2 texCoords)
            {
                _position = position;
                _texCoords = texCoords;
            }

            /// <summary>
            /// Determine the total size of this structure, in bytes.
            /// </summary>
            public static int Size => (3 * sizeof(float)) + (2 * sizeof(float));
        }



        /// <summary>
        /// Temporary list of vertex data used to build the triangle list.
        /// </summary>
        protected List<VertexInformation> vertexData = new List<VertexInformation>();

        /// <summary>
        /// Flag indicating whether the triangle list has been built and is ready for use.
        /// </summary>
        public bool IsReady { get; protected set; }

        /// <summary>
        /// Native OpenGL handle of the VAO
        /// </summary>
        protected int VAOHandle { get; set; }

        /// <summary>
        /// Native OpenGL handle of the VBO
        /// </summary>
        protected int VBOHandle { get; set; }

        /// <summary>
        /// Number of vertices contained in the list
        /// </summary>
        protected int VertexCount { get; set; }

        /// <summary>
        /// Add a new vertex to the list. The list needs to be in unbuild state.
        /// </summary>
        /// <param name="vertex">Vertex to add</param>
        /// <param name="color">Texture coordinates of vertex</param>
        public void AddVertex(Vector3 vertex, Vector2 texCoords)
        {
            if (IsReady)
                throw new InvalidOperationException("Cannot add vertex to TriangleList that has already been built");

            vertexData.Add(new VertexInformation(vertex, texCoords));
        }

        /// <summary>
        /// Reserve space in the internal array for given number of vertices.
        /// This can be used to avoid reallocation when adding many vertices.
        /// This method can only be used before the mesh is built.
        /// </summary>
        /// <param name="size">Number of vertices to reserve space for</param>
        public void Reserve(int size)
        {
            if (IsReady)
                throw new InvalidOperationException("Called Reserve on already-built triangle mesh");

            // This causes the list to allocate space for at least `size` number of elements.
            vertexData.Capacity = size;
        }

        /// <summary>
        /// Uses the stored temporary vertex data to create a VBO with matching data
        /// </summary>
        public void Build()
        {
            // Create the VAO
            VAOHandle = GL.GenVertexArray();
            GL.BindVertexArray(VAOHandle);

            // Create the VBO
            VBOHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOHandle);

            // Fill it with our data
            GL.NamedBufferData(
                VBOHandle,
                VertexInformation.Size * vertexData.Count,
                vertexData.ToArray(),
                BufferUsageHint.StaticDraw);

            // Set up vertex attribute pointers
            // Position
            GL.VertexArrayAttribBinding(VAOHandle, 0, 0);
            GL.EnableVertexArrayAttrib(VAOHandle, 0);
            GL.VertexArrayAttribFormat(
                VAOHandle,
                0,
                3,
                VertexAttribType.Float,
                false,
                0);

            // Tex coords
            GL.VertexArrayAttribBinding(VAOHandle, 1, 0);
            GL.EnableVertexArrayAttrib(VAOHandle, 1);
            GL.VertexArrayAttribFormat(
                VAOHandle,
                1,
                2,
                VertexAttribType.Float,
                false,
                12);

            // Associate vertex buffer object with our vertex array object
            GL.VertexArrayVertexBuffer(VAOHandle, 0, VBOHandle, IntPtr.Zero, VertexInformation.Size);

            VertexCount = vertexData.Count;

            vertexData = null;
            //GC.Collect();
            IsReady = true;
        }

        /// <summary>
        /// Render triangle list. This is a very dumb function, it assumes a proper
        /// program with properly-bound uniforms is already active.
        /// This basically just activates all needed buffers and calls a matching triangle
        /// drawing routine.
        /// </summary>
        public void Render()
        {
            if (!IsReady)
                throw new InvalidOperationException("Cant render a triangle list that hasn't been built yet");

            GL.BindVertexArray(VAOHandle);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOHandle);

            // Draw all triangles
            GL.DrawArrays(PrimitiveType.Triangles, 0, VertexCount);
        }
    }
}