using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using EasySocket.Exception;

namespace EasySocket
{
	public class HeaderedWrapper : Wrapper
	{
		private int headerLength = 4;
		public int HeaderLength
		{
			get { return headerLength; }
			set
			{
				if (value < 1 || value > 4)
					throw new ArgumentOutOfRangeException("The value of HeaderLength must be in the range of 1 to 4.");
				HeaderLength = value;
			}
		}

		public override void Send(byte[] bytes)
		{
			if (running)
			{
				byte[] header = bytes.GetHeader(HeaderLength);
				byte[] tudo = header.Unir(bytes);
				socket.Send(tudo);
			}
		}

		protected override byte[] Receive()
		{
			ulong tamanho = ReadHeader();
			byte[] bytes = ReadBody(tamanho);
			return bytes;
		}
		private ulong ReadHeader()
		{
			byte[] header = new byte[HeaderLength];
			while (socket.Available < HeaderLength && running)
			{
				Thread.Sleep(100);
			}

			// If it is not running then abort
			if (!running) return 0;
			int n = socket.Receive(header);

			if (n < HeaderLength) return 0;
			ulong tamanho = header.AsInteger();

			return tamanho;
		}
		private byte[] ReadBody(ulong length)
		{
			// If it is not running then abort
			if (!running || length < 1) return null;

			// Wait for all bytes available
			Helper.TimeoutLoop(() => (ulong)socket.Available < length && running, 5000);

			// If it is not running then abort
			if (!running) return null;

			byte[] buffer = new byte[length];
			int n = socket.Receive(buffer);
			if (n < HeaderLength) return null;

			return buffer;
		}
	}
}
