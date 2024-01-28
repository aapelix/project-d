using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project_d
{
    internal class ColoredSprite : ScaledSprite
    {
        public Color color;
        public ColoredSprite(Texture2D texture, Vector2 pos, Color color) : base(texture, pos)
        {
            this.color = color;
        }
    }
}