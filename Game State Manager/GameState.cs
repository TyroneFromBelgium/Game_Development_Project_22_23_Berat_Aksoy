using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Development_Project.Game_State_Management
{
    // Representeert een scene
    public class GameState
    {
        protected List<GameComponent> _components;

        protected readonly GameStateManager _stateManager;
        protected readonly Game1 _gameRef;

        // Public proptery om child componenten te accessen.
        public List<GameComponent> Components { get { return _components; } }

        public GameState(Game1 gameRef, GameStateManager stateManager)
        {
            _gameRef = gameRef;
            _stateManager = stateManager;

            _components = new List<GameComponent>();
        }

        // Aangeroepen wanneer de staat wordt ge-entered
        public virtual void Enter()
        {
            Debug.WriteLine("Entering state... " + this);

            _components = new List<GameComponent>();
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var child in _components)
            {
                if (child.Enabled)
                {
                    child.Update(gameTime);
                }
            }
        }

        protected void BeginSpriteBatch()
        {
            // We roepen het begin aan met de volgende parameters omdat het een pixel art spel is :D
            _gameRef.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
        }
        protected void EndSpriteBatch()
        {
            _gameRef.SpriteBatch.End();
        }
        public virtual void Draw(GameTime gameTime)
        {
            // Teken alle componenten als ze tekenbaar en zichbaar zijn
            foreach (var child in _components)
            {
                if (child is DrawableGameComponent component && component.Visible)
                {
                    component.Draw(gameTime);
                }
            }
        }

        // Aangeroepen wanneer de staat wordt ge-exit
        public virtual void Exit()
        {
            Debug.WriteLine("Exiting State... " + this);
        }

        // Toevoegen en verwijderen van child componenten
        public void AddComponent(GameComponent component)
        {
            _components.Add(component);
        }
        public void RemoveComponent(GameComponent component)
        {
            _components?.Remove(component);
        }
    }
}
