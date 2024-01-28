using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project_d
{
    internal class ScaledSprite : Sprite
    {

        public Rectangle Rect
        {
            get
            {
                return new Rectangle((int)pos.X, (int)pos.Y, 120, 140);
            }
        }
        public ScaledSprite(Texture2D texture, Vector2 pos) : base(texture, pos)
        {

        }
    }
}