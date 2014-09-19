using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace EasySocket
{
	public class HeaderedProxy<W1, W2> : IDisposable
		where W1 : Wrapper, new()
		where W2 : Wrapper, new()
	{
		private W1 source;
		private W2 target;
		private bool running;
		private Socket socketSource;
		private Socket socketTarget;

		public HeaderedProxy(Socket source, Socket target)
		{
			this.socketSource = source;
			this.source = new W1();
			this.source.OnReceived += source_OnReceived;

			this.socketTarget = target;
			this.target = new W2();
			this.target.OnReceived += target_OnReceived;
		}

		private void source_OnReceived(byte[] bytes)
		{
			if (running)
				this.target.Send(bytes);
		}
		private void target_OnReceived(byte[] bytes)
		{
			if (running)
				this.source.Send(bytes);
		}
		public void Start()
		{
			running = true;
			source.Start(socketSource);
			target.Start(socketTarget);
		}
		public void Stop()
		{
			running = false;
			source.Stop();
			target.Stop();
		}
		public void Dispose()
		{
			Stop();
		}
	}
}
