using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoRPG
{
    public class Tween
    {
        public enum State {PAUSED, STOPPED, FINISHED, RUNNING, NOT_RUNNING}
        public State CurrentState { get; private set; } = State.NOT_RUNNING;

        public enum EaseType { LINEAR, EASE_OUT, EASE_IN, EASE_IN_OUT }
        EaseType selectedEaseTyped = EaseType.LINEAR;

        /*
        Delegate onComplete;
        object[] onCompleteArgs;
        */

        public Signal OnComplete { get; private set; } = new Signal();

        int fromInt;
        int toInt;

        float fromFloat;
        float toFloat;

        Vector2 fromVector2;
        Vector2 toVector2;

        float duration;
        float currentDuration;

        ///<summary>
        /// Set the values for the tween, but will not start it. Accepted values for _from and _to are floats, vector2s, and ints. Start tween with Start function
        ///</summary>
        public void SetTween<T>(T _from, T _to, float _duration, EaseType _easeType = EaseType.LINEAR)
        {
            if (_from is int)
            {
                fromInt = (int)(object)_from;
                toInt = (int)(object)_to;
            }
            else if (_from is float)
            {
                fromFloat = (float)(object)_from;
                toFloat = (float)(object)_to;
            }
            else if (_from is Vector2)
            {
                fromVector2 = (Vector2)(object)_from;
                toVector2 = (Vector2)(object)_to;
            }
            else
            {
                Console.Error.WriteLine("ERROR: Value type not accepted in tweens");
            }
            
            duration = _duration;
        }

        public void Start() 
        {
            CurrentState = State.RUNNING;
        }

        public void Stop() 
        {
            CurrentState = State.NOT_RUNNING;
            currentDuration = 0;
            // onComplete = null;
            // onCompleteArgs = null;
            // TODO null out all values
        }

        public void Pause() 
        {
            CurrentState = State.PAUSED;
        }

        public bool IsRunning()
        {
            if (CurrentState == State.RUNNING)
                return true;
            else
                return false;
        }

        ///<summary>
        /// Returns a tweened value of a float until progress is complete
        ///</summary>
        public float TweenFloat(float _currentAmount, float deltaTime)
        {
            float progress = SetProcessByEaseType(currentDuration);

            progress = Math.Clamp(progress, 0, 1);

            if (progress < 1)
            {
                currentDuration += deltaTime;
                float tweenAmount = toFloat - _currentAmount;
                return _currentAmount + progress * tweenAmount;
            }
            else
            {
                OnComplete.Emit();
                Stop();
            }

            return toFloat;
        }

        ///<summary>
        /// Returns a tweened value of a Vector2 until progress is complete
        ///</summary>
        public Vector2 TweenVector2(Vector2 _currentAmount, float deltaTime)
        {
            float progress = SetProcessByEaseType(currentDuration);
            progress = Math.Clamp(progress, 0, 1);

            if (progress < 1)
            {
                currentDuration += deltaTime;
                return Vector2.Lerp(fromVector2, toVector2, progress);
            }
            else
            {
                OnComplete.Emit();
                Stop();
            }

            return toVector2;
        }

        ///<summary>
        /// A branching pathway to choose the correct EaseType
        ///</summary>
        private float SetProcessByEaseType(float _currentDuration)
        {
            switch(selectedEaseTyped)
            {
                case EaseType.LINEAR:
                {
                    return Linear(_currentDuration);
                }
                case EaseType.EASE_IN:
                {
                    return EaseIn(_currentDuration);
                }
                case EaseType.EASE_OUT:
                {
                    return EaseOut(_currentDuration);
                }
                case EaseType.EASE_IN_OUT:
                {
                    return EaseInOut(_currentDuration);
                }
                default:
                {
                    return 0;
                }
            }
        }

        private float Linear(float _currentDuration)
        {
            return _currentDuration / duration;
        }

        private float EaseIn(float _currentDuration)
        {
            return 1 - (float)Math.Cos((_currentDuration * Math.PI) / 2);
        }

        private float EaseOut(float _currentDuration)
        {
            return (float)Math.Sin((_currentDuration * Math.PI) / 2);
        }
        
        private float EaseInOut(float _currentDuration)
        {
            return (float)-(Math.Cos(Math.PI * _currentDuration) - 1) / 2;
        }
    }
}