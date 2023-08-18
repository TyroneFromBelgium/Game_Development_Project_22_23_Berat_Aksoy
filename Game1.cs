using Game_Development_Project.Animations;
using Game_Development_Project.ContentLoading;
using Game_Development_Project.Game_State_Management;
using Game_Development_Project.Game_State_Management.States;
using Game_Development_Project.Input_Handling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Development_Project
{
    public class Game1 : Game
    {
        public SpriteBatch SpriteBatch { get { return _spriteBatch; } }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameStateManager _gameStateManager;

        // Game States
        public MainMenu MainMenu { get; private set; }
        public LoseState LoseState { get; private set; }
        public WinState WinState { get; private set; }
        public Level1 Level1 { get; private set; }
        public Level2 Level2 { get; private set; }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            // Stel de scherm breedte en hoogte in, en pas toe
            _graphics.PreferredBackBufferWidth = Globals.ScreenWidth;
            _graphics.PreferredBackBufferHeight = Globals.ScreenHeight;
            _graphics.ApplyChanges();

            // Set  globals graphics device
            Globals.Instance.GraphicsDevice = GraphicsDevice;

            //Voeg de Input Manager toe als een component
            Components.Add(new InputHandler(this));

            // Initialiseer de gamestatemanager en de staten
            _gameStateManager = new GameStateManager(this);
            MainMenu = new MainMenu(this, _gameStateManager);
            LoseState = new LoseState(this, _gameStateManager);
            WinState = new WinState(this, _gameStateManager);
            Level1 = new Level1(this, _gameStateManager);
            Level2 = new Level2(this, _gameStateManager);

            // Voeg de gamestatemanager toe aan de lijst met componenten zodat het automatisch wordt bijgewerkt en getekend.
            Components.Add(_gameStateManager);
            // Stel de initiële toestand in om te laden.
            _gameStateManager.SetState(MainMenu);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.Instance.SpriteBatch = _spriteBatch;

            // Initialiseer de content loader
            ContentLoader.Initialize(Content);

            // Laden van alle texturen hier aan het begin van het spel.
            // Sprite Font
            ContentLoader.LoadSpriteFont("Font");
            // Textures
            ContentLoader.LoadTexture("Heart", "Heart");
            ContentLoader.LoadTexture("Tileset", "Tileset");
            ContentLoader.LoadTexture("Button", "Button");
            ContentLoader.LoadTexture("Background0", "bg0");
            ContentLoader.LoadTexture("Background1", "bg1");
            ContentLoader.LoadTexture("PlayerIdle", "PlayerIdle");
            ContentLoader.LoadTexture("PlayerRun", "PlayerRun");
            ContentLoader.LoadTexture("PlayerInAir", "PlayerInAir");
            ContentLoader.LoadTexture("PlayerAttack", "PlayerAttack");
            ContentLoader.LoadTexture("PatrollingEnemyRun", "PatrollingEnemyRun");
            ContentLoader.LoadTexture("ShootingEnemyRun", "ShootingEnemyRun");
            ContentLoader.LoadTexture("ShootingEnemyAttack", "ShootingEnemyAttack");
            ContentLoader.LoadTexture("Fireball", "Fireball");

            // Speel muziek
            var music = Content.Load<Song>("The Vile Grove LOOP");
            MediaPlayer.Volume = .5f;
            MediaPlayer.Play(music);
            MediaPlayer.IsRepeating = true;
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}