using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using EasySocket.Exception;
using System.Threading;

namespace EasySocket
{
	public abstract class Wrapper : IDisposable
	{
		protected Socket socket;
		protected bool running;
		private Thread thread;
		public Wrapper()
		{
			this.running = false;
		}

		public void Start(Socket socket)
		{
			this.socket = socket;
			if (running)
				throw new EasySocketException("This wrapper is already running.");
			running = true;
			thread = new Thread(Process);
			thread.Name = "Wrapper_Read";
			thread.Start();
		}
		public void Stop()
		{
			running = false;
		}

		public delegate void RecebidoEventHandler(byte[] bytes);
		public event RecebidoEventHandler OnReceived;

		protected void Received(byte[] bytes)
		{
			if (running && OnReceived != null)
			{
				OnReceived(bytes);
			}
		}
		private void Process()
		{
			try
			{
				while (running)
				{
					byte[] bytes = Receive();
					if (bytes != null)
						Received(bytes);
				}
			}
			catch (ThreadAbortException)
			{
				// Cancel the abortation process but aborts
				Thread.ResetAbort();
				return;
			}
		}
		protected abstract byte[] Receive();
		public abstract void Send(byte[] bytes);

		public void Dispose()
		{
			this.Stop();
		}
	}
}
