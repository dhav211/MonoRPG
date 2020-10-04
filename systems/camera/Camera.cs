using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoRPG
{
    public class Camera
    {
        public enum State { FREE, SCROLLING, LOCKED }
        public State CurrentState { get; set; } = State.FREE;

        private CameraController controller;

        private Matrix transformationMatrix = Matrix.Identity;
        private Matrix inverseMatrix = Matrix.Identity;
        private Matrix textTransformationMatrix = Matrix.Identity;
        private Matrix textInverseMatrix = Matrix.Identity;
        private Vector2 position = Vector2.Zero;
        private float rotation = 0;
        private Vector2 zoom = Vector2.One;
        private Vector2 origin = Vector2.Zero;
        private bool hasChanged;

        public Viewport Viewport { get; set; }

        Tween tween = new Tween();

        public Action _onFinishedScrolling { get; private set; }

        public Matrix TransformationMatrix
        {
            get
            {
                if (hasChanged)
                    UpdateMatrices();
                
                return transformationMatrix;
            }
        }

        public Matrix InverseMatrix
        {
            get
            {
                if (hasChanged)
                    UpdateMatrices();
                
                return inverseMatrix;
            }
        }

        public Matrix TextTransformationMatrix
        {
            get
            {
                if (hasChanged)
                    UpdateMatrices();
                
                return textTransformationMatrix;
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set 
            {
                if (position == value) { return; }

                position = value;

                hasChanged = true;
            }
        }

        public float Rotation
        {
            get { return rotation; }
            set
            {
                if (rotation == value) { return; }

                rotation = value;
                hasChanged = true;
            }
        }

        public Vector2 Zoom
        {
            get { return zoom; }
            set
            {
                if (zoom == value) { return; }

                zoom = value;
                hasChanged = true;
            }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set
            {
                if (origin == value) { return; }

                origin = value;
                hasChanged = true;
            }
        }

        public float X
        {
            get { return position.X; }
            set
            {
                if (position.X == value) { return; }

                position.X = value;
                hasChanged = true;
            }
        }

        public float Y
        {
            get { return position.Y; }
            set
            {
                if (position.Y == value) { return; }

                position.Y = value;
                hasChanged = true;
            }
        }

        public Camera(Viewport _viewport)
        {
            Viewport = _viewport;
            controller = new CameraController(this);
            Screen.SetCamera(this);
            _onFinishedScrolling = onFinishedScrolling;
            tween.OnComplete.Add("camera", _onFinishedScrolling);
        }

        public Camera(int _width, int _height)
        {
            Viewport = new Viewport(0, 0, _width, _height);
            controller = new CameraController(this);
            Screen.SetCamera(this);
            _onFinishedScrolling = onFinishedScrolling;
            tween.OnComplete.Add("camera", _onFinishedScrolling);
        }

        public void Update(float deltaTime)
        {
            if (CurrentState == State.SCROLLING)
            {
                Position = tween.TweenVector2(Position, deltaTime);
            }
            else if (CurrentState == State.FREE)
            {
                controller.Update(deltaTime);
            }
        }

        private void UpdateMatrices()
        {
            Matrix positionTranslationMatrix = Matrix.CreateTranslation(new Vector3()
            {
                X = -(int)Math.Floor(position.X),
                Y = -(int)Math.Floor(position.Y),
                Z = 0
            });

            Matrix rotationMatrix = Matrix.CreateRotationZ(rotation);

            Matrix scaleMatrix = Matrix.CreateScale(new Vector3()
            {
                X = zoom.X,
                Y = zoom.Y,
                Z = 1
            });

            Matrix originTranslationMatrix = Matrix.CreateTranslation(new Vector3()
            {
                X = (int)Math.Floor(origin.X),
                Y = (int)Math.Floor(origin.Y),
                Z = 0
            });

            transformationMatrix = positionTranslationMatrix * rotationMatrix * scaleMatrix * originTranslationMatrix;

            inverseMatrix = Matrix.Invert(transformationMatrix);

            Matrix textPositionTranslationMatrix = Matrix.CreateTranslation(new Vector3()
            {
                X = -(int)Math.Floor(position.X) * 4,
                Y = -(int)Math.Floor(position.Y) * 4,
                Z = 0
            });

            Matrix textRotationMatrix = Matrix.CreateRotationZ(rotation);

            Matrix textScaleMatrix = Matrix.CreateScale(new Vector3()
            {
                X = zoom.X,
                Y = zoom.Y,
                Z = 1
            });

            Matrix textOriginTranslationMatrix = Matrix.CreateTranslation(new Vector3()
            {
                X = (int)Math.Floor(origin.X) * 4,
                Y = (int)Math.Floor(origin.Y) * 4,
                Z = 0
            });

            textTransformationMatrix = textPositionTranslationMatrix * textRotationMatrix * textScaleMatrix * textOriginTranslationMatrix;
            
            textInverseMatrix = Matrix.Invert(textTransformationMatrix);

            hasChanged = false;
        }

        public Vector2 ScreenToWorld(Vector2 _position)
        {
            return Vector2.Transform(_position, InverseMatrix);
        }

        public Vector2 WorldToScreen(Vector2 _position)
        {
            return Vector2.Transform(_position, TransformationMatrix);
        }

        ///<summary>
        /// Check to see if the given entity is within the camera range
        ///</summary>
        public bool IsEntityOutOfBounds(Entity _entity)
        {
            Transform entityPosition = _entity.GetComponent<Transform>() as Transform;
            // Set the cameras position as center of the screen opposed to its usual top left
            Vector2 adjustedCameraPosition = new Vector2(Position.X + (Screen.Width / 2), Position.Y + (Screen.Height / 2));

            Vector2 distance = entityPosition.Position - adjustedCameraPosition;
            distance = new Vector2(Math.Abs(distance.X), Math.Abs(distance.Y));

            if (distance.X > Screen.Width / 2 || distance.Y > Screen.Height / 2)
                return true;

            return false;
        }

        public void Scroll(Vector2 _direction, float _distance, float _duration)
        {
            tween.SetTween(Position, Position + (_direction * _distance), _duration, Tween.EaseType.EASE_OUT);
            tween.Start();
            CurrentState = State.SCROLLING;
        }

        public void ScrollToPosition(Vector2 _position, float _speed)
        {
            float distance = Vector2.Distance(Position, _position);

            tween.SetTween(Position, _position, distance / _speed, Tween.EaseType.EASE_OUT);
            tween.Start();
            CurrentState = State.SCROLLING;
        }

        public void onFinishedScrolling()
        {
            CurrentState = State.FREE;
        }
    }
}