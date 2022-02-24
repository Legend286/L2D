//#define USE_INSTANCING
using System.Collections.Generic;

namespace L2D
{
    public static class SpriteRenderer
    {
        private static List<Sprite> Sprites = new List<Sprite>();
        public static Quad Q = new Quad();
        public static void AddSprite(Sprite spr)
        {
            Sprites.Add(spr);
        }

        public static void RemoveSprite(Sprite spr)
        {
            Sprites.Remove(spr);
        }

        public static int GetCount()
        {
            return Sprites.Count;
        }

        public static void Render()
        {
            foreach(Sprite spr in Sprites)
            {
#if USE_INSTANCING
                spr.SetupDrawingInstanced();
                DrawInstanced();
#else
                spr.RenderStandard();
#endif
            }
        }

        public static void DrawInstanced()
        {
            Q.DrawInstanced(Sprites);
        }
    }
}
