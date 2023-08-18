using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Development_Project.Animations
{
    public class Animator
    {
        public Animation CurrentAnimation { get { return currentAnimation; } }
        public int FrameIndex { get { return frameIndex; } }

        private Animation currentAnimation;
        private int frameIndex;
        private float time;

        // Speel animatie
        public void PlayAnimation(Animation animation)
        {
            currentAnimation = animation;
            frameIndex = 0;
            time = 0.0f;
        }

        // Draw animatie
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, Color color = default, SpriteEffects spriteEffects = SpriteEffects.None)
        {
            if (currentAnimation == null) return;

            // Increment time
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Als de timer groter is dan of gelijk aan de duur van het frame, springen we naar de volgende frameIndex.
            if (time >= currentAnimation.FrameDuration)
            {
                time = 0.0f;

                if (currentAnimation.Loop)
                {
                    frameIndex = (frameIndex + 1) % currentAnimation.FrameCount;
                }
                else
                {
                    frameIndex = Math.Min(frameIndex + 1, currentAnimation.FrameCount - 1);
                    if (frameIndex == currentAnimation.FrameCount - 1) currentAnimation.InvokeFinishedAnimation();
                }
            }

            // Source rectangle van texture pakken me frameIndex zodat we alleen drawen wat we nodig hebben.
            Rectangle sourceRect = new Rectangle(FrameIndex * currentAnimation.FrameWidth, 0,
                currentAnimation.FrameWidth, currentAnimation.Texture.Height);

            spriteBatch.Draw(currentAnimation.Texture, position, sourceRect, color, 0.0f,
                Vector2.Zero, 1.0f, spriteEffects, 0.0f);
        }
    }
}
