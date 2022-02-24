using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace L2D
{
    public class Sprite : GameObject, IDisposable
    {
        public Texture2D Texture;
        private Shader spriteShader;
        private int layer = 0;
        public int Layer { get { return layer; } set { layer = value; UpdateTransform(); } }

        Quad Q = new Quad();
        private Matrix4 modelMatrix;


         
        public override void UpdateTransform()
        {
            Matrix4 sz = Matrix4.CreateScale(Scale, Scale, 1.0f);
            Matrix4 rot = Matrix4.CreateRotationZ(Rotation * (float)(MathF.PI / 180.0f));
            Matrix4 trans = Matrix4.CreateTranslation(Position.X, Position.Y, Layer);

            modelMatrix = sz * rot * trans;
        }

        public Sprite(Vector2 pos, float rot, float sz, int layer, string texturePath) : base(pos, rot, sz)
        {
            Texture = ContentManager.GetTexture2D(texturePath);
            modelMatrix = Matrix4.Identity;
            Scale = sz;
            Layer = layer;
            Position = pos;
            Rotation = rot;
            UpdateTransform();
          
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
            Q.Draw();
        }

        public void Dispose()
        {
            SpriteRenderer.RemoveSprite(this);
        }
    }
}
