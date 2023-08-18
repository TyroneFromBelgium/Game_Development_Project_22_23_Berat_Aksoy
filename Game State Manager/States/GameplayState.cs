using Game_Development_Project.ContentLoading;
using Game_Development_Project.Levels;
using Game_Development_Project.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Game_Development_Project.Game_State_Management.States
{
    public abstract class GameplayState : GameState
    {
        protected Level _level;
        protected Player _player;
        protected Rectangle _doorRectangle;

        // Resources
        protected SpriteFont _font;
        protected Texture2D _tilesetTexture;
        protected Texture2D _backgroundTexture;
        protected Texture2D _heartTexture;

        // Object Textures
        protected Texture2D _playerIdleTexture;
        protected Texture2D _playerRunTexture;
        protected Texture2D _playerAttackTexture;
        protected Texture2D _playerInAirTexture;
        protected Texture2D _patrollingEnemyRunTexture;
        protected Texture2D _shootingEnemyRunTexture;
        protected Texture2D _shootingEnemyAttackTexture;
        protected Texture2D _fireballTexture;


        public GameplayState(Game1 gameRef, GameStateManager stateManager) : base(gameRef, stateManager)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _font = ContentLoader.Font;
            _tilesetTexture = ContentLoader.GetTexture("Tileset");
            _backgroundTexture = ContentLoader.GetTexture("Background0");
            _heartTexture = ContentLoader.GetTexture("Heart");

            _playerIdleTexture = ContentLoader.GetTexture("PlayerIdle");
            _playerRunTexture = ContentLoader.GetTexture("PlayerRun");
            _playerAttackTexture = ContentLoader.GetTexture("PlayerAttack");
            _playerInAirTexture = ContentLoader.GetTexture("PlayerInAir");
            _patrollingEnemyRunTexture = ContentLoader.GetTexture("PatrollingEnemyRun");
            _shootingEnemyRunTexture = ContentLoader.GetTexture("ShootingEnemyRun");
            _shootingEnemyAttackTexture = ContentLoader.GetTexture("ShootingEnemyAttack");
            _fireballTexture = ContentLoader.GetTexture("Fireball");


            _level = new Level(GetLevelString(), _tilesetTexture, _backgroundTexture);

            // Spawn deur (om te teleporteren naar volgende level)
            _doorRectangle = new Rectangle(_level.DoorSpawnPosition.ToPoint(), new Point(48, 48));

            // Spawn speler
            _player = GetPlayer();
            AddComponent(_player);
            _level.AddDynamic(_player.Box);

            _player.OnObjectDeath += PlayerDeath;

            // Spawn vijanden

            // Patrouilleerde vijand (Paddenstoelkop)
            foreach (var pos in _level.PatrollingEnemySpawnPositions)
            {
                var p = new PatrollingEnemy(_gameRef, pos, _level, _patrollingEnemyRunTexture);
                AddComponent(p);
                _level.AddDynamic(p.Box);
            }
            // Vuurbal schietende vijand (slang)
            foreach (var pos in _level.ShootingEnemySpawnPositions)
            {
                var p = new ShootingEnemy(_gameRef, pos, _level, _shootingEnemyRunTexture,
                    _shootingEnemyAttackTexture, _fireballTexture, _player);
                AddComponent(p);
                _level.AddDynamic(p.Box);
            }
        }
        protected abstract Player GetPlayer();

        private void PlayerDeath()
        {
            _stateManager.SetState(_gameRef.LoseState);
        }

        // Moet worden geïmplementeerd in alle child gameplay states.
        // Geeft het level van de gameplay state terug.
        public abstract List<string> GetLevelString();

        

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
       

            // Controleer of de speler collide met de deur
            Rectangle playerRect = new Rectangle(_player.Box.Position.ToPoint(), _player.Box.Size.ToPoint());
            if (playerRect.Intersects(_doorRectangle))
            {
                MoveToNextLevel();
            }
        }
        protected abstract void MoveToNextLevel();

        public override void Draw(GameTime gameTime)
        {
            BeginSpriteBatch();
            // Teken level eerst
            _level.Draw(_gameRef.SpriteBatch);
            // Teken alle derest na het tekenen van level
            base.Draw(gameTime);
            EndSpriteBatch();


            BeginSpriteBatch();
            // Teken U.I laatst.
            if (_player != null)
            {
                for (int i = 0; i < _player.CurrentHealth; i++)
                {
                    Vector2 pos = new Vector2(10 + (i * 40), 10);
                    _gameRef.SpriteBatch.Draw(_heartTexture, pos, null, Color.White, 0f, Vector2.Zero, .7f, SpriteEffects.None, 0f);
                }
            }
            EndSpriteBatch();
        }
    }
}
