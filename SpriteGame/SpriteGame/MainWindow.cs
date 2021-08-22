using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using SpriteGame.Rendering;
using SpriteGame.Rendering.Sprites;
using SpriteGame.Rendering.Particles;
/// <summary>
///  Irgendwie irgendwas zum testen oder so
/// </summary>
namespace SpriteGame
{
    public sealed class MainWindow : GameWindow
    {
        SpriteSheet _sheet;
        OrthographicProjection _projection;
        Sprite _sprite;
        ParticleSystem _system;

        public MainWindow()
            : base( GameWindowSettings.Default,
                    new NativeWindowSettings
                    {
                        NumberOfSamples = 16,
                        Title = "SpriteGame",
                        Size = new Vector2i(1240, 720)
                    })
        {

        }

        /// <summary>
        /// Called when the application is starting
        /// </summary>
        protected override void OnLoad()
        {
            // Set background color to black
            GL.ClearColor(Color4.Black);

            // Enable transparency
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            this._sheet = new SpriteSheet("sheet2.png", 64, 64);
            this._projection = new OrthographicProjection();
            this._sprite = new Sprite(this._sheet) { SpriteIndex = 0 };
            this._system = new ParticleSystem();

            base.OnLoad();
        }

        /// <summary>
        /// Called whenever the window size changes
        /// </summary>
        protected override void OnResize(ResizeEventArgs e)
        {
            // Resize OpenGL viewport to fit the new screen size
            GL.Viewport(0, 0, e.Width, e.Height);
            this._projection.Refresh(e.Width, e.Height);
            base.OnResize(e);
        }

        /// <summary>
        /// Called whenever input needs to be processed
        /// </summary>
        /// <param name="args"></param>
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            this._system.Update((float)args.Time);

            base.OnUpdateFrame(args);
        }

        /// <summary>
        /// Called whenever a new frame needs to be rendered
        /// </summary>
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            // Clear the window with black
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var renderParams = this._projection.Params;
            renderParams.Translate(100.0f, 100.0f, 0.0f);

            //this._system.Render(renderParams);
            this._sprite.Render(renderParams);
 
            // Present frame
            Context.SwapBuffers();

            base.OnRenderFrame(args);
        }
    }
}
