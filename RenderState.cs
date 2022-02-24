#define RENDER_STATE_FIX

using OpenTK.Graphics.OpenGL;

namespace L2D
{
    public static class RenderState
    {
        static int CurrentVertexArray = -1;
        static int CurrentVertexBuffer = -1;

        public static bool BindVAO(int id)
        {
            if(id == CurrentVertexArray)
            {
                return false;
            }
            else
            {
                GL.BindVertexArray(id);
                CurrentVertexArray = id;
                return true;
            }
        }

        public static bool BindVBO(BufferTarget target, int id)
        {
#if RENDER_STATE_FIX
            if (id == CurrentVertexBuffer)
            {
                return false;
            }
            else
            {
                GL.BindBuffer(target, id);
                CurrentVertexBuffer = id;
                return true;
            }
#else
            GL.BindBuffer(target, id);
            return true;
#endif
        }
    }
}
