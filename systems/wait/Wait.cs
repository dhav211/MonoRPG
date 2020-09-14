using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoRPG
{
    public class Wait
    {

        ///<summary>
        /// Return from waiting when bool function returns the same as intendedResult
        ///</summary>
        public async Task WaitUntil(Func<bool> _condition, bool _intendedResult)
        {
            bool result = false;
            while (result != _intendedResult)
            {
                //await Task.Run(() => result = _condition());
                result = _condition();
                await Task.Delay(1);
            }
        }

        ///<summary>
        /// Return from waiting when given time in seconds as been met
        ///</summary>
        public async Task WaitUntil(float _timeInSeconds)
        {
            float currentTime = 0;

            while(currentTime < _timeInSeconds)
            {
                currentTime += Time.DeltaTime;
                await Task.Delay((int)(Time.DeltaTime * 1000));
            }
        }
    }
}