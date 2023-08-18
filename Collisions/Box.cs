using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Development_Project.Collisions
{
    public class Box
    {
        public enum BoxType
        {
            None,
            // Static = solid tiles
            Static,
            Coin,
            Spike,
            // De volgende 3 enum waardes zijn dynamisch bewegende objecten
            Player,
            Enemy,
            Projectile,
        }


        public Vector2 Position { get { return _position; } set { _position = value; } }
        private Vector2 _position;

        public Vector2 Size { get { return _size; } set { _size = value; } }
        private Vector2 _size;

        public Vector2 Velocity { get { return _velocity; } set { _velocity = value; } }
        private Vector2 _velocity;

        public BoxType Type { get; set; }

        // Deze data object stelt boxen in staat om info over zichzelf op te slaan
        // bv: elk object slaat een verwijzing naar zichzelf op in zijn box en later kunnen andere objecten de informatie rechtstreeks uit het box halen
        public object Data { get { return _data; } set { _data = value; } }
        private object _data;

        public Box(Vector2 position, Vector2 size, BoxType type)
        {
            _position = position;
            _size = size;
            _velocity = Vector2.Zero;
            Type = type;
        }

        // Stuurt een raycast voor rayOrigin in de richting van rayDirection
        // Geeft true als de ray het box raakt/ziet en false wanneer niet.
        // Geef het raakpunt terug a.k.a waar de ray hit
        public bool RayCast(Vector2 rayOrigin, Vector2 rayDirection, out Vector2 contactPoint, out Vector2 contactNormal, out float hitNear)
        {
            hitNear = 0;
            contactPoint = default;
            contactNormal = default;

            Vector2 invdir = new(1f / rayDirection.X, 1f / rayDirection.Y);

            Vector2 near = (_position - rayOrigin) * invdir;
            Vector2 far = (_position + _size - rayOrigin) * invdir;

            if (float.IsNaN(near.X) || float.IsNaN(near.Y) || float.IsNaN(far.X) || float.IsNaN(far.Y))
            {
                return false;
            }

            // Sort near en far x and y
            if (near.X > far.X)
            {
                (far.X, near.X) = (near.X, far.X);
            }

            if (near.Y > far.Y)
            {
                (far.Y, near.Y) = (near.Y, far.Y);
            }

            if (near.X > far.Y || near.Y > far.X)
            {
                return false;
            }

            hitNear = MathF.Max(near.X, near.Y);
            float hitfar = MathF.Min(far.X, far.Y);

            if (hitfar < 0)
            {
                return false;
            }

            contactPoint = rayOrigin + (hitNear * rayDirection);

            contactNormal = near.X > near.Y
                ? invdir.X < 0 ? new Vector2(1, 0) : new Vector2(-1, 0)
                : invdir.Y < 0 ? new Vector2(0, 1) : new Vector2(0, -1);

            return true;
        }

        // Geeft true wanneer deze box een andere box raakt (target)
        public bool Cast(Box target, out Vector2 contactPoint, out Vector2 contactNormal, out float hitNear, float deltaTime)
        {
            contactPoint = default;
            contactNormal = default;
            hitNear = float.PositiveInfinity;

            if (_velocity.X == 0 && _velocity.Y == 0)
            {
                return false;
            }

            Box expandedTarget = new(target.Position - (Size / 2f), target.Size + Size, BoxType.None);

            return expandedTarget.RayCast(Position + (Size / 2f), _velocity * deltaTime, out contactPoint, out contactNormal, out hitNear) && hitNear >= 0f && hitNear < 1f;
        }
    }
}
