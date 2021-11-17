using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;


namespace L2D
{
    class ContentManager
    {
        private static Dictionary<string, Texture2D> LoadedTextures = new Dictionary<string, Texture2D>();
        public static Texture2D GetTexture2D(string filename)
        {
            Texture2D loadedTexture;

            if(LoadedTextures.ContainsKey(filename))
            {
                // try find the texture by the filename we asked for.
                LoadedTextures.TryGetValue(filename, out loadedTexture);

                // if found, return the texture we loaded previously.
                return loadedTexture;
            }

            // try load texture
            Texture2D textureToLoad = LoadTexture2D(filename);

            // Add texture to loaded files
            LoadedTextures.Add(filename, textureToLoad);

            // return texture we just loaded
            return textureToLoad;
        }

        private static Texture2D LoadTexture2D(string filename)
        {
            if(!File.Exists("Content/" + filename))
            {
                throw new FileNotFoundException("File not found at 'Content/" + filename + "'");
            }
            
            Texture2D newTexture = new Texture2D(filename);

            newTexture.BindTexture();

            // load bitmap
            Bitmap bmp = new Bitmap("Content/" + filename);
            Rectangle textureRect = new Rectangle(new Point(0, 0), new Size(bmp.Width, bmp.Height));
            BitmapData data = bmp.LockBits(textureRect, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(
                TextureTarget.Texture2D,
                0, PixelInternalFormat.Rgba,
                data.Width, data.Height,
                0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte, data.Scan0
                );

            bmp.UnlockBits(data);

            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureWrapS,
                (int)TextureWrapMode.Clamp
                );

            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureWrapT,
                (int)TextureWrapMode.Clamp
                );

            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Nearest
                );

            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Nearest
                );

            return newTexture;
        }
    }
}
