using OpenTK.Graphics.OpenGL;

namespace L2D
{
    class Texture2D
    {
        string filename = "";
        int id = -1;

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
