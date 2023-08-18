using Game_Development_Project.ContentLoading;
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
    public class WinState : GameState
    {
        private UIButton _retryButton;

        private const string _youWinText = "Gewonnen!";
        private const string _scoreText = "Aantal muntjes verzameld: ";
        private SpriteFont _font;

        private int _playerScore;

        public WinState(Game1 gameRef, GameStateManager stateManager) : base(gameRef, stateManager)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _font = ContentLoader.Font;

            _retryButton = new UIButton(_gameRef, new Vector2(Globals.ScreenWidth / 2 - 60, 200),
                new Vector2(120, 120), ContentLoader.GetTexture("Button"), _font, "Hoofdmenu");

            _retryButton.OnClick += RetryButtonClicked;

            AddComponent(_retryButton);

            // Bereken de spelers score.
            _playerScore = Globals.Instance.PlayerScore + Globals.Instance.Player.CurrentHealth;
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
            _gameRef.SpriteBatch.DrawString(_font, _youWinText,
                new Vector2(Globals.ScreenWidth / 2, 80) - _font.MeasureString(_youWinText) * 1.5f, Color.White, 0f, Vector2.Zero,
                3f, SpriteEffects.None, 0f);
            _gameRef.SpriteBatch.DrawString(_font, _scoreText + _playerScore,
                new Vector2(Globals.ScreenWidth / 2, 150) - _font.MeasureString(_scoreText + _playerScore) * 1.5f, Color.White, 0f, Vector2.Zero,
                3f, SpriteEffects.None, 0f);
            base.Draw(gameTime);
            EndSpriteBatch();
        }
    }
}
