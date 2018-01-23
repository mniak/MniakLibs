using System;

namespace Mniak.Core
{
    public sealed class ActionLock
    {
        private object lockObject = new object();

        public void Invoke(Action action)
        {
            lock (lockObject)
            {
                action.Invoke();
            }
        }
    }
}