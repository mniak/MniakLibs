using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace EasySocket
{
	public class Proxy<W1, W2> : IDisposable
		where W1 : Wrapper, new()
		where W2 : Wrapper, new()
	{
		public W1 SourceWrapper { get; private set; }
		public W2 TargetWrapper { get; private set; }
		private bool running;
		private Socket socketSource;
		private Socket socketTarget;
		private AutoResetEvent lockEnd = new AutoResetEvent(false);

		public Proxy(Socket source, Socket target)
		{
			this.socketSource = source;
			this.SourceWrapper = new W1();
			this.SourceWrapper.OnReceived += source_OnReceived;
			this.SourceWrapper.OnConnectionClosed += source_OnConnectionClosed;

			this.socketTarget = target;
			this.TargetWrapper = new W2();
			this.TargetWrapper.OnReceived += target_OnReceived;
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

		private void source_OnReceived(byte[] bytes)
		{
			if (running)
				this.TargetWrapper.Send(bytes);
		}
		private void target_OnReceived(byte[] bytes)
		{
			if (running)
				this.SourceWrapper.Send(bytes);
		}
		public void Start()
		{
			running = true;
			lockEnd.Reset();
			SourceWrapper.Start(socketSource);
			TargetWrapper.Start(socketTarget);
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
		}
		public void WaitEnd()
		{
			lockEnd.WaitOne();
		}
	}
}
