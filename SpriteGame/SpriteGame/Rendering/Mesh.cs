using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace SpriteGame.Rendering
{
    /// <summary>
    /// Mesh operation mode
    /// </summary>
    enum MeshRenderMode
    {
        /// <summary>
        /// Direct rendering of vertices in VBO
        /// </summary>
        Direct,

        /// <summary>
        /// Mesh will be rendered using index buffer
        /// </summary>
        Indexed
    }

    /// <summary>
    /// Mesh primitive mode
    /// </summary>
    enum MeshPrimitiveMode
    {
        /// <summary>
        /// Simple triangles, one after each other
        /// </summary>
        Triangles,

        /// <summary>
        /// Strip of triangles, where each one shares the last two vertices
        /// of the previous one
        /// </summary>
        TriangleStrip,

        /// <summary>
        /// Fan of triangles, where each one shares the first vertex
        /// </summary>
        TriangleFan,

        /// <summary>
        /// Simple line segments, each having their own vertices
        /// </summary>
        Lines,

        /// <summary>
        /// Strip of lines, each one sharing the previous ones last vertex
        /// </summary>
        LineStrip
    }

    /// <summary>
    /// A simple class abstracting the concept of a vertex buffer (and an index buffer)
    /// containing triangle data. It allows easy creation of models by adding triangles
    /// one by one.
    /// </summary>
    /// <typeparam name="T">Vertex type, must be a struct</typeparam>
    /// TODO:
    /// - Primitive restart, maybe with Optional<int>
    /// - Wireframe and line width support
    class Mesh<T> where T : struct, IVertex<T>
    {
        /// <summary>
        /// Temporary list of vertex data used to build the triangle list.
        /// </summary>
        protected List<T> vertexData = new List<T>();

        /// <summary>
        /// Index buffer data. Only used if <see cref="RenderMode"/> is set to <see cref="MeshRenderMode.Indexed"/>.
        /// </summary>
        protected List<int> indices = new List<int>();

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
        /// Native OpenGL handle of the vertex index buffer
        /// </summary>
        protected int VIBHandle { get; set; }

        /// <summary>
        /// Number of vertices contained in the list
        /// </summary>
        protected int VertexCount { get; set; }

        /// <summary>
        /// Mesh primitive mode
        /// </summary>
        public MeshPrimitiveMode PrimitiveMode { get; protected set; }

        /// <summary>
        /// Mesh render mode
        /// </summary>
        public MeshRenderMode RenderMode { get; protected set; }

        /// <summary>
        /// Create new, empty mesh.
        /// </summary>
        /// <param name="primitiveMode">Primitive rendering mode to use</param>
        /// <param name="renderMode">Base rendering mode to use</param>
        public Mesh(MeshPrimitiveMode primitiveMode, MeshRenderMode renderMode = MeshRenderMode.Direct)
        {
            this.PrimitiveMode = primitiveMode;
            this.RenderMode = renderMode;
        }

        /// <summary>
        /// Add a new vertex to the list. The list needs to be in unbuild state.
        /// </summary>
        /// <param name="vertex">Vertex to add</param>
        public void AddVertex(T vertex)
        {
            if (IsReady)
                throw new InvalidOperationException("Cannot add vertex to mesh that has already been built");

            vertexData.Add(vertex);
        }

        /// <summary>
        /// Add a new vertex index to the mesh. Only usable if the mesh is configured for
        /// indexed rendering by setting <see cref="RenderMode"/> to <see cref="MeshRenderMode.Indexed"/>.
        /// </summary>
        /// <param name="index">Vertex index to add</param>
        public void AddIndex(int index)
        {
            if (this.RenderMode != MeshRenderMode.Indexed)
                throw new InvalidOperationException("Mesh is not indexed");

            if (IsReady)
                throw new InvalidOperationException("Cannot add index to mesh that has already been built");

            indices.Add(index);
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
            if (this.vertexData.Count < 3)
                throw new InvalidOperationException("Can not build triangle list with less than three vertices!");

            // Create the VAO
            VAOHandle = GL.GenVertexArray();
            GL.BindVertexArray(VAOHandle);

            // Create the VBO
            VBOHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOHandle);

            // Fill it with our data
            GL.NamedBufferData(
                VBOHandle,
                this.vertexData[0].Size * vertexData.Count,
                vertexData.ToArray(),
                BufferUsageHint.StaticDraw);

            // Set up vertex attribute pointers
            // Position
            int accumulatedLength = 0;
            int currentAttrib = 0;

            var descriptors = this.vertexData[0].Attributes;

            foreach(var attrib in descriptors)
            {
                GL.VertexArrayAttribBinding(VAOHandle, currentAttrib, 0);
                GL.EnableVertexArrayAttrib(VAOHandle, currentAttrib);
                GL.VertexArrayAttribFormat(
                    VAOHandle,
                    currentAttrib,
                    attrib.Length,
                    attrib.AttributeType,
                    false,
                    accumulatedLength);

                accumulatedLength += attrib.LengthInBytes;
                ++currentAttrib;
            }

            // Associate vertex buffer object with our vertex array object
            GL.VertexArrayVertexBuffer(VAOHandle, 0, VBOHandle, IntPtr.Zero, vertexData[0].Size);

            // Create vertex index buffer, if needed
            if(this.RenderMode == MeshRenderMode.Indexed)
            {
                this.VIBHandle = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.VIBHandle);

                GL.BufferData(BufferTarget.ElementArrayBuffer,
                    this.indices.Count * sizeof(int),
                    this.indices.ToArray(),
                    BufferUsageHint.StaticDraw);
            }

            VertexCount = vertexData.Count;

            vertexData = null;
            IsReady = true;
        }


        /// <summary>
        /// Convert instance of type <see cref="MeshPrimitiveMode"/> to one of type <see cref="PrimitiveType"/>
        /// </summary>
        protected PrimitiveType ConvertPrimitiveType(MeshPrimitiveMode mode)
        {
            switch(mode)
            {
                case MeshPrimitiveMode.TriangleFan:
                    return PrimitiveType.TriangleFan;
                case MeshPrimitiveMode.Triangles:
                    return PrimitiveType.Triangles;
                case MeshPrimitiveMode.TriangleStrip:
                    return PrimitiveType.TriangleStrip;
                default:
                    throw new ArgumentException("Invalid primitive mode");
            }
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

            var primitiveType = this.ConvertPrimitiveType(this.PrimitiveMode);

            if(this.RenderMode == MeshRenderMode.Indexed)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.VIBHandle);
                GL.DrawElements(primitiveType, this.indices.Count, DrawElementsType.UnsignedInt, 0);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            }
            else
            {
                GL.DrawArrays(primitiveType, 0, VertexCount);
            }
            
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
    }
}