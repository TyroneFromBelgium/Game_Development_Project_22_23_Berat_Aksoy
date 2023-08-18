using Game_Development_Project.Animations;
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
    public class PatrollingEnemy : Enemy
    {
        private float _moveSpeed = 50f;
        private float _gravityForce = 190f;

        private Animation _runAnimation;

        public PatrollingEnemy(Game game, Vector2 spawnPosition, Level level, Texture2D patrollingEnemyRunTextrure) : base(game, spawnPosition, level)
        {
            _runAnimation = new Animation(patrollingEnemyRunTextrure, .1f, true, 8);
            Animator.PlayAnimation(_runAnimation);
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Beweeg de patrouillerende vijand in de richting waarin hij kijkt
            Box.Velocity = new Vector2(_facingDirection * _moveSpeed, Box.Velocity.Y);
            // Zwaartekracht toepassen
            Box.Velocity += new Vector2(0, _gravityForce) * deltaTime;

            base.Update(gameTime);

            // Als de vijand een muur raakt, keert hij zijn looprichting om.
            if (_isTouchingRightWall || _isTouchingLeftWall)
            {
                _facingDirection *= -1;
            }
        }

        protected override int GetMaxHealth()
        {
            return 1;
        }
        protected override Vector2 GetBoxSize()
        {
            return new Vector2(20, 37);
        }
    }
}
