using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System;
using ImGuiNET;
using Box2DX.Dynamics;
using Box2DX.Collision;

namespace L2D
{
    public class GameEngine : GameWindow
    {
        ImGuiController _imguicontroller;
        static int SPRITES_X = 100;
        static int SPRITES_Y = 50;
        Sprite[,] sprites = new Sprite[SPRITES_X, SPRITES_Y];
        Camera camera;

        public GameEngine(int width, int height) : base(width, height, GraphicsMode.Default)
        {
            Width = width;
            Height = height;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
            _imguicontroller.WindowResized(Width, Height);
            camera.OrthographicHeight = Height;
            camera.OrthographicWidth = Width;
        }
        float scaleup = 1.0f;
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            Physics2DWorld.UpdatePhysicsWorld((float)e.Time);
            ShaderManager.UpdateCurrentShader();
        }
        float timer = 0;
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            // update imgui controller for rendering
            _imguicontroller.Update(this, (float)e.Time);

            // Clear colour and depth :)
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Render everything between here
            int count = SpriteRenderer.GetCount();
            ImGui.Begin("test");
            ImGui.Text("Hello and welcome to my silly psyduck sprite test,\nFeaturing " + count + " psyduck sprites :)");
            ImGui.SliderFloat("Spacing", ref scaleup, 0.5f, 100.0f);
          //  ImGui.Text("Number of Sprites: " + count);
            ImGui.End();
            timer += (float)e.Time;

            for (int i = 0; i < SPRITES_X; i++)
                for (int j = 0; j < SPRITES_Y; j++)
                {
                    {
                        float size = (MathF.Abs(MathF.Sin(timer + (float)(i + j)) * 32f) + 32f);
                        sprites[i, j].SetRotation(45f);

                        sprites[i, j].Scale = size;
                        sprites[i, j].Rotation = MathF.Sin( timer + (float)(i * j) ) * 45.0f + 180.0f;
                        sprites[i, j].Position = new Vector2((i - SPRITES_X / 2) * 32.0f * scaleup, (j - SPRITES_Y / 2) * 32.0f * scaleup);
                    }
                }

            SpriteRenderer.Render();

            // and here
            
            // Render ui last
            _imguicontroller.Render();

            // flush and swap
            GL.Flush();
            this.SwapBuffers();


            // set title
            this.Title = "L2D Engine - " + (1.0f / e.Time).ToString("0.") + " fps - " + (e.Time * 1000).ToString("0.") + " ms.";
        }



        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Physics2DWorld.InitialiseWorld(new Vector2(0, -9.8f), new Vector2(-100f, -100f), new Vector2(100f, 100f));
            _imguicontroller = new ImGuiController(Width, Height);
            _imguicontroller.CreateSkin();
            GL.ClearColor(Color4.CornflowerBlue);
            GL.Disable(EnableCap.CullFace);
            for (int i = 0; i < SPRITES_X; i++)
                for (int j = 0; j < SPRITES_Y; j++)
                {
                    {
                        sprites[i, j] = new Sprite(new Vector2((i - SPRITES_X / 2) * 32.0f, (j - SPRITES_Y / 2) * 32.0f), 0, 50.0f, 0, "test.png");
                      
                    }
                }

            BodyDef bd = new BodyDef();
            bd.Angle = 1.0f;

            Physics2DBody body = new Physics2DBody(bd);
            Physics2DWorld.AddBodyToWorld(body);

            camera = new Camera(Width, Height, 0.0f, 1000.0f);
            camera.Position = new Vector3(0, 0, 0);

       
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            _imguicontroller.PressChar(e.KeyChar);
        }
    }
}
