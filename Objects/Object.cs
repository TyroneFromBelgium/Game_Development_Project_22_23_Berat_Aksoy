using Game_Development_Project.Animations;
using Game_Development_Project.Collisions;
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

    // Met objects hier bedoel ik alles dat een dynamische box heeft, dus onze speler, 2 enemies enz.
    public abstract class Object : DrawableGameComponent
    {
        public Box Box { get; protected set; }
        public Animator Animator { get; protected set; }
        public Level Level { get; set; }

        public int CurrentHealth { get; protected set; }
        public event Action OnObjectDeath;


        // 1 = rechts
        // -1 = links
        protected int _facingDirection;
        protected bool _grounded;
        protected bool _isTouchingRightWall;
        protected bool _isTouchingLeftWall;
        protected float _invulnurableTimer;
        protected Level _level;

        protected bool _isInvulnurable { get { return _invulnurableTimer > 0; } }

        public Object(Game game, Vector2 spawnPosition, Level level, Box.BoxType type) : base(game)
        {
            Box = new Box(spawnPosition, GetBoxSize(), type);
            Box.Data = this;
            Animator = new Animator();

            _facingDirection = 1;
            _grounded = false;
            _isTouchingRightWall = false;
            _isTouchingLeftWall = false;
            _level = level;

            _invulnurableTimer = -1;

            CurrentHealth = GetMaxHealth();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _invulnurableTimer -= deltaTime;

            DetectAndSolveStaticCollisions(deltaTime);
            DetectAndSolveSpikeCollisions(deltaTime);
            DetectAndSolveCoinCollisions(deltaTime);

            DetectAndResolveDynamicCollisions(deltaTime);

            // Set the position according to the velocity,
            // Stel de positie in op basis van de snelheid,
            // We zorgen ervoor dat we vermenigvuldigen met de deltaTime om de beweging consistent te houden

            Box.Position += Box.Velocity * deltaTime;

            if (Math.Abs(Box.Velocity.X) >= 1f && Math.Sign(Box.Velocity.X) != Math.Sign(_facingDirection))
                _facingDirection *= -1;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Animator.Draw(gameTime, Globals.Instance.SpriteBatch, Box.Position,
                _isInvulnurable ? Color.Black : Color.White, _facingDirection == -1 ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None);
        }

        // Levens
        protected abstract int GetMaxHealth();
        protected abstract float GetInvulnurableTime();
        public void TakeDamage(int damage)
        {
            if (_isInvulnurable) return;

            _invulnurableTimer = GetInvulnurableTime();
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                OnObjectDeath?.Invoke();
                CurrentHealth = 0;
            }
        }
        public void SetHealth(int health)
        {
            CurrentHealth = health;
        }

        // Collisions
        protected abstract Vector2 GetBoxSize();
        protected virtual void DetectAndResolveDynamicCollisions(float deltaTime)
        {
            var dynamics = _level.Dynamics;
            foreach (var d in  dynamics)
            {
                // Als het dynamische object is uitgeschakeld, doen we niets
                if (((Object)d.Data).Enabled == false) return;

                if (Box.Cast(d, out Vector2 cp, out Vector2 cn, out float hitNear, deltaTime))
                {
                    ResolveDynamicCollision(d);
                }
            }
        }
        protected abstract void ResolveDynamicCollision(Box other);

        protected virtual void DetectAndSolveStaticCollisions(float deltaTime)
        {
            var tiles = _level.StaticTiles;

            _grounded = false;
            _isTouchingRightWall = false;
            _isTouchingLeftWall = false;
            
            List<KeyValuePair<int, float>> tilesDistance = new();
            // Haal waarden op die gesorteerd moeten worden op basis van hun afstand tot de speler
            for (int i = 0; i < tiles.Count; i++)
            {
                if (Box.Cast(tiles[i], out Vector2 cp, out Vector2 cn, out float hitNear, deltaTime))
                {
                    // Als x as 0 en y as -1 is van de colission betekent dit dat het object een tegel raakt die eronder ligt, wat betekent dat het op de grond staat aka isGrounded
                    if (cn == new Vector2(0, -1))
                    {
                        _grounded = true;
                    }

                    tilesDistance.Add(new KeyValuePair<int, float>(i, hitNear));
                }
            }

            // Sorteer
            tilesDistance.Sort((a, b) => a.Value.CompareTo(b.Value));

            // Collision resolution
            for (int i = 0; i < tilesDistance.Count; i++)
            {
                Box rect = tiles[tilesDistance[i].Key];

                if (Box.Cast(rect, out Vector2 cp, out Vector2 cn, out float ct, deltaTime))
                {
                    if (cn.X == 1)
                    {
                        _isTouchingRightWall = true;
                    }
                    else if (cn.X == -1)
                    {
                        _isTouchingLeftWall = true;
                    }

                    Box.Velocity += cn * new Vector2(MathF.Abs(Box.Velocity.X), MathF.Abs(Box.Velocity.Y)) * (1 - ct);
                }
            }
        }

        protected virtual void DetectAndSolveCoinCollisions(float deltaTime)
        {
            var coins = _level.CoinTiles;

            for (int i = 0; i < coins.Count; i++)
            {
                if (Box.Cast(coins[i], out Vector2 cp, out Vector2 cn, out float hitNear, deltaTime))
                {
                    // Collision met coin/munt is gebeurd.
                    Vector2 gridPos = new Vector2(coins[i].Position.X / Level.TileWidth,
                        coins[i].Position.Y / Level.TileHeight);

                    ResolveCoinCollision(gridPos);
                }
            }
        }
        protected abstract void ResolveCoinCollision(Vector2 coinGridPosition);


        protected virtual void DetectAndSolveSpikeCollisions(float deltaTime)
        {
            var spikes = _level.SpikeTiles;

            for (int i = 0; i < spikes.Count; i++)
            {
                if (Box.Cast(spikes[i], out Vector2 cp, out Vector2 cn, out float hitNear, deltaTime))
                {
                    // Collision met spike gebeurd.
                    ResolveSpikeCollision();
                }
            }
        }
        protected abstract void ResolveSpikeCollision();
    }
}
