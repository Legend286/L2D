using System.ComponentModel;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace L2D
{
    class GameEngine
    {
        public GameWindow window;
        GameWindowSettings gameWindowSettings;
        NativeWindowSettings nativeWindowSettings;

        public GameEngine(int width, int height, float fps, float timestep)
        {
            InitialiseGameWindow(width, height, fps, timestep);

            window.Load += WindowLoad;
            window.RenderFrame += RenderFrame;
            window.UpdateFrame += UpdateFrame;
            window.Closing += WindowClose;
            window.Resize += WindowResize;
        }

        private void WindowResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        private void InitialiseGameWindow(int width, int height, float fps, float timestep)
        {
            gameWindowSettings = new GameWindowSettings();
            gameWindowSettings.UpdateFrequency = timestep;
            gameWindowSettings.RenderFrequency = fps;
            gameWindowSettings.IsMultiThreaded = true;
            nativeWindowSettings = new NativeWindowSettings();
            nativeWindowSettings.Size = new Vector2i(width, height);
            nativeWindowSettings.Title = "L2D Engine";

            this.window = new GameWindow(gameWindowSettings, nativeWindowSettings);
        }

        private void WindowClose(CancelEventArgs e)
        {
        }

        public void Run()
        {
            window.Run();
        }

        private void UpdateFrame(FrameEventArgs e)
        {
        }

        private void RenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Flush();
            window.SwapBuffers();
            window.Title = "L2D Engine - " + (1.0f / e.Time).ToString("0.") + " fps.";
        }

        private void WindowLoad()
        {
            GL.ClearColor(Color4.CornflowerBlue);
        }
    }
}
