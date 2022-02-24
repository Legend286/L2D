using ImGuiNET;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace L2D
{
    /// <summary>
    /// A modified version of Veldrid.ImGui's ImGuiRenderer.
    /// Manages input for ImGui and handles rendering ImGui's DrawLists with Veldrid.
    /// </summary>
    public class ImGuiController : IDisposable
    {
        private bool _frameBegun;

        // Veldrid objects
        private int _vertexArray;
        private int _vertexBuffer;
        private int _vertexBufferSize;
        private int _indexBuffer;
        private int _indexBufferSize;

        private ImGuiTexture _fontTexture;
        private Shader _shader;

        private int _windowWidth;
        private int _windowHeight;

        private System.Numerics.Vector2 _scaleFactor = System.Numerics.Vector2.One;

        /// <summary>
        /// Constructs a new ImGuiController.
        /// </summary>
        public ImGuiController(int width, int height)
        {
            _windowWidth = width;
            _windowHeight = height;

            IntPtr context = ImGui.CreateContext();
            ImGui.SetCurrentContext(context);
            var io = ImGui.GetIO();
            // io.Fonts.AddFontDefault();

            io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;

            CreateDeviceResources();
            SetKeyMappings();

            SetPerFrameImGuiData(1f / 60f);

            ImGui.NewFrame();
            _frameBegun = true;
        }

        public void WindowResized(int width, int height)
        {
            _windowWidth = width;
            _windowHeight = height;
        }

        public void DestroyDeviceObjects()
        {
            Dispose();
        }

        public void CreateSkin()
        {
            ImGuiStylePtr style = ImGui.GetStyle();

            style.WindowPadding = new System.Numerics.Vector2(15, 15);
            style.WindowRounding = 5.0f;
            style.FramePadding = new System.Numerics.Vector2(5, 5);
            style.FrameRounding = 4.0f;
            style.ItemSpacing = new System.Numerics.Vector2(12, 8);
            style.ItemInnerSpacing = new System.Numerics.Vector2(8, 6);
            style.IndentSpacing = 25.0f;
            style.ScrollbarSize = 15.0f;
            style.ScrollbarRounding = 9.0f;
            style.GrabMinSize = 5.0f;
            style.GrabRounding = 3.0f;


            style.Colors[(int)(int)ImGuiCol.Text] = new System.Numerics.Vector4(1f, 1f, 1f, 1.00f);
            style.Colors[(int)ImGuiCol.TextDisabled] = new System.Numerics.Vector4(0.24f, 0.23f, 0.29f, 1.00f);
            style.Colors[(int)ImGuiCol.WindowBg] = new System.Numerics.Vector4(0.07f, 0.06f, 0.08f, 1.00f);
            //  style.Colors[(int)ImGuiCol.ChildWindowBg] = new System.Numerics.Vector4(0.07f, 0.07f, 0.09f, 1.00f);
            style.Colors[(int)ImGuiCol.PopupBg] = new System.Numerics.Vector4(0.07f, 0.07f, 0.09f, 1.00f);
            style.Colors[(int)ImGuiCol.Border] = new System.Numerics.Vector4(0.80f, 0.80f, 0.83f, 0.88f);
            style.Colors[(int)ImGuiCol.BorderShadow] = new System.Numerics.Vector4(0.92f, 0.91f, 0.88f, 0.00f);
            style.Colors[(int)ImGuiCol.FrameBg] = new System.Numerics.Vector4(0.10f, 0.09f, 0.12f, 1.00f);
            style.Colors[(int)ImGuiCol.FrameBgHovered] = new System.Numerics.Vector4(0.24f, 0.23f, 0.29f, 1.00f);
            style.Colors[(int)ImGuiCol.FrameBgActive] = new System.Numerics.Vector4(0.56f, 0.56f, 0.58f, 1.00f);
            style.Colors[(int)ImGuiCol.TitleBg] = new System.Numerics.Vector4(0.10f, 0.09f, 0.12f, 1.00f);
            style.Colors[(int)ImGuiCol.TitleBgCollapsed] = new System.Numerics.Vector4(0.2f, 0.18f, 0.24f, 0.55f);
            style.Colors[(int)ImGuiCol.TitleBgActive] = new System.Numerics.Vector4(0.07f, 0.07f, 0.09f, 1.00f);
            style.Colors[(int)ImGuiCol.MenuBarBg] = new System.Numerics.Vector4(0.10f, 0.09f, 0.12f, 1.00f);
            style.Colors[(int)ImGuiCol.ScrollbarBg] = new System.Numerics.Vector4(0.10f, 0.09f, 0.12f, 1.00f);
            style.Colors[(int)ImGuiCol.ScrollbarGrab] = new System.Numerics.Vector4(0.80f, 0.80f, 0.83f, 0.31f);
            style.Colors[(int)ImGuiCol.ScrollbarGrabHovered] = new System.Numerics.Vector4(0.56f, 0.56f, 0.58f, 1.00f);
            style.Colors[(int)ImGuiCol.ScrollbarGrabActive] = new System.Numerics.Vector4(0.06f, 0.05f, 0.07f, 1.00f);
            // style.Colors[(int)ImGuiCol.ComboBg] = new System.Numerics.Vector4(0.19f, 0.18f, 0.21f, 1.00f);
            style.Colors[(int)ImGuiCol.CheckMark] = new System.Numerics.Vector4(0.80f, 0.80f, 0.83f, 0.31f);
            style.Colors[(int)ImGuiCol.SliderGrab] = new System.Numerics.Vector4(0.80f, 0.80f, 0.83f, 0.31f);
            style.Colors[(int)ImGuiCol.SliderGrabActive] = new System.Numerics.Vector4(0.06f, 0.05f, 0.07f, 1.00f);
            style.Colors[(int)ImGuiCol.Button] = new System.Numerics.Vector4(0.10f, 0.09f, 0.12f, 1.00f);
            style.Colors[(int)ImGuiCol.ButtonHovered] = new System.Numerics.Vector4(0.24f, 0.23f, 0.29f, 1.00f);
            style.Colors[(int)ImGuiCol.ButtonActive] = new System.Numerics.Vector4(0.56f, 0.56f, 0.58f, 1.00f);
            style.Colors[(int)ImGuiCol.Header] = new System.Numerics.Vector4(0.10f, 0.09f, 0.12f, 1.00f);
            style.Colors[(int)ImGuiCol.HeaderHovered] = new System.Numerics.Vector4(0.56f, 0.56f, 0.58f, 1.00f);
            style.Colors[(int)ImGuiCol.HeaderActive] = new System.Numerics.Vector4(0.06f, 0.05f, 0.07f, 1.00f);
            //   style.Colors[(int)ImGuiCol.Column] = new System.Numerics.Vector4(0.56f, 0.56f, 0.58f, 1.00f);
            //   style.Colors[(int)ImGuiCol.ColumnHovered] = new System.Numerics.Vector4(0.24f, 0.23f, 0.29f, 1.00f);
            //  style.Colors[(int)ImGuiCol.ColumnActive] = new System.Numerics.Vector4(0.56f, 0.56f, 0.58f, 1.00f);
            style.Colors[(int)ImGuiCol.ResizeGrip] = new System.Numerics.Vector4(0.00f, 0.00f, 0.00f, 0.00f);
            style.Colors[(int)ImGuiCol.ResizeGripHovered] = new System.Numerics.Vector4(0.56f, 0.56f, 0.58f, 1.00f);
            style.Colors[(int)ImGuiCol.ResizeGripActive] = new System.Numerics.Vector4(0.06f, 0.05f, 0.07f, 1.00f);
            //  style.Colors[(int)ImGuiCol.CloseButton] = new System.Numerics.Vector4(0.40f, 0.39f, 0.38f, 0.16f);
            //  style.Colors[(int)ImGuiCol.CloseButtonHovered] = new System.Numerics.Vector4(0.40f, 0.39f, 0.38f, 0.39f);
            // style.Colors[(int)ImGuiCol.CloseButtonActive] = new System.Numerics.Vector4(0.40f, 0.39f, 0.38f, 1.00f);
            style.Colors[(int)ImGuiCol.PlotLines] = new System.Numerics.Vector4(0.40f, 0.39f, 0.38f, 0.63f);
            style.Colors[(int)ImGuiCol.PlotLinesHovered] = new System.Numerics.Vector4(0.25f, 1.00f, 0.00f, 1.00f);
            style.Colors[(int)ImGuiCol.PlotHistogram] = new System.Numerics.Vector4(0.40f, 0.39f, 0.38f, 0.63f);
            style.Colors[(int)ImGuiCol.PlotHistogramHovered] = new System.Numerics.Vector4(0.25f, 1.00f, 0.00f, 1.00f);
            style.Colors[(int)ImGuiCol.TextSelectedBg] = new System.Numerics.Vector4(0.25f, 1.00f, 0.00f, 0.43f);
            //  style.Colors[(int)ImGuiCol.ModalWindowDarkening] = new System.Numerics.Vector4(1.00f, 0.98f, 0.95f, 0.73f);

        }

        void setShaderFlags(ShaderManager.ShaderFlags flags)
        {
            _shader = ShaderManager.get(ShaderManager.ShaderTypeL2D.ImGui, flags);
        }

        public void CreateDeviceResources()
        {
            Util.CreateVertexArray("ImGui", out _vertexArray);

            _vertexBufferSize = 10000;
            _indexBufferSize = 2000;

            Util.CreateVertexBuffer("ImGui", out _vertexBuffer);
            Util.CreateElementBuffer("ImGui", out _indexBuffer);
            GL.NamedBufferData(_vertexBuffer, _vertexBufferSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
            GL.NamedBufferData(_indexBuffer, _indexBufferSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);

            RecreateFontDeviceTexture();

            string VertexSource = "Shaders/ImGui.vert";//      FIXED 04/04/20 17:31 GMT+1
            string FragmentSource = "Shaders/ImGui.frag";//    FIXED 04/04/20 17:31 GMT+1

            _shader = new Shader("ImGui", VertexSource, FragmentSource);

            GL.VertexArrayVertexBuffer(_vertexArray, 0, _vertexBuffer, IntPtr.Zero, Unsafe.SizeOf<ImDrawVert>());
            GL.VertexArrayElementBuffer(_vertexArray, _indexBuffer);

            GL.EnableVertexArrayAttrib(_vertexArray, 0);
            GL.VertexArrayAttribBinding(_vertexArray, 0, 0);
            GL.VertexArrayAttribFormat(_vertexArray, 0, 2, VertexAttribType.Float, false, 0);

            GL.EnableVertexArrayAttrib(_vertexArray, 1);
            GL.VertexArrayAttribBinding(_vertexArray, 1, 0);
            GL.VertexArrayAttribFormat(_vertexArray, 1, 2, VertexAttribType.Float, false, 8);

            GL.EnableVertexArrayAttrib(_vertexArray, 2);
            GL.VertexArrayAttribBinding(_vertexArray, 2, 0);
            GL.VertexArrayAttribFormat(_vertexArray, 2, 4, VertexAttribType.UnsignedByte, true, 16);

            Util.CheckGLError("End of ImGui setup");
        }

        /// <summary>
        /// Recreates the device texture used to render text.
        /// </summary>
        public void RecreateFontDeviceTexture()
        {
            ImGuiIOPtr io = ImGui.GetIO();
            io.Fonts.GetTexDataAsRGBA32(out IntPtr pixels, out int width, out int height, out int bytesPerPixel);

            _fontTexture = new ImGuiTexture("ImGui Text Atlas", width, height, pixels);
            _fontTexture.SetMagFilter(TextureMagFilter.Linear);
            _fontTexture.SetMinFilter(TextureMinFilter.Linear);

            io.Fonts.SetTexID((IntPtr)_fontTexture.GLTexture);

            io.Fonts.ClearTexData();
        }

        /// <summary>
        /// Renders the ImGui draw list data.
        /// This method requires a <see cref="GraphicsDevice"/> because it may create new DeviceBuffers if the size of vertex
        /// or index data has increased beyond the capacity of the existing buffers.
        /// A <see cref="CommandList"/> is needed to submit drawing and resource update commands.
        /// </summary>
        public void Render()
        {
            if (_frameBegun)
            {
                _frameBegun = false;
                RenderState.BindVAO(0);
                RenderState.BindVBO(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, 0);
                ImGui.Render();
                RenderImDrawData(ImGui.GetDrawData());
            }
        }

        /// <summary>
        /// Updates ImGui input and IO configuration state.
        /// </summary>
        public void Update(GameWindow wnd, float deltaSeconds)
        {
            if (_frameBegun)
            {
                ImGui.Render();
            }

            SetPerFrameImGuiData(deltaSeconds);
            UpdateImGuiInput(wnd);

            _frameBegun = true;
            ImGui.NewFrame();
        }

        /// <summary>
        /// Sets per-frame data based on the associated window.
        /// This is called by Update(float).
        /// </summary>
        private void SetPerFrameImGuiData(float deltaSeconds)
        {
            ImGuiIOPtr io = ImGui.GetIO();
            io.DisplaySize = new System.Numerics.Vector2(
                _windowWidth / _scaleFactor.X,
                _windowHeight / _scaleFactor.Y);
            io.DisplayFramebufferScale = _scaleFactor;
            io.DeltaTime = deltaSeconds; // DeltaTime is in seconds.
        }

        MouseState PrevMouseState;
        KeyboardState PrevKeyboardState;
        readonly List<char> PressedChars = new List<char>();

        private void UpdateImGuiInput(GameWindow wnd)
        {
            ImGuiIOPtr io = ImGui.GetIO();

            MouseState MouseState = Mouse.GetCursorState();
            KeyboardState KeyboardState = Keyboard.GetState();

            io.MouseDown[0] = MouseState.LeftButton == ButtonState.Pressed;
            io.MouseDown[1] = MouseState.RightButton == ButtonState.Pressed;
            io.MouseDown[2] = MouseState.MiddleButton == ButtonState.Pressed;

            var screenPoint = new System.Drawing.Point(MouseState.X, MouseState.Y);
            var point = wnd.PointToClient(screenPoint);
            io.MousePos = new System.Numerics.Vector2(point.X, point.Y);

            io.MouseWheel = MouseState.Scroll.Y - PrevMouseState.Scroll.Y;
            io.MouseWheelH = MouseState.Scroll.X - PrevMouseState.Scroll.X;

            foreach (Key key in Enum.GetValues(typeof(Key)))
            {
                io.KeysDown[(int)key] = KeyboardState.IsKeyDown(key);
            }

            foreach (var c in PressedChars)
            {
                io.AddInputCharacter(c);
            }
            PressedChars.Clear();

            io.KeyCtrl = KeyboardState.IsKeyDown(Key.ControlLeft) || KeyboardState.IsKeyDown(Key.ControlRight);
            io.KeyAlt = KeyboardState.IsKeyDown(Key.AltLeft) || KeyboardState.IsKeyDown(Key.AltRight);
            io.KeyShift = KeyboardState.IsKeyDown(Key.ShiftLeft) || KeyboardState.IsKeyDown(Key.ShiftRight);
            io.KeySuper = KeyboardState.IsKeyDown(Key.WinLeft) || KeyboardState.IsKeyDown(Key.WinRight);

            PrevMouseState = MouseState;
            PrevKeyboardState = KeyboardState;
        }


        internal void PressChar(char keyChar)
        {
            PressedChars.Add(keyChar);
        }

        private static void SetKeyMappings()
        {
            ImGuiIOPtr io = ImGui.GetIO();
            io.KeyMap[(int)ImGuiKey.Tab] = (int)Key.Tab;
            io.KeyMap[(int)ImGuiKey.LeftArrow] = (int)Key.Left;
            io.KeyMap[(int)ImGuiKey.RightArrow] = (int)Key.Right;
            io.KeyMap[(int)ImGuiKey.UpArrow] = (int)Key.Up;
            io.KeyMap[(int)ImGuiKey.DownArrow] = (int)Key.Down;
            io.KeyMap[(int)ImGuiKey.PageUp] = (int)Key.PageUp;
            io.KeyMap[(int)ImGuiKey.PageDown] = (int)Key.PageDown;
            io.KeyMap[(int)ImGuiKey.Home] = (int)Key.Home;
            io.KeyMap[(int)ImGuiKey.End] = (int)Key.End;
            io.KeyMap[(int)ImGuiKey.Delete] = (int)Key.Delete;
            io.KeyMap[(int)ImGuiKey.Backspace] = (int)Key.BackSpace;
            io.KeyMap[(int)ImGuiKey.Enter] = (int)Key.Enter;
            io.KeyMap[(int)ImGuiKey.Escape] = (int)Key.Escape;
            io.KeyMap[(int)ImGuiKey.A] = (int)Key.A;
            io.KeyMap[(int)ImGuiKey.C] = (int)Key.C;
            io.KeyMap[(int)ImGuiKey.V] = (int)Key.V;
            io.KeyMap[(int)ImGuiKey.X] = (int)Key.X;
            io.KeyMap[(int)ImGuiKey.Y] = (int)Key.Y;
            io.KeyMap[(int)ImGuiKey.Z] = (int)Key.Z;
        }

        private void RenderImDrawData(ImDrawDataPtr draw_data)
        {
            uint vertexOffsetInVertices = 0;
            uint indexOffsetInElements = 0;

            if (draw_data.CmdListsCount == 0)
            {
                return;
            }

            uint totalVBSize = (uint)(draw_data.TotalVtxCount * Unsafe.SizeOf<ImDrawVert>());
            if (totalVBSize > _vertexBufferSize)
            {
                int newSize = (int)Math.Max(_vertexBufferSize * 1.5f, totalVBSize);
                GL.NamedBufferData(_vertexBuffer, newSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
                _vertexBufferSize = newSize;

                Console.WriteLine($"Resized vertex buffer to new size {_vertexBufferSize}");
            }

            uint totalIBSize = (uint)(draw_data.TotalIdxCount * sizeof(ushort));
            if (totalIBSize > _indexBufferSize)
            {
                int newSize = (int)Math.Max(_indexBufferSize * 1.5f, totalIBSize);
                GL.NamedBufferData(_indexBuffer, newSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
                _indexBufferSize = newSize;

                Console.WriteLine($"Resized index buffer to new size {_indexBufferSize}");
            }


            for (int i = 0; i < draw_data.CmdListsCount; i++)
            {
                ImDrawListPtr cmd_list = draw_data.CmdListsRange[i];

                GL.NamedBufferSubData(_vertexBuffer, (IntPtr)(vertexOffsetInVertices * Unsafe.SizeOf<ImDrawVert>()), cmd_list.VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>(), cmd_list.VtxBuffer.Data);
                Util.CheckGLError($"Data Vert {i}");
                GL.NamedBufferSubData(_indexBuffer, (IntPtr)(indexOffsetInElements * sizeof(ushort)), cmd_list.IdxBuffer.Size * sizeof(ushort), cmd_list.IdxBuffer.Data);

                Util.CheckGLError($"Data Idx {i}");

                vertexOffsetInVertices += (uint)cmd_list.VtxBuffer.Size;
                indexOffsetInElements += (uint)cmd_list.IdxBuffer.Size;
            }

            // Setup orthographic projection matrix into our constant buffer
            ImGuiIOPtr io = ImGui.GetIO();
            Matrix4 mvp = Matrix4.CreateOrthographicOffCenter(
                -1.0f,
                io.DisplaySize.X,
                io.DisplaySize.Y,
                0.0f,
                -1.0f,
                1.0f);

            ShaderManager.SetCurrentShader(_shader);
            ShaderManager.UseShader();
            GL.ProgramUniformMatrix4(_shader.Program, _shader.GetUniformLocation("projection_matrix"), false, ref mvp);
            GL.ProgramUniform1(_shader.Program, _shader.GetUniformLocation("in_fontTexture"), 0);
            Util.CheckGLError("Projection");

            GL.BindVertexArray(_vertexArray);
            Util.CheckGLError("VAO");

            draw_data.ScaleClipRects(io.DisplayFramebufferScale);

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.ScissorTest);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);

            // Render command lists
            int vtx_offset = 0;
            int idx_offset = 0;
            for (int n = 0; n < draw_data.CmdListsCount; n++)
            {
                ImDrawListPtr cmd_list = draw_data.CmdListsRange[n];
                for (int cmd_i = 0; cmd_i < cmd_list.CmdBuffer.Size; cmd_i++)
                {
                    ImDrawCmdPtr pcmd = cmd_list.CmdBuffer[cmd_i];
                    if (pcmd.UserCallback != IntPtr.Zero)
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        GL.ActiveTexture(TextureUnit.Texture0);
                        GL.BindTexture(TextureTarget.Texture2D, (int)pcmd.TextureId);
                        Util.CheckGLError("Texture");

                        // We do _windowHeight - (int)clip.W instead of (int)clip.Y because gl has flipped Y when it comes to these coordinates
                        var clip = pcmd.ClipRect;
                        GL.Scissor((int)clip.X, _windowHeight - (int)clip.W, (int)(clip.Z - clip.X), (int)(clip.W - clip.Y));
                        Util.CheckGLError("Scissor");

                        GL.DrawElementsBaseVertex(PrimitiveType.Triangles, (int)pcmd.ElemCount, DrawElementsType.UnsignedShort, (IntPtr)(idx_offset * sizeof(ushort)), vtx_offset);
                        Util.CheckGLError("Draw");
                    }

                    idx_offset += (int)pcmd.ElemCount;
                }
                vtx_offset += cmd_list.VtxBuffer.Size;
            }

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.ScissorTest);
        }

        /// <summary>
        /// Frees all graphics resources used by the renderer.
        /// </summary>
        public void Dispose()
        {
            _fontTexture.Dispose();
            _shader.Dispose();
        }
    }
}