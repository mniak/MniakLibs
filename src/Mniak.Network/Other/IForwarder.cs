using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mniak.Network
{
    public interface IForwarder
    {
        Wrapper SourceWrapper { get; }
        Wrapper TargetWrapper { get; }
        void Start();
        void Stop();
        void WaitEnd();
    }
}
