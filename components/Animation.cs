using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace MonoRPG
{
    public class AnimationController : Component
    {
        SpriteRenderer spriteRenderer;

        enum State { PLAYING, PAUSED }
        State currentState = State.PLAYING;

        public Dictionary<string, Animation> Animations = new Dictionary<string, Animation>();
        Animation currentAnimation;
        List<Point> frames = new List<Point>();

        int currentFrame;
        int numberOfFrames;
        int framesWidth = 0;
        int framesHeight = 0;

        float animationSpeed = 0;
        float currentFrameTime = 0;

        public AnimationController(Entity _owner) : base(_owner)
        {
            owner.AddComponent<AnimationController>(this);
        }

        public override void Initialize()
        {
            spriteRenderer = owner.GetComponent<SpriteRenderer>() as SpriteRenderer;

            SplitTexture();
        }

        public override void Update(float deltaTime)
        {
            if (currentAnimation == null)
                return;
            
            if (currentState == State.PLAYING)
            {
                if (currentFrameTime >= animationSpeed)  // It is time to go to next frame
                {
                    if (currentAnimation.CurrentFrameIndex == currentAnimation.TotalFrames) // The animation has completed it's cycle
                    {
                        currentAnimation.OnComplete.Emit();
                        if (!currentAnimation.IsRepeating)
                        {
                            // stop the animation
                        }
                    }

                    currentFrame = currentAnimation.GetNextFrame();
                    spriteRenderer.SetTextureFrame(frames[currentFrame].X, frames[currentFrame].Y);
                    currentFrameTime = 0;
                }

                currentFrameTime += deltaTime;
            }
        }

        ///<summary>
        /// Add a new animation to the controller, the name variable is used to access this animation in the dictonary.
        ///</summary>
        public void Add(string _name, int[] _frames, float _speed = 8, bool _isRepeating = true) 
        {
            Animation animation = new Animation(_name, _frames, _isRepeating, _speed, numberOfFrames);
            Animations.Add(animation.Name, animation);

            if (currentAnimation == null)
            {
                currentAnimation = animation;
                animationSpeed = currentAnimation.Speed;
            }
        }

        ///<summary>
        /// Set animation to play by giving the name of the animation
        ///</summary>
        public void Play(string _animationName) 
        {
            if (currentAnimation.Name == _animationName)  // animation is already playing, so get out of this function.
                return;
            
            currentAnimation = Animations[_animationName];
            currentFrame = Animations[_animationName].Frames[0];
            animationSpeed = currentAnimation.Speed;
            spriteRenderer.SetTextureFrame(frames[currentFrame].X, frames[currentFrame].Y);
            currentState = State.PLAYING;
        }

        public void Pause() 
        {
            if (currentState == State.PLAYING)
            {
                currentState = State.PAUSED;
            }
            else if (currentState == State.PAUSED)
            {
                currentState = State.PLAYING;
            }
        }

        private Point GetAnimationPosition(int _frame)
        {

            return new Point();
        }

        ///<summary>
        /// Animations are created from one large sprite. This doesn't so much spilt it, but gets all the origin points of each animation frame so when the
        /// frame needs to be played, it gets it's origin point and sets it in a rectangle.
        ///</summary>
        private void SplitTexture()
        {
            int textureWidth = spriteRenderer.Texture.Width;
            int textureHeight = spriteRenderer.Texture.Height;

            framesWidth = textureWidth / spriteRenderer.Size.X;
            framesHeight = textureHeight / spriteRenderer.Size.Y;
            numberOfFrames = framesHeight * framesWidth;

            for (int y = 0; y < framesHeight; y++)
            {
                for (int x = 0; x < framesWidth; x++)
                {
                    frames.Add(new Point(x * spriteRenderer.Size.X, y * spriteRenderer.Size.Y));
                }
            }
        }

        public class Animation
        {
            public string Name { get; private set; }
            public int[] Frames { get; private set; }
            public int TotalFrames { get; private set; }
            public bool IsRepeating { get; private set; }
            public float Speed { get; private set; }
            public int CurrentFrameIndex { get; set; }
            public Signal OnComplete { get; set; }

            public Animation(string _name, int[] _frames, bool _isRepeating, float _speed, int _maxFrames)
            {
                Name = _name;
                Frames = _frames;
                TotalFrames = Frames.Length - 1;
                IsRepeating = _isRepeating;
                Speed = 1 / _speed;
                OnComplete = new Signal();
                SetIncorrectFrames(_maxFrames);
            }

            ///<summary>
            /// If an incorrect frame is given during the creation of an animation, then it is set to zero so it won't give any errors.
            ///</summary>
            private void SetIncorrectFrames(int _maxFrames)
            {
                for (int i = 0; i < Frames.Length; ++i)
                {
                    if (Frames[i] < 0 || Frames[i] >= _maxFrames)
                    {
                        System.Console.WriteLine("Incorrect correct frame given for " + Name + " animation");
                        Frames[i] = 0;
                    }
                }
            }

            ///<summary>
            /// Increase the frame, if frame over the total number of frames then set it back to 0.
            ///</summary>
            public int GetNextFrame()
            {
                CurrentFrameIndex++;

                if (CurrentFrameIndex >= Frames.Length)
                {
                    CurrentFrameIndex = 0;
                }

                return Frames[CurrentFrameIndex];
            }
        }
    }
}