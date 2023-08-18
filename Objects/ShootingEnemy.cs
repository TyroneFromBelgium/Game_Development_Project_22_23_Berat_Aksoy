using Game_Development_Project.Animations;
using Game_Development_Project.Game_State_Management.States;
using Game_Development_Project.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Development_Project.Objects
{
    public class ShootingEnemy : Enemy
    {
        private Player _player;

        private float _moveSpeed = 60f;
        private float _gravityForce = 190f;
        private float _playerDetectionDistance = 200f;

        private Animation _runAnimation;
        private Animation _attackAnimation;

        private bool _isPlayerInRange { get { return Vector2.Distance(Box.Position, _player.Box.Position) <= _playerDetectionDistance; } }
        private bool _isAttacking = false;

        private Texture2D _fireballTexture;

        private List<Fireball> _fireballs;

        public ShootingEnemy(Game game, Vector2 spawnPosition, Level level,
            Texture2D shootingEnemyRunTexture, Texture2D shootingEnemyAttackTexture, Texture2D fireballTexture,
            Player player) : base(game, spawnPosition, level)
        {
            _runAnimation = new Animation(shootingEnemyRunTexture, .1f, true, 9);
            _attackAnimation = new Animation(shootingEnemyAttackTexture, .2f, false, 6);

            _attackAnimation.FinishedAnimation += () =>
            {
                // Spawn vuurbal
                Fireball f = new Fireball(Game, Box.Position, _level, _fireballTexture, _facingDirection);
                _fireballs.Add(f);
                _level.AddDynamic(f.Box);

                // Set attacking attribuut op false.
                _isAttacking = false;
            };

            _player = player;

            _fireballTexture = fireballTexture;
            _fireballs = new List<Fireball>();

            Animator.PlayAnimation(_runAnimation);
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Beweeg de vijand in de richting waarin hij kijkt.
            // Als de speler binnen bereik is, vermenigvuldigen we de x-snelheid met 0, wat betekent dat de vijand niet beweegt.
            Box.Velocity = new Vector2(_facingDirection * _moveSpeed * (_isPlayerInRange ? 0 : 1), Box.Velocity.Y);
            // Zwaartekracht toepassen
            Box.Velocity += new Vector2(0, _gravityForce) * deltaTime;

            // Update vuurbal
            foreach(var f in _fireballs)
            {
                if (f.Enabled)
                {
                    f.Update(gameTime);
                }
            }

            base.Update(gameTime);

            // Als deze vijand te dicht bij de speler is, val aan
            if (_isPlayerInRange && !_isAttacking)
            {
                // Kijk naar de speler.
                _facingDirection = Math.Sign(_player.Box.Position.X - Box.Position.X);

                // VAL AAN!!!
                Animator.PlayAnimation(_attackAnimation);
                _isAttacking = true;
            }
            else if (!_isAttacking)
            {
                // Speel loop animatie
                if (Animator.CurrentAnimation != _runAnimation)
                {
                    Animator.PlayAnimation(_runAnimation);
                }

                if (_isTouchingRightWall || _isTouchingLeftWall)
                {
                    _facingDirection *= -1;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Teken vuurbal
            foreach(var f in _fireballs)
            {
                if (f.Visible)
                {
                    f.Draw(gameTime);
                }
            }

        }

        protected override Vector2 GetBoxSize()
        {
            return new Vector2(49, 39);
        }

        protected override int GetMaxHealth()
        {
            return 1;
        }
    }
}
