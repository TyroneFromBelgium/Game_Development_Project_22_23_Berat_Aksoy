using Game_Development_Project.Input_Handling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Development_Project.UI
{
    public class UIButton : DrawableGameComponent
    {
        private Vector2 _position;
        private Vector2 _size;
        private Texture2D _texture;
        private SpriteFont _font;
        private string _text;

        private Rectangle _rect;
        private Vector2 _textPosition;

        // Opgeroepen wanneer de gebruiker op de knop klikt
        public event Action OnClick;

        public UIButton(Game game, Vector2 position, Vector2 size, Texture2D texture, SpriteFont spriteFont, string text) : base(game)
        {
            _position = position;
            _size = size;
            _texture = texture;
            _font = spriteFont;
            _text = text;

            _rect = new Rectangle(position.ToPoint(), size.ToPoint());

            _textPosition = _rect.Center.ToVector2() - (_font.MeasureString(_text) / 2f);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Haal de positie van de muis op
            Point mousePos = InputHandler.GetMousePosition();

            // Controleer of de positie van de muis zich binnen de rechthoek van de knop bevindt
            if (_rect.Contains(mousePos))
            {
                // Als de linker muisknop wordt ingedrukt, roepen we het OnClick() aan.
                if (InputHandler.MousePress(InputHandler.MouseButton.Left))
                {
                    OnClick?.Invoke();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            var spriteBatch = Globals.Instance.SpriteBatch;

            spriteBatch.Draw(_texture, _rect, Color.White);
            spriteBatch.DrawString(_font, _text, _textPosition, Color.White);
        }
    }
}
