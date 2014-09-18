using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace EasySocket
{
	public class HeaderedProxy<W> where W : Wrapper, new()
	{
		private W source;
		private W target;
		private bool running;
		private Socket socketSource;
		private Socket socketTarget;

		public HeaderedProxy(Socket source, Socket target)
		{
			this.socketSource = source;
			this.source = new W();
			this.source.OnReceived += source_OnReceived;

			this.socketTarget = target;
			this.target = new W();
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
