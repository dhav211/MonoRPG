using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoRPG
{
    public class CameraController
    {
        Camera camera;

        bool canMove = false;
        Vector2 initialPosition = new Vector2();
        private const float MOVE_SPEED = 150f;
        private const float MAX_DISTANCE = 100f;
        private const float RETURN_SPEED = 150f;

        public CameraController(Camera _camera)
        {
            camera = _camera;
        }

        public void Update(float deltaTime)
        {
            if (Input.IsKeyJustPressed(Keys.C))
            {
                initialPosition = camera.Position;
                canMove = true;
            }
            if (Input.IsKeyReleased(Keys.C))
            {
                camera.ScrollToPosition(initialPosition, RETURN_SPEED);
                canMove = false;
            }

            if (canMove)
                MoveCamera(deltaTime);
        }

        private void MoveCamera(float deltaTime)
        {
            Vector2 mousePosition = Input.GetMousePosition();

            if (mousePosition.X > Screen.Width * .80 && camera.Position.X < initialPosition.X + MAX_DISTANCE)
            {
                camera.X += MOVE_SPEED * deltaTime;
            }
            else if (mousePosition.X < Screen.Width * .20 && camera.Position.X > initialPosition.X - MAX_DISTANCE)
            {
                camera.X -= MOVE_SPEED * deltaTime;
            }

            if (mousePosition.Y > Screen.Height * .80 && camera.Position.Y < initialPosition.Y + MAX_DISTANCE)
            {
                camera.Y += MOVE_SPEED * deltaTime;
            }
            else if (mousePosition.Y < Screen.Height * .20 && camera.Position.Y > initialPosition.Y - MAX_DISTANCE)
            {
                camera.Y -= MOVE_SPEED * deltaTime;
            }
        }
    }
}