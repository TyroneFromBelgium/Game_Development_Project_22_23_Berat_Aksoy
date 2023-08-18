using Game_Development_Project.Collisions;
using Game_Development_Project.Levels;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Development_Project.Objects
{
    public abstract class Enemy : Object
    {
        public Enemy(Game game, Vector2 spawnPosition, Level level) : base(game, spawnPosition, level, Box.BoxType.Enemy)
        {
            // Subscribe op death event
            // Als de vijand dood gaat, wordt deze uitgeschakeld en wordt er niet meer getekend.
            OnObjectDeath += () =>
            {
                Enabled = false;
                Visible = false;
            };
        }

        protected override void ResolveDynamicCollision(Box other)
        {
            // Als we colliden met de speler.
            if (other.Type == Box.BoxType.Player)
            {
                // We halen het spelersobject op uit de gegevens van de box.
                var playerObject = (Object)other.Data;
                // Doe schade naar de speler
                playerObject.TakeDamage(1);
            }
        }

        protected override float GetInvulnurableTime()
        {
            return .2f;
        }
        protected override void ResolveCoinCollision(Vector2 coinGridPosition)
        {
            return;
        }

        protected override void ResolveSpikeCollision()
        {
            TakeDamage(1);
        }
    }
}
