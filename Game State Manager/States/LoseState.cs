using Game_Development_Project.ContentLoading;
using Game_Development_Project.Levels;
using Game_Development_Project.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Development_Project.Game_State_Management.States
{
    public class LoseState : GameState
    {
        private UIButton _retryButton;

        private const string _youLostText = "Ge zijt dood jung!";
        private SpriteFont _font;

        public LoseState(Game1 gameRef, GameStateManager stateManager) : base(gameRef, stateManager)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _font = ContentLoader.Font;

            _retryButton = new UIButton(_gameRef, new Vector2(Globals.ScreenWidth / 2 - 60, 200),
                new Vector2(120, 120), ContentLoader.GetTexture("Button"), _font, "Opnieuw?");

            _retryButton.OnClick += RetryButtonClicked;

            AddComponent(_retryButton);
        }

        private void RetryButtonClicked()
        {
            _stateManager.SetState(_gameRef.MainMenu);
        }

        public override void Draw(GameTime gameTime)
        {
            BeginSpriteBatch();
            _gameRef.SpriteBatch.Draw(ContentLoader.GetTexture("Background0"), new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 3f,
                SpriteEffects.None, 0f);
            _gameRef.SpriteBatch.DrawString(_font, _youLostText,
                new Vector2(Globals.ScreenWidth / 2, 150) - _font.MeasureString(_youLostText) * 1.5f, Color.White, 0f, Vector2.Zero,
                3f, SpriteEffects.None, 0f);
            base.Draw(gameTime);
            EndSpriteBatch();
        }
    }
}
