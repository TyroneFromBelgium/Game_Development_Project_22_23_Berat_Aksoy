using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Development_Project.ContentLoading
{
    public static class ContentLoader
    {
        public static SpriteFont Font { get; private set; }

        private static Dictionary<string, Texture2D> _textures;

        private static ContentManager _content;

        public static void Initialize(ContentManager content)
        {
            _textures = new Dictionary<string, Texture2D>();
            _content = content;
        }

        public static void LoadSpriteFont(string contentName)
        {
            Font =_content.Load<SpriteFont>(contentName);
        }

        public static void LoadTexture(string name, string contentName)
        {
            _textures.Add(name, _content.Load<Texture2D>(contentName));
        }
        public static Texture2D GetTexture(string name)
        {
            return _textures[name];
        }
    }
}
