using Game_Development_Project.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Development_Project.Levels
{
    public class Level
    {
        public List<string> LevelString { get {  return _levelString; }  set { _levelString = new List<string>(value); } }
        public Vector2 PlayerSpawnPosition { get { return _playerSpawnPosition; } }
        public Vector2 DoorSpawnPosition { get { return _doorSpawnPosition; } }
        public List<Vector2> PatrollingEnemySpawnPositions { get { return _patrollingEnemySpawnPositions; } }
        public List<Vector2> ShootingEnemySpawnPositions { get { return _shootingEnemySpawnPositions; } }
        public int LevelWidth => _levelString[0].Length;
        public int LevelHeight => _levelString.Count;

        public List<Box> StaticTiles { get; private set; }
        public List<Box> CoinTiles { get; private set; }
        public List<Box> SpikeTiles { get; private set; }
        public List<Box> Dynamics { get; private set; }

        public const int TileWidth = 32;
        public const int TileHeight = 32;

        // De textuur moet 4 sprites bevatten, alle sprites moeten 32x32 zijn
        // op index 0,0 -> Grond
        // 32,0 -> Muur
        // 64, 0 -> Spike (trap/val)
        // 96, 0 -> Coin
        private Texture2D _tilesetTexture;
        private List<string> _levelString;

        private Texture2D _bgTexture;

        // Spawnposities van dynamische objecten, deze worden gebruikt in de gameplay state om ze te spawnen.
        private Vector2 _playerSpawnPosition;
        private List<Vector2> _patrollingEnemySpawnPositions;
        private List<Vector2> _shootingEnemySpawnPositions;

        private Vector2 _doorSpawnPosition;

        public Level(List<string> levelString, Texture2D tilesetTexture, Texture2D bgTexture)
        {
            _levelString = new List<string>(levelString);
            _tilesetTexture = tilesetTexture;

            _bgTexture = bgTexture;

            StaticTiles = new List<Box>();
            CoinTiles = new List<Box>();
            SpikeTiles = new List<Box>();
            Dynamics = new List<Box>();

            _patrollingEnemySpawnPositions = new List<Vector2>();
            _shootingEnemySpawnPositions = new List<Vector2>();

            for (int x = 0; x < LevelWidth; x++)
            {
                for (int y = 0; y < LevelHeight; y++)
                {
                    Vector2 pos = new Vector2(x * TileWidth, y * TileHeight);
                    Vector2 size = new Vector2(TileWidth, TileHeight);

                    // Voeg tiles/tegels toe aan hun lijst op basis van hun type.
                    if (GetTile(x, y) == 'W' || GetTile(x, y) == 'G')
                    {
                        StaticTiles.Add(new Box(pos, size, Box.BoxType.Static));
                    }
                    else if (GetTile(x, y) == 'C')
                    {
                        CoinTiles.Add(new Box(pos, size, Box.BoxType.Coin));
                    }
                    else if (GetTile(x, y) == 'S')
                    {
                        SpikeTiles.Add(new Box(pos, size, Box.BoxType.Spike));
                    }
                    else if (GetTile(x, y) == 'P')
                    {
                        _playerSpawnPosition = pos;
                    }
                    else if (GetTile(x, y) == 'Z')
                    {
                        _patrollingEnemySpawnPositions.Add(pos);
                    }
                    else if (GetTile(x, y) == 'X')
                    {
                        _shootingEnemySpawnPositions.Add(pos);
                    }
                    else if (GetTile(x, y) == 'D')
                    {
                        _doorSpawnPosition = pos;
                    }
                }
            }
        }
        // Dit moet worden opgeroepen vanuit de gameplay state bij het spawnen van dynamische objecten
        // Objecten zullen de lijst met dynamics gebruiken om colission met andere dynamische objecten op te lossen.
        public void AddDynamic(Box dynamic)
        {
            Dynamics.Add(dynamic);
        }

        public char GetTile(int x, int y)
        {
            if (x >= 0 && x < LevelWidth && y >= 0 && y < LevelHeight)
            {
                return _levelString[y][x];
            }
            else
            {
                return ' ';
            }
        }
        public void SetTile(int x, int y, char c)
        {
            if (x >= 0 && x < LevelWidth && y >= 0 && y < LevelHeight)
            {
                var horizontal = _levelString[y].ToCharArray();
                horizontal[x] = c;
                _levelString[y] = new string(horizontal);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_bgTexture, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 3f,
                SpriteEffects.None, 0f);

            // Teken tegels/tiles
            for (int x = 0; x < LevelWidth; x++)
            {
                for (int y = 0; y < LevelHeight; y++)
                {
                    Vector2 pos = new Vector2(x * TileWidth, y * TileHeight);
                    switch (GetTile(x, y))
                    {
                        case 'G':
                            spriteBatch.Draw(_tilesetTexture, pos, new Rectangle(0, 0, TileWidth, TileHeight), Color.White);
                            break;
                        case 'W':
                            spriteBatch.Draw(_tilesetTexture, pos, new Rectangle(32, 0, TileWidth, TileHeight), Color.White);
                            break;
                        case 'S':
                            spriteBatch.Draw(_tilesetTexture, pos, new Rectangle(64, 0, TileWidth, TileHeight), Color.White);
                            break;
                        case 'C':
                            spriteBatch.Draw(_tilesetTexture, pos, new Rectangle(96, 0, TileWidth, TileHeight), Color.White);
                            break;
                        case 'D':
                            spriteBatch.Draw(_tilesetTexture, pos, new Rectangle(128, 0, TileWidth, TileHeight), Color.White);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
