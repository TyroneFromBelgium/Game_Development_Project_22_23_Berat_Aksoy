using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Development_Project.Input_Handling
{
    public class InputHandler : GameComponent
    {
        public enum MouseButton { Right, Left };

        // Toetsenboard staat in deze frame
        private static KeyboardState keyboardState;
        // Toetsenboard staat in vorige frame
        private static KeyboardState lastFrameKeyboardState;

        // Muis staat in deze frame
        private static MouseState mouseState;
        // Muis staat in vorige frame
        private static MouseState lastFrameMouseState;

        public InputHandler(Game game) : base(game)
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
        }

        public override void Update(GameTime gameTime)
        {
            // Set de laatste frames van muis en toetsenbord staten van de vorige update.
            lastFrameKeyboardState = keyboardState;
            lastFrameMouseState = mouseState;

            // Update beide muis en toetsenbord staten
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            base.Update(gameTime);
        }

        // Retourneert true als een key in het huidige frame wordt ingedrukt en niet in het vorige frame.
        public static bool KeyHeldDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }
        public static bool KeyPress(Keys key)
        {
            return KeyHeldDown(key) && lastFrameKeyboardState.IsKeyUp(key);
        }

        // Retourneert true als een muis in het huidige frame wordt ingedrukt en niet in het vorige frame.
        public static bool MousePress(MouseButton button)
        {
            switch(button)
            {
                case MouseButton.Right:
                    return mouseState.RightButton == ButtonState.Pressed &&
                           lastFrameMouseState.RightButton == ButtonState.Released;
                case MouseButton.Left:
                    return mouseState.LeftButton == ButtonState.Pressed &&
                           lastFrameMouseState.LeftButton == ButtonState.Released;
                default:
                    return false;
            }
        }
        public static Point GetMousePosition()
        {
            return mouseState.Position;
        }
    }
}
