using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mniak.Core
{
    public sealed class ActionLock
    {
        private object internalLock = new object();

        public void Invoke(Action action)
        {
            lock (internalLock)
            {
                action.Invoke();
            }
        }
    }
}
