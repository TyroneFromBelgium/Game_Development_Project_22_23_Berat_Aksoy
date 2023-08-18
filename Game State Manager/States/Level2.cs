using Game_Development_Project.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Development_Project.Game_State_Management.States
{
    public class Level2 : GameplayState
    {
        public Level2(Game1 gameRef, GameStateManager stateManager) : base(gameRef, stateManager)
        {
        }

        public override List<string> GetLevelString()
        {
            return new List<string>
            {
                "W.........................WW",
                "W.........................WW",
                "WP........................WW",
                "W.........................WW",
                "W........C................WW",
                "W.....C.SGSS..C....C....CDWW",
                "WGSSSSGS....SSGSSSSGSSSSGGWW",
                "W.........................WW",
                "W.........................WW",
                "W.........................WW",
                "W.........................WW",
                "WX..Z.....Z...X.....Z..Z..WW",
                "W..Z.Z..X..Z.X.Z.X.Z.X..Z.WW",
                "W.........................WW",
                "GGGGGGGGGGGGGGGGGGGGGGGGGGGG",
            };
        }

        protected override Player GetPlayer()
        {
            // Als we al een speler in Globals hebben, halen we zijn health op.
            int playerHealth = -1;
            if (Globals.Instance.Player != null)
            {
                // Pak spelers health
                playerHealth = Globals.Instance.Player.CurrentHealth;
            }

            Globals.Instance.Player = new Player(_gameRef, _level.PlayerSpawnPosition, _level,
                _playerRunTexture, _playerIdleTexture, _playerAttackTexture, _playerInAirTexture);

            // Set de spelers HP op de waarde die we hierboven pakken.
            if (playerHealth != -1)
            {
                Globals.Instance.Player.SetHealth(playerHealth);
            }

            return Globals.Instance.Player;
        }

        protected override void MoveToNextLevel()
        {
            _stateManager.SetState(_gameRef.WinState);
        }
    }
}
