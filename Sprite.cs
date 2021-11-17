using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace L2D
{
    class Sprite : IDisposable
    {
        public Texture2D Texture;
        private Shader spriteShader;
        private int layer = 0;
        public int Layer { get { return layer; } set { layer = value; UpdateSprite(); } }

        private Vector2 pos;
        private float rot;
        private float sz_x;
        private float sz_y;

        public Vector2 Position { get { return pos; } set { pos = value; UpdateSprite(); } }

        public float Rotation { get { return rot; } set { rot = value; UpdateSprite(); } }

        public float SizeX { get { return sz_x; } set { sz_x = value; UpdateSprite(); } }
        public float SizeY { get { return sz_y; } set { sz_y = value; UpdateSprite(); } }

        private int vbo;
        private int vao;
        private Matrix4 modelMatrix;


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
         
        public void UpdateSprite()
        {
            float rot = Rotation * (MathF.PI / 180.0f);
            Matrix4 scale = Matrix4.CreateScale(SizeX, SizeY, 1.0f);
            Matrix4 rotation = Matrix4.CreateRotationZ(Rotation * (float)(MathF.PI / 180.0f));
            Matrix4 translation = Matrix4.CreateTranslation(Position.X, Position.Y, Layer);

            modelMatrix = scale * rotation * translation;
        }

        public Sprite(float size_x, float size_y, Vector2 initialPosition, float initialRotation, int layer, string texturePath)
        {
            Texture = ContentManager.GetTexture2D(texturePath);
            modelMatrix = Matrix4.Identity;
            SizeX = size_x;
            SizeY = size_y;
            Layer = layer;
            Position = initialPosition;
            Rotation = initialRotation;
            UpdateSprite();
            vbo = GL.GenBuffer();
            vao = GL.GenVertexArray();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindVertexArray(vao);         
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.BindVertexArray(0);
            spriteShader = ShaderManager.get(ShaderManager.ShaderTypeL2D.Sprite, ShaderManager.ShaderFlags.NULL);
            ShaderManager.SetCurrentShader(spriteShader);
        }

        public void SetRotation(float rotation)
        {
            this.Rotation = rotation;
        }
        public void RenderStandard()
        {
            ShaderManager.SetCurrentShader(spriteShader);
            ShaderManager.UseShader();
            ShaderManager.BindModelMatrix(modelMatrix);
            GL.ActiveTexture(TextureUnit.Texture0);
            Texture.BindTexture();
            ShaderManager.GetCurrentShader().BindInt("texture0", 0);
            GL.Enable(EnableCap.AlphaTest);

            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
        }

        public void Dispose()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(vbo);
        }
    }
}
