using Mniak.Network.Events;
using Mniak.Network.Wrappers;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Mniak.Network
{
    public abstract class Wrapper : IDisposable
    {
        protected Socket socket;
        protected bool running;
        protected object lockRunning = new object();
        private Thread thread;
        private bool disposed = false;

        public event EventHandler<DataReceivedEventArgs> OnDataReceived;

        public Wrapper(Socket socket)
        {
            this.running = false;

            this.Encoding = Encoding.Default;
            this.socket = socket;
        }
        public void Start()
        {
            CheckDisposed();
            lock (lockRunning)
            {
                if (running)
                    throw new NetworkException("This wrapper is already running.");
                running = true;
            }
            thread = new Thread(Read);
            thread.Name = "Wrapper Read (" + thread.ManagedThreadId + ")";
            thread.Start();
        }

        private void CheckDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException("Wrapper");
        }
        public void Stop()
        {
            lock (lockRunning)
                running = false;
        }

        protected void Received(byte[] bytes)
        {
            if (running && OnDataReceived != null)
            {
                OnDataReceived(this, new DataReceivedEventArgs(bytes, this.Encoding));
            }
        }
        private void Read()
        {
            try
            {
                while (running)
                {
                    try
                    {
                        byte[] bytes = Receive();
                        if (bytes == null)
                        {
                            Stop();
                            ConnectionClosed();
                        }

                        Received(bytes);
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                }
            }
            catch (SocketException)
            {
            }
            catch (ThreadAbortException)
            {
                // Cancel the abortation process but aborts
                Thread.ResetAbort();
                return;
            }
            finally
            {
                ConnectionClosed();
            }
        }

        protected abstract byte[] Receive();
        public void Send(byte[] data)
        {
            try
            {
                if (!running)
                    throw new WrapperNotRunningException();
                InnerSend(data);
            }
            catch (SocketException ex)
            {
                switch (ex.ErrorCode)
                {
                    case 10053:
                    case 10054:
                        this.Stop();
                        this.ConnectionClosed();
                        break;

                    default:
                        throw;
                }
            }
        }
        public void Send(string data)
        {
            Send(this.Encoding.GetBytes(data));
        }

        protected abstract void InnerSend(byte[] bytes);

        public void CloseAndDisposeSocket()
        {
            this.Stop();
            if (socket.Connected)
                socket.Disconnect(false);
            socket.Close();
            socket.Dispose();
        }
        public virtual void Dispose()
        {
            Stop();
            disposed = true;
        }

        private bool closed;
        private void ConnectionClosed()
        {
            if (closed) return;
            else closed = true;
            OnConnectionClosed?.Invoke();
        }
        public event Action OnConnectionClosed;

        public Encoding Encoding { get; set; }

        //private void AcceptConnections()
        //{
        //    //CheckDisposed();
        //    //var wrapperType = this.GetType();

        //    //while (this.running)
        //    //{
        //    //    Socket inSocket = this.socket.Accept();
        //    //    var inWrapper = (Wrapper)Activator.CreateInstance(wrapperType);
        //    //    inWrapper.Start(inSocket);
        //    //    Router rtargeter = new Router(inSocket, targetAddr, targetPort);
        //    //    Thread thread = new Thread(rtargeter.Run);
        //    //    thread.Name = "Router";
        //    //    thread.Start();
        //    //}
        //}
    }
}
