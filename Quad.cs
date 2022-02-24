//#define USE_INSTANCING

using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK;
using System;

namespace L2D
{
    public class Quad
    {
        private int vbo, vao, imatrix;

        private float[] Vertices =
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
#if USE_INSTANCING
            CreateQuadWithInstancing();
#else
            CreateQuad();
#endif
        }
        public void CreateQuad()
        {
            vbo = GL.GenBuffer();
            vao = GL.GenVertexArray();
            imatrix = 0;
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);
            GL.BindVertexArray(vao);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);

            GL.EnableVertexAttribArray(0);
            GL.BindVertexArray(0);
        }

        public void CreateQuadWithInstancing()
        {
            vbo = GL.GenBuffer();
            vao = GL.GenVertexArray();
            imatrix = GL.GenBuffer();


            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);
            GL.BindVertexArray(vao);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);


            GL.EnableVertexAttribArray(0);
            RenderState.BindVAO(0);
        }

        public void Draw()
        {
            RenderState.BindVAO(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, Vertices.Length);
        }

        public void UpdateInstancedArray(Matrix4[] modelMatrices)
        {

            RenderState.BindVBO(BufferTarget.ArrayBuffer, imatrix);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 16 * modelMatrices.Length, modelMatrices, BufferUsageHint.DynamicDraw);
            RenderState.BindVAO(vao);


            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 4 * 4 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 4 * 4 * sizeof(float), 1 * 4 * sizeof(float));
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 4 * 4 * sizeof(float), 2 * 4 * sizeof(float));
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(4, 4, VertexAttribPointerType.Float, false, 4 * 4 * sizeof(float), 3 * 4 * sizeof(float));
            GL.EnableVertexAttribArray(4);

            GL.VertexAttribDivisor(1, 1);
            GL.VertexAttribDivisor(2, 1);
            GL.VertexAttribDivisor(3, 1);
            GL.VertexAttribDivisor(4, 1);

            RenderState.BindVAO(0);
            RenderState.BindVBO(BufferTarget.ArrayBuffer, 0);

        }

        public void DrawInstanced(List<Sprite> Sprites)
        {
            Matrix4[] matrixCache = new Matrix4[Sprites.Count];
            int index = 0;
            foreach (Sprite spr in Sprites)
            {
                matrixCache[index] = spr.modelMatrix;
                index += 1;
            }

            UpdateInstancedArray(matrixCache);

            RenderState.BindVAO(vao);

            GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, Vertices.Length, Sprites.Count);
        }
    }
}
