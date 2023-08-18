using Game_Development_Project.Animations;
using Game_Development_Project.Collisions;
using Game_Development_Project.Input_Handling;
using Game_Development_Project.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game_Development_Project.Objects
{
    public class Player : Object
    {
        private Animation _runAnimation;
        private Animation _idleAnimation;
        private Animation _attackAnimation;
        private Animation _inAirAnimation;

        private bool _isAttacking;
        private bool _hasAttacked;

        // Movement Properties
        private float _moveSpeed = 170f;
        private float _jumpForce = 260f;
        private float _gravityForce = 200f;
        private float _normalGravityMultiplier = 2.8f;
        private float _fallGravityMultiplier = 3.8f;

        public Player(Game game, Vector2 spawnPosition, Level level,
            Texture2D playerRunTex, Texture2D playerIdleTex, Texture2D playerAttackTex, Texture2D playerInAirTex) : base(game, spawnPosition, level, Box.BoxType.Player)
        {
            _runAnimation = new Animation(playerRunTex, .09f, true, 7);
            _idleAnimation = new Animation(playerIdleTex, .2f, true, 4);
            _attackAnimation = new Animation(playerAttackTex, .1f, false, 4);
            _inAirAnimation = new Animation(playerInAirTex, .5f, false, 2);

            _attackAnimation.FinishedAnimation += () => 
            {
                _isAttacking = false;
                _hasAttacked = false;
            };

            Animator.PlayAnimation(_idleAnimation);
        }

        protected override int GetMaxHealth()
        {
            // Max leven is 3
            return 3;
        }
        protected override float GetInvulnurableTime()
        {
            return 0.5f;
        }

        protected override void ResolveCoinCollision(Vector2 coinGridPosition)
        {
            _level.SetTile(((int)coinGridPosition.X), ((int)coinGridPosition.Y), '.');
            Globals.Instance.PlayerScore += 1;
        }
        protected override void ResolveSpikeCollision()
        {
            TakeDamage(1);
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Movement
            int moveDir = InputHandler.KeyHeldDown(Keys.Right) ? 1 : 0 + (InputHandler.KeyHeldDown(Keys.Left) ? -1 : 0);

            // Spring
            if (InputHandler.KeyPress(Keys.Up) && _grounded)
            {
                Box.Velocity -= new Vector2(0, _jumpForce);
            }

            // Stel de snelheid in
            Box.Velocity = new Vector2(moveDir * _moveSpeed, Box.Velocity.Y);

            // Zwaartekracht toepassen
            // Om de besturing van de speler goed te laten aanvoelen, passen we meer zwaartekracht toe wanneer de speler naar beneden valt.
            float gravityMultiplier = Box.Velocity.Y <= 0 ? _normalGravityMultiplier : _fallGravityMultiplier;
            Box.Velocity += new Vector2(0, _gravityForce) * gravityMultiplier * deltaTime;

            // Controleer of de speler aanvalt.
            if (_isAttacking && !_hasAttacked)
            {
                // Als dat waar is, doorlopen we alle dynamische objecten om vijanden te vinden waar we schade aan kunnen toebrengen.
                foreach (var d in _level.Dynamics)
                {
                    if (d.Type == Box.BoxType.Enemy)
                    {
                        // Controleer of de vijand binnen het bereik van de aanval is
                        Rectangle enemyRect = new Rectangle(d.Position.ToPoint(), d.Size.ToPoint());
                        // Deze rectangle is de aanvals-hitbox van de speler
                        Rectangle playerHitRect = new Rectangle(
                            new Point(_facingDirection == 1 ? (int)(Box.Position.X + Box.Size.X + 15) : (int)(Box.Position.X - Box.Size.X), (int)Box.Position.Y),
                            Box.Size.ToPoint() + new Point(10, 0));

                        if (playerHitRect.Intersects(enemyRect))
                        {
                            // Pak de enemy object
                            Object enemyObject = (Object)d.Data;
                            // Doe schade
                            enemyObject.TakeDamage(1);
                        }
                    }
                }

                _hasAttacked = true;
            }

            // Nu callen we de base.Update zodat het onze collisions kan afhandelen en passen onze finale posities aan
            base.Update(gameTime);

            // Animaties
            if (InputHandler.MousePress(InputHandler.MouseButton.Left) && !_isAttacking)
            {
                _isAttacking = true;
                Animator.PlayAnimation(_attackAnimation);
            }
            if (!_isAttacking && _grounded)
            {
                if (Math.Abs(Box.Velocity.X) > 1f && Animator.CurrentAnimation != _runAnimation)
                {
                    Animator.PlayAnimation(_runAnimation);
                }
                else if (Math.Abs(Box.Velocity.X) < .3f && Animator.CurrentAnimation != _idleAnimation)
                {
                    Animator.PlayAnimation(_idleAnimation);
                }
            }
            else if (!_isAttacking && Animator.CurrentAnimation != _inAirAnimation)
            {
                Animator.PlayAnimation(_inAirAnimation);
            }
        }

        protected override Vector2 GetBoxSize()
        {
            return new Vector2(35, 64);
        }

        protected override void ResolveDynamicCollision(Box other)
        {
            // Als de andere box een vijand is
            if (other.Type == Box.BoxType.Enemy)
            {
                TakeDamage(1);
            }
        }
    }
}
