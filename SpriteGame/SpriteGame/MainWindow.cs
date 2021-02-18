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

namespace SpriteGame
{
    public sealed class MainWindow : GameWindow
    {
        public MainWindow()
            : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {

        }

        /// <summary>
        /// Called when the application is starting
        /// </summary>
        protected override void OnLoad()
        {
            // Set background color to black
            GL.ClearColor(Color4.Black);

            base.OnLoad();
        }

        /// <summary>
        /// Called whenever the window size changes
        /// </summary>
        protected override void OnResize(ResizeEventArgs e)
        {
            // Resize OpenGL viewport to fit the new screen size
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        /// <summary>
        /// Called whenever input needs to be processed
        /// </summary>
        /// <param name="args"></param>
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        /// <summary>
        /// Called whenever a new frame needs to be rendered
        /// </summary>
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            // Clear the window with black
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // XXX Do actual rendering here

            // Present frame
            Context.SwapBuffers();

            base.OnRenderFrame(args);
        }
    }
}
