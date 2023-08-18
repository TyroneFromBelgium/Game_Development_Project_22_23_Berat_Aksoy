using Game_Development_Project.ContentLoading;
using Game_Development_Project.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Development_Project.Game_State_Management.States
{
    public class MainMenu : GameState
    {
        private UIButton _playButton;
        private UIButton _level1Button;
        private UIButton _level2Button;
        private UIButton _exitButton;

        private const string _gameName = "Ridder";
        private const string _tutorialText = "Linker arrow voor links, Rechter arrow voor rechts, Boven arrow om te springe, L-MouseButton voor attack";
        private SpriteFont _font;

        public MainMenu(Game1 gameRef, GameStateManager stateManager) : base(gameRef, stateManager)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _font = ContentLoader.Font;

            _playButton = new UIButton(_gameRef, new Vector2(Globals.ScreenWidth / 2 - 60, 125),
                new Vector2(120, 120), ContentLoader.GetTexture("Button"), _font, "Speel");

            _level1Button = new UIButton(_gameRef, new Vector2(Globals.ScreenWidth / 2 - 180, 200),
                new Vector2(120, 120), ContentLoader.GetTexture("Button"), _font, "Level 1");

            _level2Button = new UIButton(_gameRef, new Vector2(Globals.ScreenWidth / 2 + 60, 200),
                new Vector2(120, 120), ContentLoader.GetTexture("Button"), _font, "Level 2");

            _exitButton = new UIButton(_gameRef, new Vector2(Globals.ScreenWidth / 2 - 60, 245),
                new Vector2(120, 120), ContentLoader.GetTexture("Button"), _font, "Quit");

            _playButton.OnClick += PlayButtonClicked;
            _level1Button.OnClick += PlayButtonClicked;
            _level2Button.OnClick += Level2Pressed;
            _exitButton.OnClick += ExitButtonClicked;

            AddComponent(_playButton);
            AddComponent(_level1Button);
            AddComponent(_level2Button);
            AddComponent(_exitButton);
        }

        private void Level2Pressed()
        {
            // Reset speler
            Globals.Instance.Player = null;
            Globals.Instance.PlayerScore = 0;

            _stateManager.SetState(_gameRef.Level2);
        }
        private void PlayButtonClicked()
        {
            _stateManager.SetState(_gameRef.Level1);
        }
        private void ExitButtonClicked()
        {
            _gameRef.Exit();
        }

        public override void Draw(GameTime gameTime)
        {
            BeginSpriteBatch();
            _gameRef.SpriteBatch.Draw(ContentLoader.GetTexture("Background0"), new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 3f,
                SpriteEffects.None, 0f);
            _gameRef.SpriteBatch.DrawString(_font, _gameName,
                new Vector2(Globals.ScreenWidth / 2, 75) - _font.MeasureString(_gameName) * 1.5f, Color.White, 0f, Vector2.Zero,
                3f, SpriteEffects.None, 0f);
            _gameRef.SpriteBatch.DrawString(_font, _tutorialText,
                new Vector2(10, Globals.ScreenHeight - 25), Color.White, 0f, Vector2.Zero,
                1f, SpriteEffects.None, 0f);
            base.Draw(gameTime);
            EndSpriteBatch();
        }
    }
}
