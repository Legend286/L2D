using OpenTK.Graphics.OpenGL;

namespace L2D
{
    public class Texture2D
    {
        private string filename = "";
        private int id = -1;

        public Texture2D(string filename)
        {
            this.filename = filename;
            id = GL.GenTexture();
        }

        public int GetTextureID()
        {
            return id;
        }

        public void BindTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, id);
        }
    }
}
