using Microsoft.Xna.Framework;

namespace project_d
{
    internal class AnimationManager
    {
        int numColumns;
        int numFrames;
        Vector2 size;

        int counter;
        int activeFrame;
        int interval;

        int colPos;
        int rowPos;

        public AnimationManager(int numFrames, int numColumns, int interval, int rowPos, Vector2 size)
        {
            this.numFrames = numFrames;
            this.numColumns = numColumns;
            this.interval = interval;
            this.size = size;
            this.rowPos = rowPos;

            counter = 0;
            activeFrame = 0;

            colPos = 0;
        }

        public void Update()
        {
            counter++;
            if (counter > interval)
            {
                counter = 0;
                NextFrame();
            }


        }

        private void NextFrame()
        {
            activeFrame++;
            colPos++;
            if (activeFrame >= numFrames)
            {
                ResetAnimation();
            }

            if (colPos >= numColumns)
            {
                colPos = 0;
            }
        }

        private void ResetAnimation()
        {
            activeFrame = 0;
            colPos = 0;
        }

        public Rectangle GetFrame()
        {
            //new Rectangle(activeFrame * 192, 577, 192, 192)
            return new Rectangle(activeFrame * (int)size.X,
            rowPos * (int)size.Y,
            (int)size.X,
            (int)size.Y);
        }
    }


}