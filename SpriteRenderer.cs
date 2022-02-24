using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2D
{
    public static class SpriteRenderer
    {
        private static List<Sprite> Sprites = new List<Sprite>();

        public static void AddSprite(Sprite spr)
        {
            Sprites.Add(spr);
        }

        public static void RemoveSprite(Sprite spr)
        {
            Sprites.Remove(spr);
        }

        public static void Render()
        {
            foreach(Sprite spr in Sprites)
            {
                spr.RenderStandard();
            }
        }
    }
}
