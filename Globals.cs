using Game_Development_Project.Objects;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Development_Project
{
    public class Globals
    {
        private Globals()
        {
            PlayerScore = 0;
        }

        private static Globals _instance;
        private static readonly object _lock = new object();
        public static Globals Instance
        {
            get
            {
                lock ( _lock )
                {
                    if ( _instance == null )
                    {
                        _instance = new Globals();
                    }
                }
                return _instance;
            }
        }


        public const int ScreenWidth = 860;
        public const int ScreenHeight = 480;

        public SpriteBatch SpriteBatch { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }

        // Deze speler verwijzing wordt gebruikt om de HP van de speler tussen levels te behouden."
        public Player Player { get; set; }
        public int PlayerScore { get; set; }
    }
}
