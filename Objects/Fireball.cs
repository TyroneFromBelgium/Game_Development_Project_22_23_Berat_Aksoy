using Game_Development_Project.Animations;
using Game_Development_Project.Collisions;
using Game_Development_Project.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Development_Project.Objects
{
    public class Fireball : Object
    {
        private Animation _idleAnimation;

        public Fireball(Game game, Vector2 spawnPosition, Level level,
            Texture2D fireballTexture, int facingDirection) : base(game, spawnPosition, level, Box.BoxType.Projectile)
        {
            _facingDirection = facingDirection;

            _idleAnimation = new Animation(fireballTexture, .1f, true, 6);
            Animator.PlayAnimation(_idleAnimation);
        }

        public override void Update(GameTime gameTime)
        {
            // Move de vuurbal naar de richting waar het naar kijkt
            Box.Velocity = new Vector2(_facingDirection * 170, Box.Velocity.Y);

            base.Update(gameTime);

            // Als de vuurbal een muur raakt, dan schakelen we deze uit zodat het de speler kan schade kan brengen als deze al is gevuurt.
            if (_isTouchingRightWall || _isTouchingRightWall)
            {
                Enabled = false;
                Visible = false;
            }
        }

        protected override Vector2 GetBoxSize()
        {
            return new Vector2(20, 15);
        }

        protected override float GetInvulnurableTime()
        {
            return 0;
        }

        protected override int GetMaxHealth()
        {
            return 0;
        }

        protected override void ResolveCoinCollision(Vector2 coinGridPosition)
        {
            return;
        }

        protected override void ResolveDynamicCollision(Box other)
        {
            if (other.Type == Box.BoxType.Player)
            {
                var playerObject = (Object)other.Data;
                playerObject.TakeDamage(1);

                Enabled = false;
                Visible = false;
            }
        }

        protected override void ResolveSpikeCollision()
        {
            return;
        }
    }
}
