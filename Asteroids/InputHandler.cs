using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Asteroids
{
    class InputHandler
    {
        /// <summary>
        /// This class gets user input from Keyboard and Mouse, 
        /// it could extended to get input from the game pad too
        /// </summary>
        private KeyboardState prevKeyboardState;
        private KeyboardState keyboardState;

        private MouseState prevMouseState;
        private MouseState mouseState;

        private GamePadState prevGamePadState;
        private GamePadState gamePadState;

        private int mouseX, mouseY;

        public InputHandler()
        {
            prevKeyboardState = Keyboard.GetState();
            prevMouseState = Mouse.GetState();
        }

        // keyboard stuff
        public bool IsKeyDown(Keys key)
        {
            return (keyboardState.IsKeyDown(key));
        }

        public bool IsHoldingKey(Keys key)
        {
            return (keyboardState.IsKeyDown(key) &&
                prevKeyboardState.IsKeyDown(key));
        }

        public bool WasKeyPressed(Keys key)
        {
            return (keyboardState.IsKeyDown(key) &&
                prevKeyboardState.IsKeyUp(key));
        }

        public bool HasReleasedKey(Keys key)
        {
            return (keyboardState.IsKeyUp(key) &&
                prevKeyboardState.IsKeyDown(key));
        }

        // mouse stuff
        public Vector2 getMousePos()
        {
            Vector2 v;
            v.X = mouseX;
            v.Y = mouseY;
            return v;
        }

        public bool isMouseButtonDown(int i)
        {
            if (i == 1)
            {
                // returns the state of the left mouse button
                if (mouseState.LeftButton == ButtonState.Pressed) { return true; }
            }
            else if (i == 2)
            {
                // returns the state of the right mouse button
                if (mouseState.RightButton == ButtonState.Pressed) { return true; }
            }
        
            return false;

        }

        public bool isMouseButtonClick(int i)
        {
            if (i == 1)
            {
                // return true only directly after a mouse down click
                if (mouseState.LeftButton == ButtonState.Released &&
               prevMouseState.LeftButton == ButtonState.Pressed) return true;
            }
            if (i == 2)
            {
                // return true only directly after a mouse down click
                if (mouseState.RightButton == ButtonState.Released &&
               prevMouseState.RightButton == ButtonState.Pressed)
                {
                    return true;
                }
            }
            return false;

        }

        // gamepad stuff
        public bool IsButtonDown(Buttons button)
        {
            return (gamePadState.IsButtonDown(button));
        }

        public bool IsHoldingButton(Buttons button)
        {
            return (gamePadState.IsButtonDown(button) &&
                prevGamePadState.IsButtonDown(button));
        }

        public bool WasButtonPressed(Buttons button)
        {
            return (gamePadState.IsButtonDown(button) &&
                prevGamePadState.IsButtonUp(button));
        }

        public bool HasReleasedButton(Buttons button)
        {
            return (gamePadState.IsButtonUp(button) &&
                 prevGamePadState.IsButtonUp(button));
        }

        public float UpdateThumbStickAngle()
        {
            float angle = 0.0f;
            //returns the angle of the thumbstick so the ship can angle itself properly.
            gamePadState.ThumbSticks.Left.Normalize();
            if ((gamePadState.ThumbSticks.Left.X > 0 || gamePadState.ThumbSticks.Left.X < 0) || (gamePadState.ThumbSticks.Left.Y > 0 || gamePadState.ThumbSticks.Left.Y < 0))
            {
                angle = (float)Math.Atan2(gamePadState.ThumbSticks.Left.X, gamePadState.ThumbSticks.Left.Y);
            }
            return angle;
        }
        // the update method, must be called from the main game.Update function
        public void Update()
        {
            //set our previous state to our new state
            prevKeyboardState = keyboardState;

            //get our new state
            keyboardState = Keyboard.GetState();

            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
            mouseX = mouseState.X;
            mouseY = mouseState.Y;

            prevGamePadState = gamePadState;
            gamePadState = GamePad.GetState(0);
        }
    }
}
