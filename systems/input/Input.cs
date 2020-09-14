using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoRPG
{
    public class Input
    {
        KeyboardState currentKeyState;
        KeyboardState oldKeyState;
        MouseState currentMouseState;
        MouseState oldMouseState;
        Keys lastKeyPressed = Keys.None;

        public enum MouseButton { LEFT, RIGHT, MIDDLE }

        public bool IsKeyPressed(Keys _key)
        {
            currentKeyState = Keyboard.GetState();

            if (currentKeyState.IsKeyDown(_key))
            {
                return true;
            }

            return false;
        }

        public bool IsKeyJustPressed(Keys _key)
        {
            currentKeyState = Keyboard.GetState();

            if (lastKeyPressed != _key && currentKeyState.IsKeyDown(_key))
            {
                lastKeyPressed = _key;
                return true;
            }

            return false;
        }

        public static bool IsKeyReleased(Keys _key)
        {
            return false;
        }

        public bool IsMouseButtonJustPressed(MouseButton _mouseButton)
        {
            currentMouseState = Mouse.GetState();

            if (_mouseButton == MouseButton.LEFT)
            {
                if (oldMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    oldMouseState = currentMouseState;
                    return true;
                }

                oldMouseState = currentMouseState;
                return false;
            }
            else if (_mouseButton == MouseButton.RIGHT)
            {
                if (oldMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed)
                {
                    oldMouseState = currentMouseState;
                    return true;
                }

                oldMouseState = currentMouseState;
                return false;
            }

            return false;
        }

        public Vector2 GetMousePosition()
        {
            MouseState currentMouseState = Mouse.GetState();

            return new Vector2(currentMouseState.X / Screen.Scale, currentMouseState.Y / Screen.Scale);
        }

        public Vector2 GetMouseWorldPosition()
        {
            return Screen.Camera.ScreenToWorld(GetMousePosition());
        }

        public Point GetMouseGridPosition()
        {
            Vector2 mouseWorldPosition = GetMouseWorldPosition();

            return new Point((int)Math.Floor(mouseWorldPosition.X / 16), (int)Math.Floor(mouseWorldPosition.Y / 16));
        }
    }
}