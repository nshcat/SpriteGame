using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace SpriteGame.Rendering.Particles
{
    partial class ParticleSystem : IRenderable, ISimulationObject
    {
        /// <summary>
        /// Information about a single particle
        /// </summary>
        protected struct ParticleData
        {
            /// <summary>
            /// The particle position
            /// </summary>
            private Vector3 _position;

            /// <summary>
            /// The particles lifetime
            /// </summary>
            private float _lifetime;

            /// <summary>
            /// The particle position
            /// </summary>
            public Vector3 Position
            {
                get
                {
                    return this._position;
                }
                set
                {
                    this._position = value;
                }
            }

            /// <summary>
            /// The particles lifetime
            /// </summary>
            public float LifeTime
            {
                get
                {
                    return this._lifetime;
                }
                set
                {
                    this._lifetime = value;
                }
            }

            /// <summary>
            /// Determine the total size of this structure, in bytes.
            /// </summary>
            public static int Size => (3 * sizeof(float)) + (1 * sizeof(float));
        }

        /// <summary>
        /// Number of particles, in total
        /// </summary>
        protected static int NUM_PARTICLES = 64;

        /// <summary>
        /// The VBO containing per-particle data
        /// </summary>
        protected int DataVBO { get; set; }

        /// <summary>
        /// The VBO containing the vertices for a single particle quad
        /// </summary>
        /// <remarks>
        /// The particle quads are drawn using instancing
        /// </remarks>
        protected int VertexVBO { get; set; }

        /// <summary>
        /// The shader program used to render the particles
        /// </summary>
        protected ShaderProgram Program { get; set; }

        /// <summary>
        /// The particle size
        /// </summary>
        protected float ParticleSize { get; set; } = 5f;

        /// <summary>
        /// Per-particle data
        /// </summary>
        /// <remarks>
        /// The first three components are the position in world space, the fourth
        /// component is the life time.
        /// </remarks>
        protected ParticleData[] DataBuffer { get; set; } = new ParticleData[NUM_PARTICLES];
       
        public ParticleSystem()
        {
            this.InitializeParticles();
            this.Initialize();
        }

        /// <summary>
        /// Reset particle at given index
        /// </summary>
        /// <param name="index">Particle index to reset</param>
        protected void ResetParticle(int index)
        {
            var pos = new Vector3(
                        RngUtils.RandomBetween(300f, 330f),
                        RngUtils.RandomBetween(300f, 330f),
                        0f
                    );

            this.DataBuffer[index].Position = pos;
            this.DataBuffer[index].LifeTime = RngUtils.RandomBetween(1f, 3f);
        }

        /// <summary>
        /// Create some initial particle data
        /// </summary>
        protected void InitializeParticles()
        {
            for(int i = 0; i < NUM_PARTICLES; ++i)
            {
                this.ResetParticle(i);
            }
        }

        /// <summary>
        /// Upload current particle data to the GPU
        /// </summary>
        protected void UploadParticleData()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.DataVBO);

            GL.BufferSubData(BufferTarget.ArrayBuffer,
                IntPtr.Zero,
                NUM_PARTICLES * ParticleData.Size,
                this.DataBuffer);
        }

        /// <summary>
        /// Create VBOs and geometry data
        /// </summary>
        protected void Initialize()
        {
            this.Program = new ShaderProgram(
                Shader.FromText(ShaderType.FragmentShader, _fragmentShaderSource),
                Shader.FromText(ShaderType.VertexShader, _vertexShaderSource)
            );

            // Create the geometry VBO
            this.VertexVBO = GL.GenBuffer();

            // Generate quad geometry. We use a triangle strip.
            var vertices = new Vector3[]
            {
                new Vector3(0f, 0f, 0f),
                new Vector3(1f, 0f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(1f, 1f, 0f)
            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.VertexVBO);

            // Fill VBO it with geometry data
            GL.NamedBufferData(
                this.VertexVBO,
                3 * sizeof(float) * vertices.Length,
                vertices,
                BufferUsageHint.StaticDraw);

            // Create particle data VBO
            this.DataVBO = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.DataVBO);

            // Fill VBO it with geometry data
            GL.NamedBufferData(
                this.DataVBO,
                ParticleData.Size * NUM_PARTICLES,
                this.DataBuffer,
                BufferUsageHint.DynamicDraw);
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < NUM_PARTICLES; ++i)
            {
                this.DataBuffer[i].LifeTime -= deltaTime;

                if(this.DataBuffer[i].LifeTime <= 0f)
                {
                    this.ResetParticle(i);
                }

                this.DataBuffer[i].Position -= new Vector3(0f, deltaTime * 1f, 0f);
            }
        }

        public void Render(RenderParams rp)
        {
            this.Program.Use();
            this.Program.Projection = rp.Projection;
            this.Program.View = rp.View;
            this.Program.SetFloat("particle_size", this.ParticleSize);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(4);

            this.UploadParticleData();

            // Vertex buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.VertexVBO);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            // Data buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.DataVBO);
            GL.VertexAttribPointer(4, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.VertexAttribDivisor(4, 1);

            GL.DrawArraysInstanced(PrimitiveType.TriangleStrip, 0, 4, NUM_PARTICLES);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(4);
        }
    }
}
