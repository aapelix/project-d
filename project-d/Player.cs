using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace project_d
{
    internal class Player : ScaledSprite
    {
        public Player(Texture2D texture, Vector2 pos) : base(texture, pos)
        {

        }

        public void Update(GameTime gameTime, List<ScaledSprite> collisionGroup)
        {
            float changeX = 0;

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                changeX += 5;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                changeX -= 5;
            }

            pos.X += changeX;

                foreach (var sprite in collisionGroup)
                {
                    if (sprite.Rect.Intersects(Rect))
                    {
                        pos.X -= changeX;
                    }
                }

            foreach (var sprite in collisionGroup)
            {
                if (!sprite.Rect.Intersects(Rect))
                {
                    pos.Y += 1;
                }
            }
        }

        public override void Update()
        {
            base.Update();
        }
    }
}