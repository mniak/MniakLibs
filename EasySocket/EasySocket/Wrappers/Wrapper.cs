using System;
using System.Net.Sockets;
using System.Threading;
using EasySocket.Exceptions;
using System.IO;
using System.IO.Compression;

namespace EasySocket
{
	public abstract class Wrapper : IDisposable
	{
		protected Socket socket;
		protected bool running;
		protected object lockRunning = new object();
		private Thread thread;
		private bool disposed = false;
		public Wrapper()
		{
			lock (lockRunning)
				this.running = false;
		}
		public void Start(Socket socket)
		{
			if (disposed)
				throw new ObjectDisposedException("Wrapper");

			this.socket = socket;
			lock (lockRunning)
			{
				if (running)
					throw new EasySocketException("This wrapper is already running.");
				running = true;
			}
			thread = new Thread(Read);
			thread.Name = "Wrapper_Read";
			thread.Start();
		}
		public void Stop()
		{
			lock (lockRunning)
				running = false;
		}

		public delegate void ReceivedEventHandler(byte[] bytes);
		public event ReceivedEventHandler OnReceived;

		protected void Received(byte[] bytes)
		{
			if (running && OnReceived != null)
			{
				OnReceived(bytes);
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
						if (bytes != null)
							Received(bytes);
					}
					catch (ObjectDisposedException)
					{
					}
				}
			}
			catch (SocketException ex)
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
		public void Send(byte[] bytes)
		{
			if (!running)
				throw new WrapperNotRunningException();
			InnerSend(bytes);
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

		private void ConnectionClosed()
		{
			if (OnConnectionClosed != null)
				OnConnectionClosed();
		}
		public event Action OnConnectionClosed;

	}
}
