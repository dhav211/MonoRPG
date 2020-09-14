using Microsoft.Xna.Framework;

namespace MonoRPG
{
    public static class Screen
    {
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        public static int Scale { get; private set; }
        public static Camera Camera { get; private set; }

        public static void SetWidthHeight(int _width, int _height)
        {
            Width = _width;
            Height = _height;
        }

        public static void SetScale(int _scale)
        {
            Scale = _scale;
        }

        public static void SetCamera(Camera _camera)
        {
            Camera = _camera;
        }

        public static Vector2 ScreenToWorld(Vector2 position)
        {
            return Vector2.Transform(position, Camera.InverseMatrix);
        }

        public static Vector2 WorldToScreen(Vector2 position)
        {
            return Vector2.Transform(position, Camera.TransformationMatrix);
        }
    }
}