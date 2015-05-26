using Mniak.Network.Events;
using Mniak.Network.Wrappers;
using System;
using System.Net.Sockets;
using System.Threading;

namespace Mniak.Network
{
    public class Forwarder : IDisposable, IForwarder
    {
        public Wrapper SourceWrapper { get; private set; }
        public Wrapper TargetWrapper { get; private set; }
        private bool running;
        private AutoResetEvent lockEnd = new AutoResetEvent(false);

        public Forwarder(Wrapper wrapper1, Wrapper wrapper2)
        {
            this.SourceWrapper = wrapper1;
            this.SourceWrapper.OnDataReceived += source_OnDataReceived;
            this.SourceWrapper.OnConnectionClosed += source_OnConnectionClosed;

            this.TargetWrapper = wrapper2;
            this.TargetWrapper.OnDataReceived += target_OnDataReceived;
            this.TargetWrapper.OnConnectionClosed += target_OnConnectionClosed;
        }

        void target_OnConnectionClosed()
        {
            this.Stop();
        }
        void source_OnConnectionClosed()
        {
            this.Stop();
        }

        private void source_OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {
                if (running)
                    this.TargetWrapper.Send(e.DataBytes);
            }
            catch (WrapperNotRunningException ex)
            {
                this.Dispose();
            }
        }
        private void target_OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {
                if (running)
                    this.SourceWrapper.Send(e.DataBytes);
            }
            catch (WrapperNotRunningException ex)
            {
                this.Dispose();
            }
        }
        public void Start()
        {
            running = true;
            lockEnd.Reset();
            SourceWrapper.Start();
            TargetWrapper.Start();
        }
        public void Stop()
        {
            running = false;
            SourceWrapper.Stop();
            TargetWrapper.Stop();
            lockEnd.Set();
        }
        public void Dispose()
        {
            Stop();
            SourceWrapper.Dispose();
            TargetWrapper.Dispose();
        }
        public void WaitEnd()
        {
            lockEnd.WaitOne();
        }

        Wrapper IForwarder.SourceWrapper
        {
            get { return this.SourceWrapper; }
        }
        Wrapper IForwarder.TargetWrapper
        {
            get { return this.TargetWrapper; }
        }
    }
}
