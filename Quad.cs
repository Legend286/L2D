using OpenTK.Graphics.OpenGL;

namespace L2D
{
    public class Quad
    {
        private int vbo, vao;

        private float[] vertices =
            { 
            // pos      // tex
            -0.5f, 0.5f, 0.0f, 1.0f,
            0.5f, -0.5f, 1.0f, 0.0f,
            -0.5f, -0.5f, 0.0f, 0.0f,

            -0.5f, 0.5f, 0.0f, 1.0f,
            0.5f, 0.5f, 1.0f, 1.0f,
            0.5f, -0.5f, 1.0f, 0.0f
        };

        public Quad()
        {
            CreateQuad();
        }
        public void CreateQuad()
        {
            vbo = GL.GenBuffer();
            vao = GL.GenVertexArray();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindVertexArray(vao);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.BindVertexArray(0);
        }

        public void Draw()
        {
            RenderState.BindVAO(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
        }
    }
}
