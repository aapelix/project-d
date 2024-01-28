using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project_d
{
    internal class MovingSprite : ScaledSprite
    {
        private float speed;

        public MovingSprite(Texture2D texture, Vector2 pos, float speed) : base(texture, pos)
        {
            this.speed = speed;
        }

        public override void Update()
        {
            base.Update();
            pos.X += speed;
        }
    }
}