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
        MouseState currentLeftMouseState;
        MouseState currentRightMouseState;
        MouseState oldLeftMouseState;
        MouseState oldRightMouseState;
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

        public bool IsMouseButtonPressed(MouseButton _mouseButton)
        {
            currentMouseState = Mouse.GetState();

            if (_mouseButton == MouseButton.LEFT)
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    return true;
                }
            }
            else if (_mouseButton == MouseButton.RIGHT)
            {
                if (currentMouseState.RightButton == ButtonState.Pressed)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsMouseButtonJustPressed(MouseButton _mouseButton)
        { // TODO: odd behavior here. If this function is called twice in a row it cancels the Right button out.

            if (_mouseButton == MouseButton.LEFT)
            {
                currentLeftMouseState = Mouse.GetState();

                if (oldLeftMouseState.LeftButton == ButtonState.Released && currentLeftMouseState.LeftButton == ButtonState.Pressed)
                {
                    oldLeftMouseState = currentLeftMouseState;
                    return true;
                }

                oldLeftMouseState = currentLeftMouseState;
                return false;
            }
            else if (_mouseButton == MouseButton.RIGHT)
            {
                currentRightMouseState = Mouse.GetState();

                if (oldRightMouseState.RightButton == ButtonState.Released && currentRightMouseState.RightButton == ButtonState.Pressed)
                {
                    oldRightMouseState = currentRightMouseState;
                    return true;
                }

                oldRightMouseState = currentRightMouseState;
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