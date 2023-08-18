using Game_Development_Project.Collisions;
using Game_Development_Project.Levels;
using Game_Development_Project.Objects;
using Game_Development_Project.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Development_Project.Game_State_Management.States
{
    public class Level1 : GameplayState
    {
        public Level1(Game1 gameRef, GameStateManager stateManager) : base(gameRef, stateManager)
        {
        }

        public override List<string> GetLevelString()
        {
            return new List<string>
            {
                "W.........................W",
                "W.........................W",
                "WP..........Z.............W",
                "W...C....W.....W.........CW",
                "WGGGGGGGGGGGGGGGGGGGGG...GW",
                "W.........................W",
                "W.........................W",
                "W............X............W",
                "W....W.........W......CSSSW",
                "W..GGGGGGGGGGGGGGGGGGGGGGGW",
                "W.C.......................W",
                "WC........................W",
                "W......X...............Z..W",
                "W...SSG.....C..C..C......DW",
                "GGGGGGGGGGGGGGGGGGGGGGGGGGG",
            };
        }

        public override void Enter()
        {
            base.Enter();

            // Ze stellen hier de score van de speler op 0 omdat dit de eerste level klasse is
            Globals.Instance.PlayerScore = 0;
        }

        protected override void MoveToNextLevel()
        {
            _stateManager.SetState(_gameRef.Level2);
        }

        protected override Player GetPlayer()
        {
            // Omdat dit de eerste level is, creeren we een nieuwe instantie van onze speler.
            Globals.Instance.Player = new Player(_gameRef, _level.PlayerSpawnPosition, _level,
                _playerRunTexture, _playerIdleTexture, _playerAttackTexture, _playerInAirTexture);

            return Globals.Instance.Player;
        }
    }
}
