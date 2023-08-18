using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Development_Project.Game_State_Management
{
    // Managed de scenes
    public class GameStateManager : DrawableGameComponent
    {
        // Verwijzing naar de huidige gamestate
        private GameState _currentGameState;

        public GameStateManager(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Als enable false is returneren we
            if (!Enabled) return;

            // We updaten huidige staat als het niet null is
            if (_currentGameState != null)
            {
                _currentGameState.Update(gameTime);
            }
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Hier tekenen we de huidige staat indien niet null is
            if (_currentGameState != null)
            {
                _currentGameState.Draw(gameTime);
            }
        }

        // Verander huidige staat
        public void SetState(GameState state)
        {
            // Als een staat al actief is, roepen wij de exit() functie op.
            if (_currentGameState != null)
            {
                _currentGameState.Exit();
            }

            // Verander staat
            _currentGameState = state;
            // Roep de enter() functie op voor de nieuwe staat
            _currentGameState.Enter();
        }
    }
}
