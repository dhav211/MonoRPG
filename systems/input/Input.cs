using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoRPG
{
    public static class Input
    {
        static KeyboardState currentKeyState;
        static KeyboardState oldKeyState;
        static MouseState currentMouseState;
        static MouseState oldMouseState;

        public enum MouseButton { LEFT, RIGHT, MIDDLE }

        public static void GetKeyboardState()
        {
            oldKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
        }

        public static void GetMouseState()
        {
            oldMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }

        public static bool IsKeyPressed(Keys _key)
        {
            return currentKeyState.IsKeyDown(_key);
        }

        public static bool IsKeyJustPressed(Keys _key)
        {
            return currentKeyState.IsKeyDown(_key) && !oldKeyState.IsKeyDown(_key);
        }

        public static bool IsKeyReleased(Keys _key)
        {
            return currentKeyState.IsKeyUp(_key) && !oldKeyState.IsKeyUp(_key);
        }

        public static bool IsMouseButtonPressed(MouseButton _mouseButton)
        {
            if (_mouseButton == MouseButton.LEFT)
            {
                return currentMouseState.LeftButton == ButtonState.Pressed;
            }
            else if (_mouseButton == MouseButton.RIGHT)
            {
                return currentMouseState.RightButton == ButtonState.Pressed;
            }

            return false;
        }

        public static bool IsMouseButtonJustPressed(MouseButton _mouseButton)
        { // TODO: odd behavior here. If this function is called twice in a row it cancels the Right button out.

            if (_mouseButton == MouseButton.LEFT)
            {
                return currentMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != ButtonState.Pressed;
            }
            else if (_mouseButton == MouseButton.RIGHT)
            {
                return currentMouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton != ButtonState.Pressed;
            }

            return false;
        }

        public static Vector2 GetMousePosition()
        {
            MouseState currentMouseState = Mouse.GetState();

            return new Vector2(currentMouseState.X / Screen.Scale, currentMouseState.Y / Screen.Scale);
        }

        public static Vector2 GetMouseWorldPosition()
        {
            return Screen.Camera.ScreenToWorld(GetMousePosition());
        }

        public static Point GetMouseGridPosition()
        {
            Vector2 mouseWorldPosition = GetMouseWorldPosition();

            return new Point((int)Math.Floor(mouseWorldPosition.X / 16), (int)Math.Floor(mouseWorldPosition.Y / 16));
        }

        public static bool IsMouseInClickRange()
        {
            Vector2 mousePos = GetMousePosition();

            if (mousePos.Y < Screen.Height - 24)
                return true;

            return false;
        }
    }
}