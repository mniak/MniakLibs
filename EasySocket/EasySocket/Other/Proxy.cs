using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using EasySocket.Events;
using EasySocket.Exceptions;

namespace EasySocket
{
    public class Proxy<W1, W2> : IDisposable, IProxy
        where W1 : Wrapper
        where W2 : Wrapper
    {
        public W1 SourceWrapper { get; private set; }
        public W2 TargetWrapper { get; private set; }
        private bool running;
        private Socket socketSource;
        private Socket socketTarget;
        private AutoResetEvent lockEnd = new AutoResetEvent(false);

        public Proxy(Socket source, Socket target, Func<Socket, W1> newWrapper1, Func<Socket, W2> newWrapper2)
        {
            this.socketSource = source;
            this.SourceWrapper = newWrapper1(source);
            this.SourceWrapper.OnDataReceived += source_OnDataReceived;
            this.SourceWrapper.OnConnectionClosed += source_OnConnectionClosed;

            this.socketTarget = target;
            this.TargetWrapper = newWrapper2(target);
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

        Wrapper IProxy.SourceWrapper
        {
            get { return this.SourceWrapper; }
        }
        Wrapper IProxy.TargetWrapper
        {
            get { return this.TargetWrapper; }
        }
    }
}
