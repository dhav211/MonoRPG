using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MonoRPG
{
    public class Signal
    {

        List<Listener> listeners = new List<Listener>();
        SignalAwaiter awaiter = new SignalAwaiter();
        bool isEmitted = false;

        ///<summary>
        /// Add a listener to signal
        ///</summary>
        public void Add(string _listerName, Delegate _action)
        {
            listeners.Add(new Listener(_listerName, _action, false, listeners));
        }

        ///<summary>
        /// Add a listener to a signal which will be removed the moment it is emitted
        ///</summary>
        public void AddOnce(string _listerName, Delegate _action)
        {
            listeners.Add(new Listener(_listerName, _action, true, listeners));
        }

        ///<summary>
        /// Listeners will be notified and they will run their set functions with given parameters
        ///</summary>
        public void Emit(params object[] _args)
        {
            awaiter.NotifyAwaiter();

            foreach (Listener listener in listeners)
                listener.Execute(_args);
        }

        ///<summary>
        /// Remove a listener by searching for it's name is the list of listeners
        ///</summary>
        public void RemoveListener(string _name)
        {
            foreach (Listener listener in listeners)
            {
                if (listener.Name == _name)
                    listeners.Remove(listener);
            }
        }

        public async Task Wait()
        {
            while(!awaiter.IsEmitted)
            {
                await Task.Delay(1);
            }
        }

        public class Listener
        {
            public string Name { get; private set; }
            Delegate action;
            bool removeAfterUse = false;
            List<Listener> listeners;

            public Listener(string _name, Delegate _action, bool _remove, List<Listener> _listeners)
            {
                Name = _name;
                action = _action;
                removeAfterUse = _remove;
                listeners = _listeners;
            }

            public void Execute(params object[] _args)
            {
                action.DynamicInvoke(_args);

                if (removeAfterUse)
                    listeners.Remove(this);
            }
        }

        public class SignalAwaiter
        {
            // TODO there should be a timeout or a limit of awaiters. Since this could open too many threads and cause problems down the line
            public bool IsEmitted { get; set; }
            public int Awaiters { get; set; }

            public async void NotifyAwaiter()
            {
                if (Awaiters == 0)
                    return;
                
                IsEmitted = true;

                await Task.Delay(3);

                IsEmitted = false;
                Awaiters = 0;
            }
        }
    }
}