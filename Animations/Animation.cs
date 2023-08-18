using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Development_Project.Animations
{
    public class Animation
    {
        public Texture2D Texture { get; private set; }
        public float FrameDuration { get; private set; }
        public bool Loop { get; private set; }
        public int FrameCount { get; private set; }

        public event Action FinishedAnimation;

        public int FrameWidth
        {
            get { return Texture.Width / FrameCount; }
        }
        public int FrameHeight
        {
            get { return Texture.Height; }
        }
        public void InvokeFinishedAnimation()
        {
           FinishedAnimation?.Invoke();
        }


        public Animation(Texture2D texture, float frameDuration, bool loop, int frameCount)
        {
            Texture = texture;
            FrameDuration = frameDuration;
            Loop = loop;
            FrameCount = frameCount;
        }
    }
}
